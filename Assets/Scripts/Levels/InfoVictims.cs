using UnityEngine;
using UnityEngine.UI;

public class InfoVictims : MonoBehaviour
{
    // Ссылки на компоненты
    private Text quantity;
    private LevelManager levelManager;

    private void Awake()
    {
        quantity = GetComponent<Text>();
        levelManager = Camera.main.GetComponent<LevelManager>();

        // Подписываем в событие метод количества персонажей
        levelManager.VictimsChanged += ShowQuantity;
    }

    private void Start()
    {
        ShowQuantity();
    }

    /// <summary>
    /// Отображение количества оставшихся персонажей
    /// </summary>
    private void ShowQuantity()
    {
        quantity.text = levelManager.CurrentVictims.ToString();
    }
}