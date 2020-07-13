using UnityEngine;
using UnityEngine.UI;

namespace Cubra.Levels
{
    public class InfoScore : MonoBehaviour
    {
        [Header("Текст с очками")]
        [SerializeField] private Text _changeScore;

        private Text _score;
        private Outline _outlineScore;
        private Animator _animatorScore;
        private LevelManager _levelManager;

        private void Awake()
        {
            _score = GetComponent<Text>();
            _outlineScore = _changeScore.GetComponent<Outline>();
            _animatorScore = _changeScore.GetComponent<Animator>();

            _levelManager = Camera.main.GetComponent<LevelManager>();
            // Подписываем в событие метод обновления счета
            _levelManager.ScoresChanged += UpdateLevelScore;
        }

        /// <summary>
        /// Отображение текущего счета
        /// </summary>
        /// <param name="value">значение</param>
        public void UpdateLevelScore(int value)
        {
            _score.text = _levelManager.Score.ToString();

            _animatorScore.enabled = true;
            // Записываем количество очков в эффект изменения
            _changeScore.text = (value > 0 ? "+ " : "") + value.ToString();
            // Устанавливаем обводку эффекта в зависимости от значения
            _outlineScore.effectColor = value > 0 ? Color.green : Color.red;
            // Перезапускаем анимацию
            _animatorScore.Rebind();
        }
    }
}