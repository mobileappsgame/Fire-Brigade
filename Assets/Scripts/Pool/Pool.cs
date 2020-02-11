using UnityEngine;

public class Pool : MonoBehaviour
{
    private void Awake()
    {
        // Заполняем пул подготовленными объектами
        PoolsManager.FillPool(gameObject);
    }
}