using UnityEngine;

public class SuperiorStretcher : MonoBehaviour
{
    [Header("Компонент носилок")]
    [SerializeField] private Stretcher stretcher;

    private void Start()
    {
        CheckQuantity();
    }

    /// <summary>
    /// Проверка количества 
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
        StartCoroutine(stretcher.SuperiorStretcher());

        // Уменьшаем количество улучшенных носилок
        PlayerPrefs.SetInt("super-stretcher", PlayerPrefs.GetInt("super-stretcher") - 1);

        CheckQuantity();
    }
}