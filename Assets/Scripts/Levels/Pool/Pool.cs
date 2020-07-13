using UnityEngine;

namespace Cubra.Levels
{
    public class Pool : MonoBehaviour
    {
        private void Awake()
        {
            // Заполняем пул объектами
            PoolsManager.FillPool(gameObject);
        }
    }
}