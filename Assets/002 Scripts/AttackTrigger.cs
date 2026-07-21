using UnityEngine;

public class AttackTrigger: MonoBehaviour
{
    Weapon weapon;

    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character ch = other.GetComponent<Character>();

        if (ch == null)
            return;

        weapon.GiveDamage(ch);
    }
}
