using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
	/// <summary>
	/// Used by GameObjectPool to manage the pooled GameObjects within the scene graph.
	/// </summary>
	public class GameObjectPoolController : MonoBehaviour
	{
		private Stack<GameObject> toReparentStack;

		public void Initialize(int capacity)
		{
			// To avoid setting pooled obj's activity, this can also be done
			// by changing the GameObject's layer to a hidden layer that camera won't render
			// and set transform position out of camera frustum or camera view.
			transform.localScale = Vector3.zero;
			toReparentStack = new Stack<GameObject>(capacity);
		}

		public void OnBorrowed(GameObject obj)
		{
			// The borrowed object will always be the most recently pooled object.
			if (toReparentStack.Count > 0)
			{
				toReparentStack.Pop();
			}
		}

		public void OnPooled(GameObject obj)
		{
			toReparentStack.Push(obj);
			obj.transform.SetParent(transform, false);
		}
	}
}
