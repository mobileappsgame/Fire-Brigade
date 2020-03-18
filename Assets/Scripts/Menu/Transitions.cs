using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitions : MonoBehaviour
{
    public enum Scene { Menu = 1 }

    /// <summary>
    /// Переход на указанную сцену
    /// </summary>
    /// <param name="scene">Сцена для перехода</param>
    public void GoToScene(Scene scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    public void GoToScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}