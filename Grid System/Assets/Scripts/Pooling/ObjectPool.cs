using System.Collections.Generic;
using UnityEngine;

namespace GridSystem.Pooling
{
    /// <summary>
    /// A generic object pool for reusing Unity objects efficiently.
    /// Reduces overhead by reusing objects instead of instantiating and destroying them repeatedly.
    /// </summary>
    /// <typeparam name="T">The type of object to pool, must inherit from <see cref="UnityEngine.Object"/>.</typeparam>
    public class ObjectPool<T> where T : UnityEngine.Object
    {
        private Queue<T> pool;
        private T prefab;
        private int initialSize;

        public ObjectPool(T prefab, int initialSize)
        {
            this.prefab = prefab;
            this.initialSize = initialSize;
            pool = new Queue<T>(initialSize);

            for (int i = 0; i < initialSize; i++)
            {
                T obj = Object.Instantiate(prefab);
                (obj as GameObject)?.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        /// <summary>
        /// Retrieves an object from the pool.
        /// If the pool is empty, a new object is instantiated.
        /// </summary>
        /// <returns>An active object from the pool.</returns>
        public T GetFromPool()
        {
            if (pool.Count > 0)
            {
                T obj = pool.Dequeue();
                (obj as GameObject)?.SetActive(true);
                return obj;
            }
            else
            {
                T newObj = Object.Instantiate(prefab);
                return newObj;
            }
        }

        /// <summary>
        /// Returns an object to the pool and deactivates it.
        /// </summary>
        /// <param name="obj">The object to return to the pool.</param>
        public void ReturnToPool(T obj)
        {
            (obj as GameObject)?.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}