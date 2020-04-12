using UnityEngine;

public class PlayServices : MonoBehaviour
{
    private void Start()
    {
        SignGooglePlay();
    }

    /// <summary>
    /// Подключение к сервисам Google Play
    /// </summary>
    public static void SignGooglePlay()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
            Social.localUser.Authenticate((bool success) => {});
    }

    /// <summary>
    /// Просмотр игровых достижений
    /// </summary>
    public void ShowAchievements()
    {
        // Если пользователь авторизирован, отображаем список достижений
        if (Social.localUser.authenticated) Social.ShowAchievementsUI();
        else SignGooglePlay();
    }

    /// <summary>
    /// Получение нового достижения
    /// </summary>
    /// <param name="identifier">идентификатор достижения</param>
    public static void UnlockingAchievement(string identifier)
    {
        if (Social.localUser.authenticated)
            Social.ReportProgress(identifier, 100.0f, (bool success) => {});
    }

    /// <summary>
    /// Отправка результата в таблицу лидеров
    /// </summary>
    /// <param name="score">счет игрока</param>
    public static void PostingScoreLeaderboard(int score)
    {
        if (Social.localUser.authenticated)
            Social.ReportScore(score, GPGSIds.leaderboard, (bool success) => {});
    }
}