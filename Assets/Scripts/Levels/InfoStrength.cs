using UnityEngine;
using UnityEngine.UI;

public class InfoStrength : MonoBehaviour
{
    [Header("Компонент носилок")]
    [SerializeField] private Stretcher stretcher;

    // Текущее значение прочности
    private int presentValue;

    // Ссылки на компоненты
    private Text percent;
    private Animator animator;

    private void Awake()
    {
        percent = GetComponent<Text>();
        animator = GetComponent<Animator>();

        // Получаем прочность носилок
        presentValue = stretcher.Strength;

        // Подписываем в событие метод обновления прочности
        stretcher.StrengthChanged += ShowPercent;
    }

    private void Start()
    {
        ShowPercent();
    }

    /// <summary>
    /// Отображение прочности носилок
    /// </summary>
    private void ShowPercent()
    {
        // Выводим прочность носилок
        percent.text = (stretcher.Strength > 0 ? stretcher.Strength.ToString() : "0") + "%";

        // Если значение уменьшилось, отображаем анимацию вычитания
        if (stretcher.Strength < presentValue) SubtractionAnimation();
    }

    /// <summary>
    /// Анимация уменьшения прочности
    /// </summary>
    private void SubtractionAnimation()
    {
        // Обновляем текущий процент
        presentValue = stretcher.Strength;

        // Анимация вычитания
        animator.enabled = true;
        animator.Rebind();
    }    
}