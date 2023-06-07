using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_movePower = 10f;
    [SerializeField] float m_maxMovePower = 10f;
    [SerializeField] float m_jumpPower = 5f;
    [SerializeField] AudioClip m_hurtAudioClip = default;
    [SerializeField] AudioClip m_deathAudioClip = default;
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
        m_initialPosition = this.transform.position;
    }

    void Update()
    {
            m_Renderer.material.color = Color.HSVToRGB(Time.time % 1, 1,1);
        
        if(Input.GetButton("Jump"))
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
            m_rigidbody.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);//Impulse‚É‚·‚é‚Æ”š”­“I‚É”ò‚Ô
        }

        if(Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay((Vector2)ray.origin, (Vector2)ray.direction);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if(hit2d.collider.CompareTag("Holdable") && Vector2.Distance((Vector2)ray.origin, (Vector2)transform.position) < 2 && !isHolding)
            {
                float x = transform.position.x;
                float y = transform.position.y;
                hit2d.collider.GetComponent<Rigidbody2D>().isKinematic = true;
                hit2d.collider.enabled = false;
                hit2d.collider.transform.SetParent(transform);
                hit2d.collider.transform.position = new Vector3(x, y+1, 0);
                isHolding = true;
            }
        }

        if(Input.GetButtonDown("Fire2"))
        {
            Transform child = transform.Find("MoveableBlock");
            if(child != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 forward = Vector3.Scale((mousePos - child.position), new Vector3(1, 1, 0)).normalized;
                Rigidbody2D cRigidbody = child.GetComponent<Rigidbody2D>();
                cRigidbody.isKinematic = false;
                cRigidbody.velocity = forward * child.GetComponent<MoveableBlockController>()._throwSpeed;
                child.GetComponent<Collider2D>().enabled = true;
                child.SetParent(null);
                isHolding = false;
            }
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
    void FixedUpdate()//‰¡ˆÚ“®
    {
        if(m_rigidbody.velocity.magnitude < m_maxMovePower)
        {
            if(!isGrounded)
            {
                m_rigidbody.AddForce(Vector2.right * m_horizontal * m_movePower / 2, ForceMode2D.Force);
            }
            else m_rigidbody.AddForce(Vector2.right * m_horizontal * m_movePower, ForceMode2D.Force);

        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Plank")
        {
            transform.SetParent(collision.transform);
        }
        if(collision.gameObject.tag == "Ladder")
        {
            isCliming = true;
        }
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Plank")
        {
            transform.SetParent(null);
        }
        if (collision.gameObject.tag == "Ladder")
        {
            isCliming = false;
        }
        isGrounded = false;
    }
}
