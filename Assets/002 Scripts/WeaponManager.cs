using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    Dictionary<int, Weapon> weapons = new();

    public void Register(Weapon wp)
    {
        weapons[wp.GetInstanceID()] = wp;
    }

    public void UnRegister(Weapon wp)
    {
        weapons.Remove(wp.GetInstanceID());
    }

    public Dictionary<int, Weapon> Weapons => weapons;
}
