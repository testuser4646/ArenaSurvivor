using UnityEngine;
public abstract class Character : MonoBehaviour
{
    protected float maxHP;
    protected float nowHP;

    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [SerializeField] protected Transform holdPoint;
    [SerializeField] protected Transform handPivot;

    [SerializeField] protected Weapon currentWeapon;
    protected Weapon nearbyWeapon;

    protected Vector2 dir;
    protected Vector2 lookDir;

    protected float moveSpeed = 10f;
    protected float handRotateSpeed = 400f;

    protected bool isDead;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate()
    {
        Vector2 targetVelocity = dir * moveSpeed;

        rb.linearVelocity = Vector2.MoveTowards(
            rb.linearVelocity,
            targetVelocity,
            100f * Time.fixedDeltaTime);
    }

    public virtual void TakeDamage(float damage)
    {
        nowHP -= damage;

        if (nowHP <= 0)
            Die();
    }

    protected abstract void Die();

    protected abstract void Init();

    protected void RotateHand()
    {
        if (lookDir == Vector2.zero)
            return;

        float angle =
            Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion targetRot =
            Quaternion.Euler(0, 0, angle);

        handPivot.rotation =
            Quaternion.RotateTowards(
                handPivot.rotation,
                targetRot,
                handRotateSpeed * Time.deltaTime);

        if (lookDir.x < 0)
        {
            sr.flipX = true;
            handPivot.localScale = new Vector2(1, -1);
        }
        else
        {
            sr.flipX = false;
            handPivot.localScale = Vector2.one;
        }
    }

    protected void PickupWeapon()
    {
        if (nearbyWeapon == null)
            return;

        if (currentWeapon != null)
            currentWeapon.Drop();

        currentWeapon = nearbyWeapon;
        currentWeapon.Equip(holdPoint, col);

        nearbyWeapon = null;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Weapon weapon = other.GetComponent<Weapon>();

        if (weapon != null)
            nearbyWeapon = weapon;
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        Weapon weapon = other.GetComponent<Weapon>();

        if (weapon == nearbyWeapon)
            nearbyWeapon = null;
    }
}