using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using Cubra.Heplers;

namespace Cubra
{
    public class Leaderboard : FileProcessing
    {
        [Header("Рейтинг игрока")]
        [SerializeField] private Text _myRating;

        [Header("Таблица лидеров")]
        [SerializeField] private Text _leaderboard;

        [Header("Анимация загрузки")]
        [SerializeField] private GameObject _loading;

        // Объект для json по таблице лидеров
        private LeaderboardHelper _leaders;

        private void Awake()
        {
            _leaders = new LeaderboardHelper();

            var json = ReadJsonFile("leaderboard");
            // Преобразовываем json в объект
            ConvertToObject(ref _leaders, json);
        }

        private void Start()
        {
            // Если доступен интернет и пользователь авторизирован
            if (Application.internetReachability != NetworkReachability.NotReachable && Social.localUser.authenticated)
            {
                // Отображаем загрузку
                _loading.SetActive(true);

                // Подсчитываем и отправляем свой результат в таблицу лидеров
                var score = PlayerPrefs.GetInt("total-score") * PlayerPrefs.GetInt("victims");
                GooglePlayServices.PostingScoreLeaderboard(score);

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
                    _leaders.Rating = data.PlayerScore.rank;
                    _myRating.text = data.PlayerScore.rank.ToString();

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
                _loading.SetActive(false);

                for (int i = 0; i < scores.Length; i++)
                {
                    // Создаем пользователя и ищем его id массиве
                    IUserProfile user = FindUser(users, scores[i].userID);

                    // Выводим результаты в текстовое поле
                    _leaderboard.text += i + 1 + " - " + ((user != null) ? user.userName.ToUpper() : "UNKNOWN") + " (" + scores[i].value.ToString() + ")";
                    if (i < 9) _leaderboard.text += "\n";

                    // Записываем в json имена и результаты игроков
                    _leaders.Names[i] = (user != null) ? user.userName.ToUpper() : "UNKNOWN";
                    _leaders.Results[i] = scores[i].value;
                }

                // Записываем результаты в файл
                WriteToFile("leaderboard", ref _leaders);
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
            if (_leaders.Rating > 0)
            {
                // Выводим результат текущего игрока
                _myRating.text = _leaders.Rating.ToString();

                for (int i = 0; i < _leaders.Results.Length; i++)
                {
                    // Выводим сохраненные данные
                    _leaderboard.text += (i + 1) + " - " + _leaders.Names[i] + " (" + _leaders.Results[i] + ")";
                    if (i < 9) _leaderboard.text += "\n";
                }
            }
            else
            {
                // Выводим стандартный текст
                var translation = _leaderboard.gameObject.GetComponent<TextTranslation>();
                translation.enabled = true;
                translation.TranslateText();
            }
        }
    }
}