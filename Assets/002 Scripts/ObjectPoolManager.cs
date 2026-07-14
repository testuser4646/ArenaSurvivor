using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [SerializeField] List<GameObject> objList = new List<GameObject>();

    Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    int poolSize;

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
        poolSize = 10;
        foreach (GameObject obj in objList)
        {
            pools[obj.name] = new Queue<GameObject>();

            GameObject parentPool = new GameObject($"{obj.name}.Pool");
            parentPool.transform.SetParent(this.transform);

            for (int i = 0; i < poolSize; i++)
            {
                GameObject go = Instantiate(obj, parentPool.transform);
                go.SetActive(false);
                pools[obj.name].Enqueue(go);
            }
        }
    }

    public GameObject GetObject(string name)
    {
        if (!pools.ContainsKey(name))
        {
            return null;
        }
        if (pools[name].Count > 0)
        {
            GameObject go = pools[name].Dequeue();
            go.SetActive(true);

            return go;
        }
        else
        {
            GameObject go = Instantiate(objList.Find(obj => obj.name == name));
            return go;
        }
    }

    public void ReturnObject(string name, GameObject go)
    {
        if (!pools.ContainsKey(name))
        {
            Destroy(go);
            return;
        }
        go.SetActive(false);
        pools[name].Enqueue(go);
    }

    public void ResetPool()
    {
        foreach (Transform pool in transform)
        {
            foreach (Transform obj in pool)
            {
                GameObject go = obj.gameObject;

                if (go.activeSelf)
                {
                    go.SetActive(false);
                }
            }
        }

        pools.Clear();

        foreach (Transform pool in transform)
        {
            string objName = pool.name.Replace(".Pool", "");

            Queue<GameObject> queue = new Queue<GameObject>();

            foreach (Transform obj in pool)
            {
                queue.Enqueue(obj.gameObject);
            }

            pools.Add(objName, queue);
        }

    }
}
