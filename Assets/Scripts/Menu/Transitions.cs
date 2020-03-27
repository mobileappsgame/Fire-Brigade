using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitions : MonoBehaviour
{
    /// <summary>
    /// Переход на указанную сцену
    /// </summary>
    /// <param name="scene">номер сцены</param>
    public void GoToScene(int scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    /// <param name="scene">название сцены</param>
    public void GoToScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    /// <summary>
    /// Перезагрузка текущей сцены
    /// </summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}