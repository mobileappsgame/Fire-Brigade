using UnityEngine;

namespace Cubra
{
    public class Achievements : MonoBehaviour
    {
        private void Start()
        {
            // Если доступен интернет
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // Достижение "Вы приняты!"
                if (PlayerPrefs.GetString("training") == "yes") GooglePlayServices.UnlockingAchievement(GPGSIds.achievement);

                // Достижение "Хорошее начало"
                if (PlayerPrefs.GetInt("progress") > 1) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_2);

                // Достижение "Прочные носилки"
                if (PlayerPrefs.GetInt("stretcher") > 1) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_3);

                // Достижение "Опытный спасатель"
                if (PlayerPrefs.GetInt("progress") > 5) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_4);

                // Достижение "Улучшенные носилки"
                if (PlayerPrefs.GetString("use-bonus") == "yes") GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_6);

                // Достижение "Игра наоборот"
                if (PlayerPrefs.GetInt("progress") > 9) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_7);

                // Достижение "Быстрее тушить"
                if (PlayerPrefs.GetString("fire-stretcher") == "yes") GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_8);

                // Достижение "Спасатель года"
                if (PlayerPrefs.GetInt("progress") > 11) GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_9);
            }
        }
    }
}