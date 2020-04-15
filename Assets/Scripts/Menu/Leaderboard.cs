using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Leaderboard : FileProcessing
{
    [Header("Рейтинг игрока")]
    [SerializeField] private Text myRating;

    [Header("Таблица лидеров")]
    [SerializeField] private Text leaderboard;

    [Header("Анимация загрузки")]
    [SerializeField] private GameObject loading;

    // Объект для работы с json по таблице лидеров
    private LeaJson leaders = new LeaJson();

    private void Awake()
    {
        // Читаем файл с лидерами
        var json = ReadJsonFile("leaderboard");

        // Преобразовываем json в объект
        ConvertToObject(ref leaders, json);
    }

    private void Start()
    {
        // Если доступен интернет и пользователь авторизирован
        if (Application.internetReachability != NetworkReachability.NotReachable && Social.localUser.authenticated)
        {
            // Отображаем загрузку
            loading.SetActive(true);

            // Подсчитываем и отправляем свой результат в таблицу лидеров
            var score = PlayerPrefs.GetInt("total-score") * PlayerPrefs.GetInt("victims");
            PlayServices.PostingScoreLeaderboard(score);

            // Загружаем результаты
            LoadScoresLeaderboard();
        }
        else
        {
            ShowResultsFile();
        }
    }

    /// <summary>
    /// Загрузка результатов из удаленной таблицы лидеров
    /// </summary>
    public void LoadScoresLeaderboard()
    {
        //Загружаем десять лучших результатов
        PlayGamesPlatform.Instance.LoadScores(
            GPGSIds.leaderboard,
            LeaderboardStart.TopScores,
            10,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            (data) =>
            {
                // Записываем и выводим позицию текущего игрока
                leaders.Rating = data.PlayerScore.rank;
                myRating.text = data.PlayerScore.rank.ToString();

                // Загружаем информацию по другим игрокам
                LoadUsersLeaderboard(data.Scores);
            }
        );
    }

    /// <summary>
    /// Загрузка и отображение информации по игрокам
    /// </summary>
    /// <param name="scores">массив результатов</param>
    private void LoadUsersLeaderboard(IScore[] scores)
    {
        // Список id пользователей
        var userIds = new List<string>();
        // Перебираем результаты и добавляем id в список
        foreach (IScore score in scores) userIds.Add(score.userID);

        // Загружаем информацию по пользователям
        Social.LoadUsers(userIds.ToArray(), (users) =>
        {
            // Скрываем загрузку
            loading.SetActive(false);

            for (int i = 0; i < scores.Length; i++)
            {
                // Создаем пользователя и ищем его id массиве
                IUserProfile user = FindUser(users, scores[i].userID);

                // Выводим результаты в текстовое поле
                leaderboard.text += i + 1 + " - " + ((user != null) ? user.userName.ToUpper() : "UNKNOWN") + " (" + scores[i].value.ToString() + ")";
                if (i < 9) leaderboard.text += "\n";

                // Записываем в json имена и результаты игроков
                leaders.Names[i] = (user != null) ? user.userName.ToUpper() : "UNKNOWN";
                leaders.Results[i] = scores[i].value;
            }

            // Записываем результаты в файл
            WriteToFile("leaderboard", ref leaders);
        });
    }

    /// <summary>
    /// Поиск игрока в массиве по id
    /// </summary>
    /// <param name="users">список игроков</param>
    /// <param name="userid">id игрока</param>
    /// <returns>найденный пользователь</returns>
    private IUserProfile FindUser(IUserProfile[] users, string userid)
    {
        foreach (IUserProfile user in users)
        {
            // Если id совпадают, возвращаем игрока
            if (user.id == userid) return user;
        }

        return null;
    }

    /// <summary>
    /// Отображение сохраненных данных по игрокам
    /// </summary>
    private void ShowResultsFile()
    {
        if (leaders.Rating > 0)
        {
            // Выводим результат текущего игрока
            myRating.text = leaders.Rating.ToString();

            for (int i = 0; i < leaders.Results.Length; i++)
            {
                // Выводим сохраненные данные
                leaderboard.text += (i + 1) + " - " + leaders.Names[i] + " (" + leaders.Results[i] + ")";
                if (i < 9) leaderboard.text += "\n";
            }
        }
        else
        {
            // Выводим стандартный текст
            var translation = leaderboard.gameObject.GetComponent<TextTranslation>();
            translation.enabled = true;
            translation.TranslateText();
        }
    }
}