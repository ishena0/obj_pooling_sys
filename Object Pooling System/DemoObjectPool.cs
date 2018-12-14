using Pool;
using UnityEngine;
using UnityEngine.Assertions;

public class DemoObjectPool : MonoBehaviour
{
    [Tooltip("The prefab.")]
    public GameObject prefab;
    [Tooltip("Pool capacity.")]
    public int poolCapacity = 2;
    private string prefabName;
    private GameObjectPool Pool
    {
        get
        {
            ObjectPoolManager poolManager = ObjectPoolManager.Instance;
            Assert.IsNotNull(poolManager);
            GameObjectPool pool = poolManager.GetPool<GameObjectPool>(prefabName);
            if (pool == null)
            {
                pool = new GameObjectPool(prefab, poolCapacity);
                poolManager.AddPool(prefabName, pool);
            }
            return pool;
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(prefab);
        prefabName = prefab.name;
    }

    public GameObject Provide()
    {
        return Pool.Borrow();
    }

    public void Remove(GameObject obj)
    {
        Pool.Return(obj);
    }
}
