using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_maxMovePower = 10f;
    [SerializeField] float m_jumpPower = 5f;
    [SerializeField] Tilemap m_tilemap;
    [SerializeField] Slider m_hpSlider;
    Image m_holdItem;
    GameObject m_selectionTile;
    GameObject m_gameCursor;
    GameObject m_arrow;
    GameObject m_throwArrow;
    Text m_arrowMessage;
    Text _hpTransition;
    GameObject m_vanner;
    Animator m_Animator;
    Rigidbody2D m_rigidbody = default;
    float m_horizontal;
    Vector3 m_initialPosition;
    bool isGrounded = false;
    bool isHolding = false;
    bool isCliming = false;
    float m_interval = 0.1f;
    float m_timer = 0.0f;
    float _hpTransitionTimer, _hpTransitionInterval = 1.0f;
    List<Vector3> _linePoints = new List<Vector3>();
    LineRenderer _lineRenderer;
    MyInputs _myInputs;     // Input System
    /// <summary>マウスの座標を記録</summary>
    Vector3 m_mousePos;
    PlayerInput _playerInput;
    GameObject _target;
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
    private void Awake()
    {
        _myInputs = new MyInputs();
        _myInputs.Enable();
    }
    private void OnDestroy()
    {
        _myInputs?.Dispose();
    }
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_selectionTile = GameObject.Find("SelectionTile");
        m_gameCursor = GameObject.Find("GameCursor");
        m_arrow = GameObject.Find("Player/WorldCanvas/Arrow");
        m_throwArrow = GameObject.Find("Player/WorldCanvas/ThrowArrow");
        m_arrowMessage = GameObject.Find("Player/WorldCanvas/ArrowMessage").GetComponent<Text>();
        m_vanner = GameObject.Find("Vanner");
        m_holdItem = GameObject.Find("DisplayCanvas/Inventory/HoldItem").GetComponent<Image>();
        _hpTransition = GameObject.Find("DisplayCanvas/HPTransition").GetComponent<Text>();
        if (m_throwArrow) m_throwArrow.GetComponent<Image>().enabled = false;
        if (m_selectionTile) m_selectionTile.SetActive(false);
        if (m_holdItem) m_holdItem.enabled = false;
        if (_hpTransition) _hpTransition.text = "";
        m_initialPosition = this.transform.position;
        m_hpSlider.value = GManager.instance.hpDefault;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (_playerInput.currentControlScheme == "Keyboard & Mouse")
        {
            m_mousePos = _myInputs.Player.MouseCursor.ReadValue<Vector2>();
        }
        if (_playerInput.currentControlScheme == "GamePad")
        {
            m_mousePos += (Vector3)_myInputs.Player.VirtualCursor.ReadValue<Vector2>();
        }
        CursorLock();
        m_timer += Time.deltaTime;
        _hpTransitionTimer += Time.deltaTime;
        m_horizontal = _myInputs.Player.Horizontal.ReadValue<float>();
        Vector3 worldMousePoint = Camera.main.ScreenToWorldPoint(m_mousePos);
        if (m_gameCursor) m_gameCursor.transform.position = (Vector2)worldMousePoint;
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
        if (_hpTransitionTimer > _hpTransitionInterval) _hpTransition.text = "";
        Vector3Int gridPos = m_tilemap.WorldToCell(worldMousePoint);
        Vector3 completePos = new Vector3(m_tilemap.cellSize.x / 2, m_tilemap.cellSize.y / 2, 0);
        Vector3 worldPos = m_tilemap.CellToWorld(gridPos) + completePos;
        m_selectionTile.SetActive(true);
        m_selectionTile.transform.position = worldPos;
        if (_myInputs.Player.Jump.IsPressed() && (isCliming))
        {
            m_rigidbody.velocity = new Vector2(0, 5);
        }
        else if (_myInputs.Player.Jump.triggered && (isGrounded))
        {
            m_rigidbody.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);//Impulseにすると爆発的に飛ぶ
        }

        Ray ray = Camera.main.ScreenPointToRay(m_mousePos);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
        bool canHold = hit2d.collider && hit2d.collider.CompareTag("Holdable") && Vector2.Distance((Vector2)ray.origin, (Vector2)transform.position) < 2 && !isHolding;
        if (canHold)
        {
            _target = hit2d.collider.gameObject;
            _target.GetComponent<HoldableItem>().SwitchHighLight(true);
        }
        else if(_target != null)
        {
            _target.GetComponent<HoldableItem>().SwitchHighLight(false);
            _target = null;
        }
        if (_myInputs.Player.Interact.triggered)
        {
            m_selectionTile.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 0.2f);
            m_timer = 0.0f;
    
            if (canHold)
            {
                float x = transform.position.x;
                float y = transform.position.y;
                hit2d.collider.GetComponent<Rigidbody2D>().isKinematic = true;
                hit2d.collider.enabled = false;
                hit2d.collider.transform.SetParent(transform);
                hit2d.collider.transform.position = new Vector3(x, y + 1, 0);
                m_holdItem.enabled = true;
                m_holdItem.sprite = hit2d.collider.GetComponent<SpriteRenderer>().sprite;
                isHolding = true;
            }
        }

        if (_myInputs.Player.Throw.triggered)
        {
            if (transform.childCount >= 3)
            {
                Transform child = transform.GetChild(2);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(m_mousePos);
                Vector3 forward = Vector3.Scale((mousePos - child.position), new Vector3(1, 1, 0)).normalized;
                Rigidbody2D cRigidbody = child.GetComponent<Rigidbody2D>();
                cRigidbody.isKinematic = false;
                cRigidbody.velocity = forward * child.GetComponent<HoldableItem>()._throwSpeed;
                child.GetComponent<Collider2D>().enabled = true;
                child.GetComponent<HoldableItem>()._projectile = true;
                child.SetParent(null);
                m_holdItem.enabled = false;
                isHolding = false;
            }
        }

        if (isHolding)
        {
            _lineRenderer.enabled = true;
            Transform child = transform.GetChild(2);
            var speed = child.GetComponent<HoldableItem>()._throwSpeed;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(m_mousePos);
            Vector3 forward = Vector3.Scale((mousePos - child.position), new Vector3(1, 1, 0)).normalized;
            m_throwArrow.GetComponent<Image>().enabled = true;
            ThrowLine(child.position, (Vector2)forward * speed);
        }
        else
        {
            _lineRenderer.enabled = false;
            m_throwArrow.GetComponent<Image>().enabled = false;
        }

        if (this.transform.position.y < -20f)
        {
            GManager.instance.Hp -= 5;
            HPTransition(-5);
            HurtPlay();
            this.transform.position = m_initialPosition;
        }

    }

    public void HurtPlay()
    {
        m_Animator.Play("PlayerHurt");
        GManager.instance.PlaySound(2);
    }

    public void HPTransition(int trans)
    {
        string transText = $"{trans}";
        if (trans > 0)
        {
            _hpTransition.color = Color.green;
            transText = "+" + trans.ToString();
        }
        if (trans < 0) _hpTransition.color = Color.blue;
        _hpTransition.text = transText;
        _hpTransitionTimer = 0.0f;
    }
    void ThrowLine(Vector2 pos, Vector2 vector)
    {
        int maxLine = 200;
        _linePoints.Clear();
        Vector2 gravity = Physics2D.gravity * Time.fixedDeltaTime;
        Vector2 currentVector = vector;
        Vector2 currentPos = pos;
        for (int i = 0; i < maxLine; i++)
        {
            Vector2 nextPos = currentPos + (currentVector * Time.fixedDeltaTime);
            currentVector += gravity;
            _linePoints.Add(nextPos);
            currentPos = nextPos;
        }
        _lineRenderer.positionCount = _linePoints.Count;
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }

    void CursorLock()
    {
        float x = m_mousePos.x, y = m_mousePos.y;

        if (m_mousePos.x < 0)
        {
            x = 0;
        }
        else if (m_mousePos.x > Screen.width)
        {
            x = Screen.width;
        }

        if (m_mousePos.y < 0) { y = 0; } else if (m_mousePos.y > Screen.height) { y = Screen.height; }

        m_mousePos = new Vector3(x, y, 0);
    }
    void FixedUpdate()//横移動
    {
        if (m_rigidbody.velocity.magnitude > m_maxMovePower)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_maxMovePower;
        }
        if (!isGrounded)
        {
            m_rigidbody.AddForce(Vector2.right * m_horizontal * m_maxMovePower / 2, ForceMode2D.Force);
        }
        else m_rigidbody.AddForce(Vector2.right * m_horizontal * m_maxMovePower, ForceMode2D.Force);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isCliming = true;
        }
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isCliming = false;
        }
        isGrounded = false;
    }


}
