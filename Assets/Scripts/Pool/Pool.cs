using UnityEngine;

public class Pool : MonoBehaviour
{
    private void Awake()
    {
        // Заполняем пул созданными объектами
        PoolsManager.FillPool(gameObject);
    }
}