using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour
{
    [Header("Элементы меню")]
    [SerializeField] private GameObject[] menuItems;

    // Перечисление элементов меню
    private enum MenuItems { Blackout, Characters, Menu, Results, Victory, Lose }

    [Header("Компонент счета")]
    [SerializeField] private Score score;

    [Header("Текст счета")]
    [SerializeField] private Text scoreText;

    /// <summary>
    /// Вывод результата уровня
    /// </summary>
    public void ShowResult()
    {
        // Отображаем элементы меню
        menuItems[(int)MenuItems.Blackout].SetActive(true);
        menuItems[(int)MenuItems.Characters].SetActive(true);
        menuItems[(int)MenuItems.Menu].SetActive(true);
        menuItems[(int)MenuItems.Results].SetActive(true);

        if (LevelManager.Mode == "victory")
        {
            // Домнажаем набранные очки
            Score.ChangingScore(score.ScoreLevel * 5);

            menuItems[(int)MenuItems.Victory].SetActive(true);

            // Увеличиваем прогресс викторины, общий и текущий счет
            PlayerPrefs.SetInt("progress", PlayerPrefs.GetInt("progress") + 1);
            PlayerPrefs.SetInt("total-score", PlayerPrefs.GetInt("total-score") + score.ScoreLevel);
            PlayerPrefs.SetInt("current-score", PlayerPrefs.GetInt("current-score") + score.ScoreLevel);

            // Отображаем счетчик набранных очков
            StartCoroutine(ScoreCounter());
        }
        else
        {
            menuItems[(int)MenuItems.Lose].SetActive(true);
        }
    }

    /// <summary>
    /// Счетчик набранных очков от нуля
    /// </summary>
    private IEnumerator ScoreCounter()
    {
        // Начальное значение очков
        var scoreLevel = 0;

        while (scoreLevel < score.ScoreLevel)
        {
            yield return new WaitForSeconds(0.005f);

            scoreLevel++;
            scoreText.text = scoreLevel.ToString();
        }
    }
}