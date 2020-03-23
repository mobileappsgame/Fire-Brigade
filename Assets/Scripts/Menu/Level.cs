using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [Header("Номер уровня")]
    [SerializeField] private int number;

    public string Number { get { return number.ToString(); } }

    [Header("Текстовый объект")]
    [SerializeField] private GameObject textNumber;

    [Header("Изображение замка")]
    [SerializeField] private GameObject imageLock;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        // Если игровой прогресс достаточный
        if (PlayerPrefs.GetInt("progress") >= number)
        {
            // Скрываем замок
            imageLock.SetActive(false);

            // Показываем номер уровня
            textNumber.SetActive(true);

            // Активируем кнопку
            button.interactable = true;
        }
    }
}