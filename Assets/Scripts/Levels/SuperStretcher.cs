using UnityEngine;
using UnityEngine.UI;

public class SuperStretcher : MonoBehaviour
{
    [Header("Компонент носилок")]
    [SerializeField] private Stretcher stretcher;

    [Header("Дочерние объекты")]
    [SerializeField] private Image[] objects;

    // Ссылка на компонент
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        CheckQuantity();
    }

    /// <summary>
    /// Проверка количества улучшенных носилок
    /// </summary>
    public void CheckQuantity()
    {
        button.interactable = PlayerPrefs.GetInt("super-stretcher") > 0 ? true : false;

        // Если кнопка отключена
        if (button.interactable == false)
        {
            // Увеличиваем прозрачность дочерних объектов
            foreach (var item in objects)
                item.color = new Color(1, 1, 1, 0.4f);
        }
    }

    /// <summary>
    /// Использование улучшенных носилок
    /// </summary>
    public void UseImprovement()
    {
        // Запускаем отсчет до возвращения стандартных носилок
        _ = StartCoroutine(stretcher.SuperiorStretcher());

        // Уменьшаем количество улучшенных носилок
        PlayerPrefs.SetInt("super-stretcher", PlayerPrefs.GetInt("super-stretcher") - 1);

        CheckQuantity();
    }
}