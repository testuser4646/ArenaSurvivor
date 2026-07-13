using UnityEngine;
public abstract class Character : MonoBehaviour
{
    protected float maxHP;
    protected float nowHP;


    public virtual void TakeDamage(float damage)
    {
        nowHP -= damage;

        if (nowHP <= 0)
            Die();
    }

    protected abstract void Die();

    protected abstract void Init();
}