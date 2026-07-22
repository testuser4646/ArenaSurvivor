using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    private List<WeaponInfo> weaponList = new List<WeaponInfo>();
    private Dictionary<int, WeaponInfo> weapons = new Dictionary<int, WeaponInfo>();

    [SerializeField] private WeaponDatabase weaponDatabase;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadWeaponData();
    }

    public void LoadWeaponData()
    {
        weaponList.Clear();

        for (int i = 0; i < weaponDatabase.weaponList.Count; i++)
        {
            WeaponInfo weapon = weaponDatabase.weaponList[i].Clone();
            weapons[weapon.id] = weapon;
            weaponList.Add(weapon);
        }
    }

    public WeaponInfo GetWeaponData(int id)
    {
        return weapons.GetValueOrDefault(id);

        //return weaponList.Where(weapon => weapon.id == id).FirstOrDefault();
    }
}
