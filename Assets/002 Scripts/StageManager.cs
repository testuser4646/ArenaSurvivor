using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Stage")]
    [SerializeField] int day = 1;

    [Header("Enemy")]
    [SerializeField] string enemyName = "Enemy_Blue";

    [Header("Weapon")]
    [SerializeField]
    List<string> weaponNames = new() { "Axe", "Knife", "Hammer", "Pickaxe" };

    [Header("Item")]
    [SerializeField]
    List<string> itemNames = new() { "Meat", "Wood" };

    float arenaRadius = 28f;
    Vector2 arenaCenter = Vector2.zero;

    List<GameObject> enemyList = new();

    int enemyCount;
    int weaponCount;
    int itemCount;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        enemyCount = Mathf.Min(30 + (day - 1) * 10, 150);

        weaponCount = enemyCount * 2;

        itemCount = enemyCount / 2;

        ObjectPoolManager.instance.CreatePool(enemyName, enemyCount);

        foreach (string weapon in weaponNames)
            ObjectPoolManager.instance.CreatePool(weapon, weaponCount / weaponNames.Count);

        foreach (string item in itemNames)
            ObjectPoolManager.instance.CreatePool(item, itemCount / itemNames.Count);

        SpawnEnemy(enemyName, enemyCount);

        SpawnRandom(weaponNames, weaponCount);

        SpawnRandom(itemNames, itemCount);
    }

    Vector2 GetRandomPosition()
    {
        return arenaCenter + Random.insideUnitCircle * arenaRadius;
    }

    void SpawnEnemy(string enemyName, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj =
                ObjectPoolManager.instance.GetObject(enemyName);

            obj.transform.position = GetRandomPosition();

            enemyList.Add(obj);
        }
    }

    void SpawnRandom(List<string> list, int count)
    {
        for (int i = 0; i < count; i++)
        {
            string name =
                list[Random.Range(0, list.Count)];

            GameObject obj =
                ObjectPoolManager.instance.GetObject(name);

            obj.transform.position = GetRandomPosition();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, arenaRadius);
    }
}