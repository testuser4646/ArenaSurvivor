using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [SerializeField] List<GameObject> objList = new List<GameObject>();

    Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

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

    }

    public void CreatePool(string name, int count)
    {
        if (pools.ContainsKey(name))
            return;

        GameObject prefab =
            objList.Find(x => x.name == name);

        if (prefab == null)
            return;

        pools[name] = new Queue<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(prefab);

            go.name = prefab.name;

            go.SetActive(false);

            pools[name].Enqueue(go);
        }
    }

    public GameObject GetObject(string name)
    {
        if (!pools.ContainsKey(name))
            return null;

        if (pools[name].Count > 0)
        {
            GameObject go = pools[name].Dequeue();

            go.SetActive(true);

            return go;
        }

        GameObject prefab = objList.Find(x => x.name == name);

        GameObject obj = Instantiate(prefab);

        obj.name = prefab.name;

        return obj;
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
