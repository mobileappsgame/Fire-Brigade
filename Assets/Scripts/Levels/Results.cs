using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour
{
    [Header("Элементы меню")]
    [SerializeField] private GameObject[] menuItems;

    // Перечисление элементов меню
    private enum MenuItems { Blackout, Characters, Menu, Results, Victory, Lose }

    [Header("Итоговый счет")]
    [SerializeField] private Text scoreText;

    [Header("Фоновая музыка")]
    [SerializeField] private Music backgroundMusic;

    [Header("Звуки результата")]
    [SerializeField] private AudioClip[] audioClips;

    // Перечисление звуков результата
    private enum AudioClips { Victory, Lose }

    // Ссылки на компоненты
    private LevelManager levelManager;
    private AudioSource audioSource;
    private PlayingSound playingSound;

    private void Awake()
    {
        levelManager = Camera.main.GetComponent<LevelManager>();
        audioSource = menuItems[(int)MenuItems.Results].GetComponent<AudioSource>();
        playingSound = menuItems[(int)MenuItems.Results].GetComponent<PlayingSound>();
    }

    /// <summary>
    /// Отображение результата уровня
    /// </summary>
    public void ShowResult()
    {
        // Отключаем фоновую музыку
        _ = StartCoroutine(backgroundMusic.ChangeVolume(-0.1f, x => x > 0));

        // Отображаем элементы меню
        menuItems[(int)MenuItems.Blackout].SetActive(true);
        menuItems[(int)MenuItems.Characters].SetActive(true);
        menuItems[(int)MenuItems.Menu].SetActive(true);
        menuItems[(int)MenuItems.Results].SetActive(true);

        if (LevelManager.GameMode == "victory")
        {
            // Удваиваем набранные очки
            levelManager.ChangeScore(levelManager.Score);

            // Отображаем панель победы
            menuItems[(int)MenuItems.Victory].SetActive(true);

            // Устанавливаем звук и воспроизводим
            audioSource.clip = audioClips[(int)AudioClips.Victory];
            playingSound.PlaySound();

            var progress = PlayerPrefs.GetInt("progress");
            // Если прогресс викторины не превышает номер уровня, увеличиваем прогресс
            if (progress <= LevelSelection.LevelNumber) PlayerPrefs.SetInt("progress", progress + 1);

            // Увеличиваем общий и текущий счет
            PlayerPrefs.SetInt("total-score", PlayerPrefs.GetInt("total-score") + levelManager.Score);
            PlayerPrefs.SetInt("current-score", PlayerPrefs.GetInt("current-score") + levelManager.Score);

            // Отображаем счетчик очков
            _ = StartCoroutine(ScoreCounter());
        }
        else
        {
            // Отображаем панель проигрыша
            menuItems[(int)MenuItems.Lose].SetActive(true);

            // Устанавливаем звук и воспроизводим
            audioSource.clip = audioClips[(int)AudioClips.Lose];
            playingSound.PlaySound();
        }
    }

    /// <summary>
    /// Счетчик набранных очков
    /// </summary>
    private IEnumerator ScoreCounter()
    {
        // Счетчик очков
        var counter = 0;
        // Шаг увеличения
        var counterStep = 10;
        // Набранные очки за уровень
        var scoreLevel = levelManager.Score;

        while (scoreLevel > 0)
        {
            yield return new WaitForSeconds(0.003f);

            // Уменьшаем счет, увеличиваем счетчик
            if (scoreLevel >= counterStep)
            {
                counter += counterStep;
                scoreLevel -= counterStep;
            }
            else
            {
                counter += scoreLevel;
                scoreLevel = 0;
            }
            
            // Выводим счет на экран
            scoreText.text = counter.ToString();
        }
    }
}