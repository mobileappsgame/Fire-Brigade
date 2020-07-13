using UnityEngine;
using AppodealAds.Unity.Api;

namespace Cubra
{
    public class AdsManager : MonoBehaviour
    {
        [Header("Рекламный идентификатор")]
        [SerializeField] private string _key;

        private void Awake()
        {
            // Отключаем рекламные звуки
            Appodeal.muteVideosIfCallsMuted(true);
            // Инициализируем полноэкранный баннер
            Appodeal.initialize(_key, Appodeal.INTERSTITIAL, true);
        }
    }
}