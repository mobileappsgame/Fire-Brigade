using UnityEngine;
using UnityEngine.UI;

namespace Cubra.Levels
{
    public class SuperStretcher : MonoBehaviour
    {
        [Header("Компонент носилок")]
        [SerializeField] private Stretcher _stretcher;

        [Header("Дочерние изображения")]
        [SerializeField] private Image[] _images;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        /// <summary>
        /// Проверка количества улучшенных носилок
        /// </summary>
        public void CheckQuantity()
        {
            _button.interactable = PlayerPrefs.GetInt("super-stretcher") > 0 ? true : false;

            if (_button.interactable)
            {
                for (int i = 0; i < _images.Length; i++)
                    _images[i].color = Color.white;
            }
        }

        /// <summary>
        /// Использование улучшенных носилок
        /// </summary>
        public void UseImprovement()
        {
            // Запускаем отсчет до возвращения стандартных носилок
            _ = StartCoroutine(_stretcher.SuperiorStretcher());

            // Уменьшаем количество улучшенных носилок
            PlayerPrefs.SetInt("super-stretcher", PlayerPrefs.GetInt("super-stretcher") - 1);
            CheckQuantity();
        }

        /// <summary>
        /// Отключение кнопки
        /// </summary>
        public void DisableButton()
        {
            if (_button.interactable)
            {
                _button.interactable = false;

                for (int i = 0; i < _images.Length; i++)
                    _images[i].color = new Color(1, 1, 1, 0.4f);
            }
        }
    }
}