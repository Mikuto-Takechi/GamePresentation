using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_maxMovePower = 10f;
    [SerializeField] float m_jumpPower = 5f;
    [SerializeField] AudioClip m_hurtAudioClip = default;
    [SerializeField] AudioClip m_deathAudioClip = default;
    [SerializeField] Tilemap m_tilemap;
    [SerializeField] Slider m_hpSlider;
    Image m_holdItem;
    GameObject m_selectionTile;
    GameObject m_arrow;
    GameObject m_throwArrow;
    Text m_arrowMessage;
    GameObject m_vanner;
    Animator m_Animator;
    AudioSource m_AudioSource;
    Rigidbody2D m_rigidbody = default;
    Renderer m_Renderer;
    float m_horizontal;
    Vector3 m_initialPosition;
    bool isGrounded = false;
    bool isHolding = false;
    bool isCliming = false;
    bool jumpButtonFlag = false;
    bool rainbow = true;
    float m_interval = 0.1f;
    float m_timer = 0.0f;
    public Vector3 InitialPosition
    {
        set { m_initialPosition = value; }
        get { return m_initialPosition; }
    }
    public bool IsHolding
    {
        set { isHolding = value; }
        get { return isHolding; }
    }
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_Renderer = GetComponent<Renderer>();
        m_selectionTile = GameObject.Find("SelectionTile");
        m_arrow = GameObject.Find("Player/WorldCanvas/Arrow");
        m_throwArrow = GameObject.Find("Player/WorldCanvas/ThrowArrow");
        m_arrowMessage = GameObject.Find("Player/WorldCanvas/ArrowMessage").GetComponent<Text>();
        m_vanner = GameObject.Find("Vanner");
        m_holdItem = GameObject.Find("DisplayCanvas/Inventory/HoldItem").GetComponent<Image>();
        if(m_throwArrow)
        {
            m_throwArrow.GetComponent<Image>().enabled = false;
        }
        if (m_selectionTile)
        {
            m_selectionTile.SetActive(false);
        }
        if(m_holdItem)
        {
            m_holdItem.enabled = false;
        }
        m_initialPosition = this.transform.position;
        m_hpSlider.value = GManager.instance.hpDefault;
    }

    void Update()
    {
        m_timer += Time.deltaTime;
        Vector3 worldMousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Color rainbowColor = Color.HSVToRGB(Time.time % 1, 1, 1);//虹色
        rainbowColor.a = 0.8f;//不透明度
        m_hpSlider.value = (float)GManager.instance.Hp / (float)GManager.instance.hpDefault;
        m_arrow.transform.up = m_vanner.transform.position - transform.position;
        m_arrow.GetComponent<Image>().color = rainbowColor;
        m_throwArrow.transform.up = (Vector2)(worldMousePoint - transform.position);
        int distance = (int)Vector2.Distance((Vector2)transform.position, (Vector2)m_vanner.transform.position);
        m_arrowMessage.text = $"ゴールまで{distance}m";
        m_arrowMessage.color = rainbowColor;
        if (m_timer > m_interval) 
        {
            m_selectionTile.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            m_timer = 0.0f;
        }
        Vector3Int gridPos = m_tilemap.WorldToCell(worldMousePoint);
        Vector3 completePos = new Vector3(m_tilemap.cellSize.x / 2, m_tilemap.cellSize.y / 2, 0);
        Vector3 worldPos = m_tilemap.CellToWorld(gridPos) + completePos;
        m_selectionTile.SetActive(true);
        m_selectionTile.transform.position = worldPos;

        if (Input.GetButton("Jump"))
        {
            jumpButtonFlag = true;
        }
        else if(Input.GetButtonUp("Jump"))
        {
            jumpButtonFlag = false;
        }
        m_horizontal = Input.GetAxisRaw("Horizontal");
        if(jumpButtonFlag && (isCliming))
        {
            m_rigidbody.velocity = new Vector2(0, 5);
        }
        else if (Input.GetButtonDown("Jump") && (isGrounded))
        {
            m_rigidbody.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);//Impulseにすると爆発的に飛ぶ
        }

        if(Input.GetButtonDown("Fire1"))
        {
            m_selectionTile.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 0.2f);
            m_timer = 0.0f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if(hit2d.collider.CompareTag("Holdable") && Vector2.Distance((Vector2)ray.origin, (Vector2)transform.position) < 2 && !isHolding)
            {
                float x = transform.position.x;
                float y = transform.position.y;
                hit2d.collider.GetComponent<Rigidbody2D>().isKinematic = true;
                hit2d.collider.enabled = false;
                hit2d.collider.transform.SetParent(transform);
                hit2d.collider.transform.position = new Vector3(x, y+1, 0);
                m_holdItem.enabled = true;
                m_holdItem.sprite = hit2d.collider.GetComponent<SpriteRenderer>().sprite;
                isHolding = true;
            }
        }

        if(Input.GetButtonDown("Fire2"))
        {
            Transform child = transform.GetChild(2);
            if(child != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 forward = Vector3.Scale((mousePos - child.position), new Vector3(1, 1, 0)).normalized;
                Rigidbody2D cRigidbody = child.GetComponent<Rigidbody2D>();
                cRigidbody.isKinematic = false;
                cRigidbody.velocity = forward * child.GetComponent<MoveableBlockController>()._throwSpeed;
                child.GetComponent<Collider2D>().enabled = true;
                child.GetComponent<MoveableBlockController>()._projectile = true;
                child.SetParent(null);
                m_holdItem.enabled = false;
                isHolding = false;
            }
        }

        if (isHolding)
        {
            m_throwArrow.GetComponent<Image>().enabled = true;
        }
        else
        {
            m_throwArrow.GetComponent<Image>().enabled = false;
        }

        if (this.transform.position.y < -20f)
        {
            GManager.instance.Hp -= 5;
            HurtPlay();
            this.transform.position = m_initialPosition;
        }

    }

    public void HurtPlay()
    {
        m_Animator.Play("PlayerHurt");
        m_AudioSource.PlayOneShot(m_hurtAudioClip);
    }
    void FixedUpdate()//横移動
    {
        if (m_rigidbody.velocity.magnitude > m_maxMovePower)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_maxMovePower;
        }
            if(!isGrounded)
            {
                m_rigidbody.AddForce(Vector2.right * m_horizontal * m_maxMovePower / 2, ForceMode2D.Force);
            }
            else m_rigidbody.AddForce(Vector2.right * m_horizontal * m_maxMovePower, ForceMode2D.Force);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Plank")
        //{
        //    transform.SetParent(collision.transform);
        //}
        if (collision.gameObject.tag == "Ladder")
        {
            isCliming = true;
        }
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Plank")
        //{
        //    transform.SetParent(null);
        //}
        if (collision.gameObject.tag == "Ladder")
        {
            isCliming = false;
        }
        isGrounded = false;
    }
}
