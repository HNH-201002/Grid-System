using UnityEngine;
using System.Collections.Generic;

namespace GridSystem.Pooling
{
    /// <summary>
    /// A generic object pool for reusing MonoBehaviour objects.
    /// Helps reduce overhead by reusing objects instead of creating and destroying them repeatedly.
    /// </summary>
    /// <typeparam name="T">The type of object to pool, must inherit from <see cref="MonoBehaviour"/>.</typeparam>
    public class GenericObjectPool<T> where T : MonoBehaviour
    {
        private Queue<T> objectPool;
        private T prefab;
        private Transform parent;

        public GenericObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            objectPool = new Queue<T>();
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                T obj = GameObject.Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }

        /// <summary>
        /// Retrieves an object from the pool, instantiating a new one if the pool is empty.
        /// </summary>
        /// <returns>An active object from the pool.</returns>
        public T GetFromPool()
        {
            if (objectPool.Count == 0)
            {
                AddObjects(1);
            }

            T obj = objectPool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        /// <summary>
        /// Returns an object back to the pool and deactivates it.
        /// </summary>
        /// <param name="obj">The object to return to the pool.</param>
        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            objectPool.Enqueue(obj);
        }

        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T obj = GameObject.Instantiate(prefab, parent);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }
    }

}