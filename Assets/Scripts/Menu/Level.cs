using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public class Level : MonoBehaviour
    {
        [Header("Номер уровня")]
        [SerializeField] private int _number;

        public int Number => _number;

        [Header("Текст с номером")]
        [SerializeField] private GameObject _textNumber;

        [Header("Статус уровня")]
        [SerializeField] private Image _status;

        [Header("Спрайты статусов")]
        [SerializeField] private Sprite[] _statuses;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            // Если игровой прогресс достаточный
            if (PlayerPrefs.GetInt("progress") == _number)
            {
                // Скрываем замок и показываем номер
                _status.gameObject.SetActive(false);
                _textNumber.SetActive(true);

                _button.interactable = true;
            }
            else if (PlayerPrefs.GetInt("progress") > _number)
            {
                // Меняем замок на галочку
                _status.sprite = _statuses[1];

                _button.interactable = true;
            }
        }
    }
}