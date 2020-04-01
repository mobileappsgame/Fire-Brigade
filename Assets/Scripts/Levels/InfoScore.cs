using UnityEngine;
using UnityEngine.UI;

public class InfoScore : MonoBehaviour
{
    [Header("Текст изменения")]
    [SerializeField] private Text textChange;

    // Ссылки на компоненты
    private Text textScore;
    private Outline outline;
    private Animator animator;
    private LevelManager levelManager;

    private void Awake()
    {
        textScore = GetComponent<Text>();
        outline = textChange.GetComponent<Outline>();
        animator = textChange.GetComponent<Animator>();
        levelManager = Camera.main.GetComponent<LevelManager>();

        // Подписываем в событие метод обновления счета
        levelManager.ScoresChanged += UpdateLevelScore;
    }

    /// <summary>
    /// Отображение текущего счета
    /// </summary>
    /// <param name="value">значение</param>
    public void UpdateLevelScore(int value)
    {
        // Выводим счет уровня
        textScore.text = levelManager.Score.ToString();

        animator.enabled = true;
        // Записываем количество очков в эффект изменения
        textChange.text = (value > 0 ? "+ " : "") + value.ToString();
        // Устанавливаем обводку эффекта в зависимости от значения
        outline.effectColor = value > 0 ? Color.green : Color.red;
        // Перезапускаем анимацию
        animator.Rebind();
    }
}