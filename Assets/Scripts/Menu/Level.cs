using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [Header("Номер уровня")]
    [SerializeField] private int number;

    public int Number => number;

    [Header("Номер уровня")]
    [SerializeField] private GameObject textNumber;

    [Header("Статус уровня")]
    [SerializeField] private Image imageLevel;

    [Header("Спрайты статусов")]
    [SerializeField] private Sprite[] statuses;

    // Ссылка на компонент
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        // Если игровой прогресс достаточный
        if (PlayerPrefs.GetInt("progress") == number)
        {
            // Скрываем замок и показываем номер
            imageLevel.gameObject.SetActive(false);
            textNumber.SetActive(true);

            // Активируем кнопку
            button.interactable = true;
        }
        else if (PlayerPrefs.GetInt("progress") > number)
        {
            // Меняем замок на галочку
            imageLevel.sprite = statuses[1];

            // Активируем кнопку
            button.interactable = true;
        }
    }
}