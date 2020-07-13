using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cubra
{
    public class TransitionsManager : MonoBehaviour
    {
        // Перечисление сцен главного меню (id сцен)
        public enum Scenes { Menu = 1, Levels, Leaderboard, Training }

        /// <summary>
        /// Переход на указанную сцену
        /// </summary>
        /// <param name="scene">id сцены</param>
        public void GoToScene(int scene)
        {
            SceneManager.LoadScene(scene);
        }

        /// <summary>
        /// Асинхронная загрузка уровня
        /// </summary>
        /// <param name="scene"></param>
        public void GoToScene(string scene)
        {
            _ = SceneManager.LoadSceneAsync(scene);
        }

        /// <summary>
        /// Переход на указанную сцену с паузой
        /// </summary>
        /// <param name="seconds">секунды до загрузки</param>
        /// <param name="scene">сцена для загрузки</param>
        public IEnumerator GoToSceneWithPause(float seconds, int scene)
        {
            yield return new WaitForSeconds(seconds);
            GoToScene(scene);
        }

        /// <summary>
        /// Перезагрузка текущей сцены
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Выход из игры
        /// </summary>
        public void CloseApplication()
        {
            Application.Quit();
        }
    }
}