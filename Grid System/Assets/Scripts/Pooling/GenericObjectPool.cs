using UnityEngine;
using System.Collections.Generic;

namespace GridSystem.Pooling
{
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