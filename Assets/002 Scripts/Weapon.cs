using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int weaponID;
    public int WeaponID => weaponID;

    WeaponInfo info;
    public WeaponInfo Info => info;

    Rigidbody2D rb;
    Collider2D col;


    [SerializeField] Collider2D attackCol;
    [SerializeField] LayerMask targetLayer;


    public bool isEquipped = false;
    public bool isAttacking = false;
    public bool isThrowing = false;

    int bounceCount;
    float currentSpeed;
    float travelDistance;
    void OnEnable()
    {
        WeaponManager.Instance.Register(this);
    }

    void OnDisable()
    {
        WeaponManager.Instance.UnRegister(this);
    }

    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        col=GetComponent<Collider2D>();
        Transform childAttackArea = transform.Find("AttackArea");
        attackCol = childAttackArea.GetComponent<Collider2D>();
        attackCol.isTrigger = true;
        attackCol.enabled = false;
    }
    private void Start()
    {
        info = DataManager.instance.GetWeaponData(weaponID);
    }

    private void Update()
    {
        travelDistance += rb.linearVelocity.magnitude * Time.deltaTime;
        rb.linearVelocity *= 0.995f;

        if (travelDistance >= info.throwDistance)
        {
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
                isThrowing = false;
                attackCol.enabled = false;
                travelDistance = 0f;
            }
                
        }
    }

    public void Equip(Transform parent, Collider2D owner)
    {

        isEquipped = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.simulated = false;
        col.enabled = false;
        attackCol.enabled = false;

        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //transform.localScale = Vector3.one;

        
    }

    public void Drop()
    {
        isEquipped = false;
        transform.SetParent (null);
        rb.simulated = true;
        col.enabled = true;
        //transform.rotation = Quaternion.identity;
    }

    public void Attack()
    {
        if (isAttacking)
            return;
        StartCoroutine(CoAttack());
    }

    IEnumerator CoAttack()
    {
        isAttacking = true;

        Quaternion startRot =
            Quaternion.Euler(0, 0, -info.attackAngle/2);
        Quaternion endRot =
            Quaternion.Euler(0, 0, info.attackAngle/2);

        float t = 0;

        attackCol.enabled = true;

        while (t < info.attackDelay)
        {
            t += Time.deltaTime;

            transform.localRotation =
                Quaternion.Lerp(startRot, endRot, t/info.attackDelay);

            yield return null;
        }

        t = 0;
        attackCol.enabled = false;


        while (t < info.attackDelay)
        {
            t += Time.deltaTime * 10f;

            transform.localRotation =
                Quaternion.Lerp(endRot, startRot, t/info.attackDelay);

            yield return null;
        }

        isAttacking = false;
    }

    public void GiveDamage(Character target)
    {
        if (target == null)
            return;
        if (isThrowing)
            target.TakeDamage(info.throwDamage);
        else
            target.TakeDamage(info.damage);
    }

    public void Throw(Vector2 dir, Collider2D parentCol)
    {

        if(!isEquipped)
            return;

        isEquipped = false;
        transform.SetParent(null);
        rb.simulated = true;
        col.enabled = true;
        transform.rotation = Quaternion.identity;

        travelDistance = 0;
        bounceCount = 0;
        currentSpeed = info.throwSpeed;

        attackCol.enabled = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.linearVelocity = dir.normalized * info.throwSpeed;
        isThrowing = true;

        StartCoroutine(IgnoreOwnerCollision(parentCol));
    }

    WaitForSeconds ignoreTime = new WaitForSeconds(0.2f);

    IEnumerator IgnoreOwnerCollision(Collider2D parentCol)
    {
        Physics2D.IgnoreCollision(col, parentCol, true);

        yield return ignoreTime;

        Physics2D.IgnoreCollision(col, parentCol, false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isThrowing)
            return;
        
        Character target = collision.GetComponent<Character>();
            


        isThrowing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isThrowing)
            return;
        Character ch = collision.collider.GetComponent<Character>();
        Vector2 normal = collision.contacts[0].normal;

        Vector2 reflect = Vector2.Reflect(rb.linearVelocity.normalized, normal);

        bounceCount++;

        if (currentSpeed < 1f)
        {
            currentSpeed = 0f;

            rb.linearVelocity=Vector2.zero;
            isThrowing = false;
            attackCol.enabled = false;

            return;
        }
        else
            currentSpeed *= info.bounceSpeedRate;

        rb.linearVelocity = reflect * currentSpeed;



        if (bounceCount >= info.maxBounce)
        {
            rb.linearVelocity = Vector2.zero;
            isThrowing = false;
            attackCol.enabled = false;
        }
    }

}
