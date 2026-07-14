using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;


public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [SerializeField] List<Rect> spawnArea;
    [SerializeField] Color color = new Color(1, 0, 0, 0.5f);

    float arenaRadius = 28f;
    Vector2 arenaCenter = Vector2.zero;

    private List<string> enemyPoolNames
        = new() { "Enemy_Blue", "Enemy_Purple", "Enemy_Red", "Enemy_Yellow" };
    private List<string> weaponPoolNames
        = new() { "Axe", "Knife", "Hammer", "Pickaxe" };
    private List<string> itemPoolNames
        = new() { "Meat", "Wood" };


    // 살아 있는 모든 적
    List<GameObject> EnemyList = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SpawnObject(enemyPoolNames, 15);
        SpawnObject(weaponPoolNames, 10);
        SpawnObject(itemPoolNames, 10);
    }


    private Vector2 GetRandomPosition()
    {
        return arenaCenter + Random.insideUnitCircle * arenaRadius;
    }

    private void SpawnObject(List<string> poolNames, int count)
    {
        GameObject obj;
        for (int i=0;i<count; i++)
        {
            string poolName = poolNames[Random.Range(0, poolNames.Count)];

            obj = ObjectPoolManager.instance.GetObject(poolName);
            
            obj.transform.position = GetRandomPosition();

            if (poolName.StartsWith("Enemy"))
                EnemyList.Add(obj);
        }
    }








    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(0,0,0), arenaRadius);
    }
}
