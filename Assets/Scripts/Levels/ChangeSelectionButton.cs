using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Cubra.Levels
{
    public class ChangeSelectionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Дочерние изображения")]
        [SerializeField] private Image[] _images;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        /// <summary>
        /// Нажатие на кнопку выбора носилок
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        /// <summary>
        /// Отпускание кнопки выбора носилок
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        /// <summary>
        /// Настройка кнопки выбора
        /// </summary>
        /// <param name="activity">активность кнопки</param>
        /// <param name="color">цвет кнопки</param>
        public void CustomizeButton(bool activity, Color color)
        {
            _button.interactable = activity;

            for (int i = 0; i < _images.Length; i++)
                _images[i].color = color;
        }
    }
}