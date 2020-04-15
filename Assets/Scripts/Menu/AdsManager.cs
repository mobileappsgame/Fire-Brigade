using UnityEngine;
using AppodealAds.Unity.Api;

public class AdsManager : MonoBehaviour
{
    [Header("Рекламный идентификатор")]
    [SerializeField] private string key;

    private void Awake()
    {
        // Отключаем рекламные звуки
        Appodeal.muteVideosIfCallsMuted(true);

        // Инициализируем рекламный баннер и видеорекламу с вознаграждением
        Appodeal.initialize(key, Appodeal.INTERSTITIAL, true);
    }
}