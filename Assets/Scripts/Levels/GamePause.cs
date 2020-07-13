using UnityEngine;

namespace Cubra.Levels
{
    public class GamePause : MonoBehaviour
    {
        /// <summary>
        /// Настройка паузы в игре
        /// </summary>
        /// <param name="state">состояние паузы</param>
        public void SetPause(bool state)
        {
            Time.timeScale = state ? 0 : 1;
        }

        private void OnDestroy()
        {
            SetPause(false);
        }
    }
}