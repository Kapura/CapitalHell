using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    public abstract class SpawnPool : MonoBehaviour
    {
        public abstract void Reclaim(IPoolable item);
    }

    public class SpawnPool<T> : SpawnPool where T: MonoBehaviour, IPoolable
    {
        public GameObject pooledObject;

        public int poolSize;

        protected Queue<T> _inactiveItems;

        private void Awake()
        {
            _inactiveItems = new Queue<T>(poolSize);

            T itemScript = pooledObject.GetComponent<T>();
            _inactiveItems.Enqueue(itemScript);
            itemScript.enabled = false;
            itemScript.SetPool(this);

            int itemsCreated = 1;

            while (itemsCreated < poolSize)
            {
                GameObject newObj = Instantiate(pooledObject);
                itemScript = newObj.GetComponent<T>();
                _inactiveItems.Enqueue(itemScript);
                itemScript.enabled = false;
                itemScript.SetPool(this);
                itemsCreated++;
            }
        }

        public virtual T SpawnNewItem(Vector3 position)
        {
            T itemScript = _inactiveItems.Dequeue();
            itemScript.transform.position = position;
            itemScript.enabled = true;
            return itemScript;
        }

        public override void Reclaim(IPoolable item)
        {
            Reclaim(item as T);
        }

        public void Reclaim(T item)
        {
            item.enabled = false;
            _inactiveItems.Enqueue(item);
        }
    }

    public interface IPoolable
    {
        bool enabled { get; set; }
        void SetPool(SpawnPool pool);
    }
}