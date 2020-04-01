using UnityEngine;

public class SuperStretcher : MonoBehaviour
{
    [Header("Компонент носилок")]
    [SerializeField] private Stretcher stretcher;

    private void Start()
    {
        CheckQuantity();
    }

    /// <summary>
    /// Проверка количества улучшенных носилок
    /// </summary>
    public void CheckQuantity()
    {
        gameObject.SetActive(PlayerPrefs.GetInt("super-stretcher") > 0 ? true : false);
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