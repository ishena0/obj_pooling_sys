using System.Collections;
using System.Collections.Generic;

namespace Pool
{
    /// <summary>
    /// Can be used to pool any class with a default constructor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> : IObjectPool where T : class, new()
    {
        protected Stack<T> pool;
        protected int capacity;

        public virtual void Dispose()
        {
        }

        public int NumAllocatedObjects
        {
            get { return pool.Count; }
        }

        public bool IsPoolEmpty
        {
            get { return pool.Count == 0; }
        }

        public bool IsPoolFull
        {
            get { return pool.Count == capacity; }
        }

        public void Clear()
        {
            pool.Clear();
        }

        public void Allocate(int amount)
        {
            int counter = 0;
            while (counter < amount && !IsPoolFull)
            {
                AddObject();
            }
        }

        public T Borrow()
        {
            if (IsPoolEmpty)
            {
                return AllocateObject();
            }
            T obj = pool.Pop();
            OnBorrowed(obj);
            return obj;
        }

        public void Return(T obj)
        {
            if (IsPoolFull)
            {
                OnUnableToReturn(obj);
                return;
            }
            pool.Push(obj);
            OnPooled(obj);
        }

        protected ObjectPool()
        {
        }

        public ObjectPool(int capacity, int preAllocateAmount)
        {
            Initialize(capacity, preAllocateAmount);
        }

        protected void Initialize(int capacity, int preAllocateAmount)
        {
            // Capacity must be at least 1.
            if (capacity < 1)
            {
                capacity = 1;
            }
            pool = new Stack<T>(capacity);
            this.capacity = capacity;
            // preAllocateAmount can not be higher than capacity.
            if (preAllocateAmount > capacity)
            {
                preAllocateAmount = capacity;
            }
            Allocate(preAllocateAmount);
        }

        protected virtual T AllocateObject()
        {
            return new T();
        }

        protected virtual void OnPooled(T obj)
        {
        }

        protected virtual void OnUnableToReturn(T obj)
        {
        }

        protected virtual void OnBorrowed(T obj)
        {
        }

        private void AddObject()
        {
            if (IsPoolFull) return;
            T obj = AllocateObject();
            pool.Push(obj);
            OnPooled(obj);
        }
    }
}
