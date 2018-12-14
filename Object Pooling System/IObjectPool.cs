using System;

namespace Pool
{
    /// <summary>
    /// Interface for a pool of objects used by ObjectPoolManager to manage a
    /// collection of Object Pools.
    /// </summary>
    public interface IObjectPool : IDisposable
    {
        /// <summary>
        /// The number of objects that are currently allocated in the pool.
        /// </summary>
        int NumAllocatedObjects { get; }
        /// <summary>
        /// Returns true if the pool currently has no objects in it.
        /// </summary>
        bool IsPoolEmpty { get; }
        /// <summary>
        /// Returns true if the NumAllocatedObjects is equal to the capacity of the pool.
        /// </summary>
        bool IsPoolFull { get; }
        /// <summary>
        /// Clears all of the allocated objects from the pool.
        /// </summary>
        void Clear();
        /// <summary>
        /// Allocates amount objects in the pool.
        /// </summary>
        /// <param name="amount"></param>
        void Allocate(int amount);
    }
}
