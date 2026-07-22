using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    [SerializeField] float detectRange = 10f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float throwRange = 6f;

    float attackTimer;
    float searchTimer;
    float searchInterval = 0.3f;

    Character target;
    Weapon targetWeapon;

    void Start()
    {
        
    }

    void Update()
    {
        if (isDead)
            return;

        if(searchTimer >= searchInterval)
        {
            searchTimer = 0;

            FindTarget();

            if (currentWeapon == null)
                FindWeapon();
        }
        
        if (target != null)
            Debug.DrawLine(transform.position, target.transform.position, Color.red);

        Move();

        RotateHand();

        Action();

        attackTimer += Time.deltaTime;
        searchTimer += Time.deltaTime;
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
        float minDist = detectRange;
        target = null;

        foreach (Character ch in CharacterManager.Instance.Characters.Values)
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

    void FindWeapon()
    {
        float minDist = detectRange;
        targetWeapon = null;

        foreach (Weapon weapon in WeaponManager.Instance.Weapons.Values)
        {
            if (weapon.transform.parent != null)
                continue;

            float dist = Vector2.Distance(transform.position, weapon.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                targetWeapon = weapon;
            }
        }
    }

    void Move()
    {
        if (currentWeapon == null)
        {
            if (targetWeapon == null)
            {
                dir = Vector2.zero;
                return;
            }

            lookDir = (targetWeapon.transform.position - transform.position).normalized;
            dir = lookDir;
            return;
        }

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
    void Action()
    {
        if (currentWeapon == null)
        {
            PickupWeapon();
            return;
        }

        if (target == null)
            return;

        float dist =
            Vector2.Distance(transform.position, target.transform.position);

        WeaponInfo info =
            DataManager.instance.GetWeaponData(currentWeapon.WeaponID);

        if (attackTimer < info.attackDelay)
            return;

        attackTimer = 0f;

        if (dist <= attackRange)
        {
            currentWeapon.Attack();
        }
        else if (dist <= throwRange)
        {
            currentWeapon.Throw(lookDir, col);
            currentWeapon = null;
        }
    }

    protected override void Die()
    {

    }
    protected override void Init()
    {
        
    }

}
