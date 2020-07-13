using UnityEngine;

namespace Cubra.Levels
{
    public class ResetPool : MonoBehaviour
    {
        /// <summary>
        /// Очистка пула объектов
        /// </summary>
        public void ClearPool()
        {
            PoolsManager.poolsDictionary.Clear();
        }
    }
}