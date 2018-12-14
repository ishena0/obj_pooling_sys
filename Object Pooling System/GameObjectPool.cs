using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pool
{
	/// <summary>
	/// Specialized version of Pool specifically built to work with GameObjects.
	/// 
	/// GameObjects in the pool are stored underneath a container object in the scene.
	/// This container is scaled to (0,0,0) to hide the pooled objects. This is done instead
	/// of disabling the pooled objects to prevent an issue where disabling & enabling objects
	/// will generate a large amount of memory allocations.
	///
	/// Additionally, when an object is returned to the pool it isn't reparented to the container
	/// object until the end of the frame. This way, if the same object is borrowed again before
	/// the end of the frame (a common occurence), it isn't reparented an extra time.
	/// Reparenting an object can cause a significant amount of memory allocations and CPU load.
	/// </summary>
	public class GameObjectPool : ObjectPool<GameObject>
	{
		private GameObject prefab;
		private GameObjectPoolController poolController;

		public GameObjectPool(GameObject prefab, int capacity):this(prefab, capacity, 0){}

		public GameObjectPool(GameObject prefab, int capacity, int preAllocateAmount)
		{
			Assert.IsNotNull(prefab);
			this.prefab = prefab;
			GameObject poolControllerObj = new GameObject(prefab.name + "Pool");
			poolController = poolControllerObj.AddComponent<GameObjectPoolController>();
			poolController.Initialize(capacity);
			
			Initialize(capacity, preAllocateAmount);
		}

		public override void Dispose()
		{
			if (poolController != null)
			{
				GameObject.Destroy(poolController.gameObject);
			}
		}

		protected override void OnBorrowed(GameObject obj)
		{
			poolController.OnBorrowed(obj);
		}

		protected override void OnPooled(GameObject obj)
		{
			poolController.OnPooled(obj);
		}

		protected override void OnUnableToReturn(GameObject obj)
		{
			GameObject.Destroy(obj);
		}

		protected override GameObject AllocateObject()
		{
			GameObject obj = GameObject.Instantiate(prefab);
			return obj;
		}
	}
}
