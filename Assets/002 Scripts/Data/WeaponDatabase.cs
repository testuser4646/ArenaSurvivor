using System;
using UnityEngine;
using System.Collections.Generic;


[Serializable]
public class WeaponInfo
{
    public int id;
    public string weaponName;
    public float damage;
    public float attackRange;
    public float attackAngle;
    public float attackDelay;
    public float throwDamage;
    public float throwSpeed;
    public float throwDistance;
    public int maxBounce;
    public float bounceSpeedRate;
    public WeaponInfo(int id,
                  string weaponName,
                  float damage,
                  float attackRange,
                  float attackAngle,
                  float attackDelay,
                  float throwDamage,
                  float throwSpeed,
                  float throwDistance,
                  int maxBounce,
                  float bounceSpeedRate)
    {
        this.id = id;
        this.weaponName = weaponName;
        this.damage = damage;
        this.attackRange = attackRange;
        this.attackAngle = attackAngle;
        this.attackDelay = attackDelay;
        this.throwDamage = throwDamage;
        this.throwSpeed = throwSpeed;
        this.throwDistance = throwDistance;
        this.maxBounce = maxBounce;
        this.bounceSpeedRate = bounceSpeedRate;
    }

    public WeaponInfo Clone()
    {
        return new WeaponInfo(
            id,
            weaponName,
            damage,
            attackRange,
            attackAngle,
            attackDelay,
            throwDamage,
            throwSpeed,
            throwDistance,
            maxBounce,
            bounceSpeedRate
        );
    }
}

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Data/Weapon")]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponInfo> weaponList = new List<WeaponInfo>();
}