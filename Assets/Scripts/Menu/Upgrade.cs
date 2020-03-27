using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [Header("Стоимость повышения")]
    [SerializeField] private int[] upgradeCost;

    [Header("Компонент перевода")]
    [SerializeField] private TextValue level;

    // Ссылка на компонент
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        CheckCurrentScore();
    }

    /// <summary>
    /// Проверка счета для улучшения носилок
    /// </summary>
    public void CheckCurrentScore()
    {
        // Текущий уровень носилок
        var currentLevel = PlayerPrefs.GetInt("stretcher");

        // Достаточно ли текущего счета для улучшения
        var enough = PlayerPrefs.GetInt("current-score") >= upgradeCost[currentLevel];

        // Настраиваем кнопку
        button.interactable = enough ? true : false;
    }

    /// <summary>
    /// Улучшение носилок
    /// </summary>
    public void UpgradeStretcher()
    {
        // Текущий уровень носилок
        var currentLevel = PlayerPrefs.GetInt("stretcher");

        // Уменьшаем текущее количество очков
        PlayerPrefs.SetInt("current-score", PlayerPrefs.GetInt("current-score") - upgradeCost[currentLevel]);
        // Увеличиваем уровень носилок
        PlayerPrefs.SetInt("stretcher", currentLevel + 1);

        // Обновляем перевод
        level.TranslateText();
    }
}