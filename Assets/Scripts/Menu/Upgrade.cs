using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public class Upgrade : MonoBehaviour
    {
        [Header("Стоимость повышения")]
        [SerializeField] private int[] _upgradeCost;

        [Header("Компонент перевода")]
        [SerializeField] private TextValue _level;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            CheckCurrentScore();
        }

        /// <summary>
        /// Проверка счета для улучшения носилок
        /// </summary>
        public void CheckCurrentScore()
        {
            // Текущий уровень носилок
            var currentLevel = PlayerPrefs.GetInt("stretcher");
            // Достаточно ли текущего счета для улучшения
            var enough = (currentLevel < 5) && (PlayerPrefs.GetInt("current-score") >= _upgradeCost[currentLevel]);

            _button.interactable = enough ? true : false;
        }

        /// <summary>
        /// Улучшение носилок
        /// </summary>
        public void UpgradeStretcher()
        {
            var currentLevel = PlayerPrefs.GetInt("stretcher");

            // Уменьшаем текущее количество очков
            PlayerPrefs.SetInt("current-score", PlayerPrefs.GetInt("current-score") - _upgradeCost[currentLevel]);
            // Увеличиваем уровень носилок
            PlayerPrefs.SetInt("stretcher", currentLevel + 1);

            // Открываем достижение по улучшению носилок
            if (Application.internetReachability != NetworkReachability.NotReachable)
                GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_3);

            // Обновляем перевод
            _level.TranslateText();

            CheckCurrentScore();
        }
    }
}