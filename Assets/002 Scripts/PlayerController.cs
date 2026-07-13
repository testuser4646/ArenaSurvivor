using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    Rigidbody2D rb;
    Collider2D col;

    Vector2 dir;
    float moveSpeed;

    //[SerializeField] HPBar hpBar;

    //Animator animator;

    int isMove;
    bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        //animator = GetComponent<Animator>();
    }
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        dir = Vector2.zero;
        if (isDead)
            return;

        if (Keyboard.current.wKey.isPressed)
        {
            dir += Vector2.up;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            dir += Vector2.left;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            dir += Vector2.down;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            dir += Vector2.right;
        }
        dir = dir.normalized;
        //if (dir == Vector2.zero)
        //{
        //    animator.SetBool(isMove, false);
        //}
        //else
        //{
        //    animator.SetBool(isMove, true);
        //}
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = dir * moveSpeed;
    }


    public override void TakeDamage(float damage)
    {
        nowHP -= damage;
        //hpBar.SetGauge(nowHP / maxHP);
        if (nowHP < 0)
        {
            nowHP = 0;
            Die();
        }
    }

    protected override void Die()
    {
        isDead = true;
        dir = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        //animator.SetBool("isDead", isDead);
        //StageManager.Instance.StopSpawn();
        //GameManager.Instance.GameOver();
    }

    protected override void Init()
    {
        moveSpeed = 10f;
        maxHP = 10;
        nowHP = maxHP;
        isMove = Animator.StringToHash("isMove");
        isDead = false;

        //hpBar.SetGauge(1f);

        isDead = false;
        //animator.SetBool("isDead", false);
        //animator.Play("Idle");

        transform.position = Vector3.zero;

        rb.linearVelocity = Vector2.zero;
    }

}
