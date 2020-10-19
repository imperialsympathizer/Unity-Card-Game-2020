using System.Collections.Generic;
using UnityEngine;

public static class ObjectPooler {

    // You can avoid resizing the Stack's data by setting this to a number you expect most of your pool sizes to be.
    // You can also use Preload() to set the initial size of a pool if it will uniquely have a high number of objects to spawn
    const int DEFAULT_POOL_SIZE = 20;

    // All of our pools
    static Dictionary<GameObject, Pool> pools;

    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// </summary>
    class Pool {
        // We append an id to the name of anything we instantiate. This is purely cosmetic.
        int nextId = 1;

        // The structure containing our inactive objects.
        // Popping from the back is faster than querying a list or array.
        Stack<GameObject> inactive;

        // The prefab that we are pooling
        GameObject prefab;

        public Pool(GameObject prefab, int initialQty) {
            this.prefab = prefab;
            inactive = new Stack<GameObject>(initialQty);
        }

        // Spawn an object from our pool
        public GameObject Spawn(Vector3 pos, Quaternion rot) {
            GameObject obj;
            if (inactive.Count == 0) {
                // We don't have an object in our pool, so instantiate a whole new object.
                obj = (GameObject)GameObject.Instantiate(prefab, pos, rot);
                obj.name = prefab.name + " (" + (nextId++) + ")";

                // Add a PoolMember component so we know what pool we belong to.
                obj.AddComponent<PoolMember>().myPool = this;
            }
            else {
                // Grab the last object in the inactive array
                obj = inactive.Pop();

                if (obj == null) {
                    // The inactive object we expected to find no longer exists.
                    // No worries -- we'll just try the next one in our sequence.

                    return Spawn(pos, rot);
                }
            }

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
            return obj;
        }

        // Return an object to the inactive pool.
        public void Despawn(GameObject obj) {
            obj.SetActive(false);
            inactive.Push(obj);
        }
    }

    /// <summary>
    /// Added to freshly instantiated objects, to link back to the correct pool on despawn.
    /// </summary>
    class PoolMember : MonoBehaviour {
        public Pool myPool;
    }

    /// <summary>
    /// Initialize our dictionary.
    /// </summary>
    static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE) {
        if (pools == null) {
            pools = new Dictionary<GameObject, Pool>();
        }
        if (prefab != null && pools.ContainsKey(prefab) == false) {
            pools[prefab] = new Pool(prefab, qty);
        }
    }

    /// <summary>
    /// If you want to preload a few copies of an object at the start
    /// of a scene, you can use this. Really not needed unless you're
    /// going to go from zero instances to 100+ very quickly.
    /// Could technically be optimized more, but in practice the
    /// Spawn/Despawn sequence is going to be pretty darn quick and
    /// this avoids code duplication.
    /// </summary>
    static public void Preload(GameObject prefab, int qty = 1) {
        Init(prefab, qty);

        // Make an array to grab the objects we're about to pre-spawn.
        GameObject[] obs = new GameObject[qty];
        for (int i = 0; i < qty; i++) {
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
        }

        // Now despawn them all.
        for (int i = 0; i < qty; i++) {
            Despawn(obs[i]);
        }
    }

    /// <summary>
    /// Spawns a copy of the specified prefab (instantiating one if required).
    /// NOTE: Remember that Awake() or Start() will only run on the very first
    /// spawn and that member variables won't get reset.  OnEnable will run
    /// after spawning -- but remember that toggling IsActive will also
    /// call that function.
    /// </summary>
    static public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot) {
        Init(prefab);

        return pools[prefab].Spawn(pos, rot);
    }

    /// <summary>
    /// Despawn the specified gameobject back into its pool.
    /// </summary>
    static public void Despawn(GameObject obj) {
        PoolMember pm = obj.GetComponent<PoolMember>();
        if (pm == null) {
            // Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
            GameObject.Destroy(obj);
        }
        else {
            // First, parent unused objects to the GameController to prevent sibling indexing issues with other visuals
            VisualController.SharedInstance.RemoveFromVisual(obj.transform);
            // Then despawn the object
            pm.myPool.Despawn(obj);
        }
    }

}
