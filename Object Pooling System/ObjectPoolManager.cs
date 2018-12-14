using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
	/// <summary>
	/// Manages a collection of object pools and provides access to them by name.
	/// </summary>
	public class ObjectPoolManager : MonoBehaviour
	{
		private static ObjectPoolManager instance;

		public static ObjectPoolManager Instance
		{
			get { return instance; }
		}
		
		private Dictionary<string, IObjectPool> pools = new Dictionary<string, IObjectPool>();

		public bool ContainsPool(string poolName)
		{
			return pools.ContainsKey(poolName);
		}

		public T GetPool<T>(string poolName) where T : class, IObjectPool
		{
			IObjectPool pool;
			if (pools.TryGetValue(poolName, out pool))
			{
				T result = pool as T;
				if (result == null)
				{
					Debug.LogError("Pool " + poolName + " is not of type " + typeof(T));
				}
				return result;
			}
			return null;
		}

		public void AddPool(string poolName, IObjectPool pool)
		{
			if (ContainsPool(poolName))
			{
				Debug.LogError("Cannot add pool " + poolName + " because it already exists.");
				return;
			}
			pools.Add(poolName, pool);
		}

		public void RemovePool(string poolName)
		{
			IObjectPool pool;
			if (!pools.TryGetValue(poolName, out pool))
			{
				return;
			}
			pool.Dispose();
			pools.Remove(poolName);
		}

		public void RemoveAllPools()
		{
			var enumerator = pools.GetEnumerator();
			while (enumerator.MoveNext())
			{
				IObjectPool pool = enumerator.Current.Value;
				pool.Dispose();
			}
			pools.Clear();
		}

		private void Awake()
		{
			if (instance != null)
			{
				Debug.LogError("Cannot have multiple instances of ObjectPoolManager");
				Destroy(this);
				return;
			}
			instance = this;
		}

		private void OnDestroy()
		{
			RemoveAllPools();
		}
	}
}
