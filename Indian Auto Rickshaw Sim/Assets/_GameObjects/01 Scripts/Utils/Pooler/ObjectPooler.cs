using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    [System.Serializable]
    public class Pool
    {
        public int tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;

    private Dictionary<int, Queue<GameObject>> poolDictionary;

    public static ObjectPooler Instance;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        poolDictionary = new Dictionary<int, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.transform.localPosition = Vector3.zero;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }
	
    public GameObject SpawnFromPool(int tag, Vector3 position, Quaternion rot)
    {	
        if(!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject objto_spwan = poolDictionary[tag].Dequeue();
        objto_spwan.SetActive(true);
        objto_spwan.transform.position = position;
        objto_spwan.transform.rotation = rot;

        poolDictionary[tag].Enqueue(objto_spwan);

        return objto_spwan;
    }
}