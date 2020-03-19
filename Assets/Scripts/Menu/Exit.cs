using UnityEngine;

public class Exit : MonoBehaviour
{
    [Header("Панель выхода")]
    [SerializeField] private GameObject panelExit;

    private void Update()
    {
        // Если нажата кнопка возврата и панель выхода неактивна
        if (Input.GetKey(KeyCode.Escape) && panelExit.activeSelf == false)
        {
            // Активируем панель
            panelExit.SetActive(true);
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