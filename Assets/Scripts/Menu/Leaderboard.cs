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

            // Отправляем свой результат в таблицу лидеров
            PlayServices.PostingScoreLeaderboard(PlayerPrefs.GetInt("total-score") * PlayerPrefs.GetInt("victims"));

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
                // Записываем позицию текущего игрока
                leaders.Rating = data.PlayerScore.rank;
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
            // Очищаем результаты
            leaders.Users.Clear();

            for (int i = 0; i < scores.Length; i++)
            {
                // Создаем пользователя и ищем его id массиве
                IUserProfile user = FindUser(users, scores[i].userID);
                // Добавляем результат в список
                leaders.Users.Add(new User() { Name = (user != null) ? user.userName : "Unknown", Result = scores[i].value });
            }

            // Записываем результаты в файл
            WriteToFile("leaderboard", ref leaders);

            // Скрываем загрузку
            loading.SetActive(false);

            // Выводим результаты на экран
            ShowResultsFile();
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
        if (leaders.Users.Count > 0)
        {
            // Выводим результат текущего игрока
            myRating.text = leaders.Rating.ToString();

            for (int i = 0; i < leaders.Users.Count; i++)
            {
                // Выводим лучшие результаты остальных игроков
                leaderboard.text += (i + 1) + " - " + leaders.Users[i].Name.ToUpper() + " (" + leaders.Users[i].Result.ToString() + ")";
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