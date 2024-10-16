using System.Collections.Generic;
using UnityEngine;

namespace GridSystem.Pooling
{
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

        public void ReturnToPool(T obj)
        {
            (obj as GameObject)?.SetActive(false);
            pool.Enqueue(obj);
        }
    }
}