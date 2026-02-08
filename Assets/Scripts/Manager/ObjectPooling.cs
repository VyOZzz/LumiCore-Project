using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ObjectPooling : MonoBehaviour
{
    #region Singleton
    public static ObjectPooling Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        InitializePools();
    }
    #endregion
    #region Pool Definitions

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int initialSize;
    }
    [Header("Pool Configurations")]
    [SerializeField] private List<Pool> pools = new List<Pool>();
    
    private Dictionary<string, Queue<GameObject>> poolDictionary ;
    #endregion
    
    #region Initialization

    /// <summary>
    /// Creates initial pools based on configuration
    /// </summary>
    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            // Pre-instantiate objects
            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = CreateNewObject(pool.prefab);
                objectQueue.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag, objectQueue);
            Debug.Log($"Pool initialized: {pool.tag} with {pool.initialSize} objects");
        }
    }

    /// <summary>
    /// Creates a new object and sets it up for pooling
    /// </summary>
    private GameObject CreateNewObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        obj.transform.SetParent(this.transform);
        return obj;
    }
    #endregion
    
    #region Spawn & Return
    /// <summary>
    /// Spawns an object from pool
    /// </summary>
    /// <param name="tag">Pool tag</param>
    /// <param name="position">Spawn position</param>
    /// <param name="rotation">Spawn rotation</param>
    /// <returns>The spawned GameObject</returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        //Check if pool exists
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist!");
            return null;
        }

        GameObject objectToSpawn;
        //If pool is empty, create a new object
        if (poolDictionary[tag].Count == 0)
        {
            Pool pool = pools.Find(p => p.tag == tag);
            objectToSpawn = CreateNewObject(pool.prefab);
            Debug.Log($"Pool '{tag}' was empty, created new object");

        }
        else
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
        }
        //setup object
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        
        //call OnObjectSpawned if it implements IPooledObject
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        pooledObj?.OnObjectSpawn();
        return objectToSpawn;
    }

    /// <summary>
    /// Returns an object to its pool
    /// </summary>
    /// <param name="obj">Object to return</param>
    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;
        // Find which pool this object belongs to
        string poolTag = FindPoolTag(obj);
        if (poolTag == null)
        {
            Debug.LogWarning($"Object {obj.name} doesn't belong to any pool!");
            Destroy(obj);
            return;
        }
        // Deactivate and return to pool
        obj.SetActive(false);
        obj.transform.SetParent(this.transform);
        poolDictionary[poolTag].Enqueue(obj);
    }

    /// <summary>
    /// Finds which pool an object belongs to
    /// </summary>
    private string FindPoolTag(GameObject obj)
    {
        foreach (Pool pool in pools)
        {
            if (obj.name.Contains(pool.prefab.name))
            {
                return pool.tag;
            }
        }

        return null;
    }
    #endregion

    #region  Helper Methods

    /// <summary>
    /// Gets current pool size fro debugging
    /// </summary>
    public int GetPoolSize(string tag)
    {
        if (!poolDictionary.ContainsKey(tag)) return -1;
        return poolDictionary[tag].Count;
    }

    /// <summary>
    /// Clears all pools
    /// </summary>
    public void ClearAllPools()
    {
        foreach (var pool in poolDictionary.Values)
        {
            while (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                Destroy(obj);
            }
        }
        poolDictionary.Clear();
        Debug.Log("All pools cleared");
    }
    #endregion
}
