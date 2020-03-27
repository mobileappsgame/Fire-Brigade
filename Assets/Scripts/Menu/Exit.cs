using UnityEngine;

public class Exit : MonoBehaviour
{
    [Header("Панель выхода")]
    [SerializeField] private GameObject exit;

    private void Update()
    {
        // Если нажата кнопка возврата и панель выхода неактивна
        if (Input.GetKey(KeyCode.Escape) && exit.activeSelf == false)
        {
            exit.SetActive(true);
        }
    }

    /// <summary>
    /// Выход из игры
    /// </summary>
    public void CloseApplication()
    {
        Application.Quit();
    }
}