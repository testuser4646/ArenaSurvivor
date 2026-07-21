using UnityEngine;

public class EnemyController : Character
{
    [SerializeField] float detectRange = 10f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float throwRange = 6f;

    Character target;

    Character[] characters;
    void Start()
    {
        
    }

    void Update()
    {
        if (isDead)
            return;

        FindTarget();
        if (target != null)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        }
        Move();

        //RotateHand();

        //Action();
    }

    protected override void FixedUpdate()
    {
        Vector2 targetVelocity = dir * moveSpeed/10;

        float accel =
            dir == Vector2.zero ? 100f : 40f;

        rb.linearVelocity =
            Vector2.MoveTowards(
                rb.linearVelocity,
                targetVelocity,
                accel * Time.fixedDeltaTime);
    }

    void FindTarget()
    {
        characters = FindObjectsByType<Character>(FindObjectsSortMode.None);

        float minDist = detectRange;
        target = null;

        foreach (Character ch in characters)
        {
            if (ch == this)
                continue;

            float dist = Vector2.Distance(transform.position, ch.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                target = ch;
            }
        }
    }

    void Move()
    {
        if (target == null)
        {
            dir = Vector2.zero;
            return;
        }

        lookDir = (target.transform.position - transform.position).normalized;

        float dist = Vector2.Distance(transform.position, target.transform.position);

        if (dist > attackRange)
            dir = lookDir;
        else
            dir = Vector2.zero;
    }

    protected override void Die()
    {

    }
    protected override void Init()
    {
        
    }

}
