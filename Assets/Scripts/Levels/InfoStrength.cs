using UnityEngine;
using UnityEngine.UI;

namespace Cubra.Levels
{
    public class InfoStrength : MonoBehaviour
    {
        [Header("Компонент носилок")]
        [SerializeField] private Stretcher _stretcher;

        // Текущее значение прочности
        private int _presentValue;

        private Text _percent;
        private Animator _animator;

        private void Awake()
        {
            _percent = GetComponent<Text>();
            _animator = GetComponent<Animator>();

            _presentValue = _stretcher.Strength;
            _stretcher.StrengthChanged += ShowPercent;
        }

        private void Start()
        {
            ShowPercent();
        }

        /// <summary>
        /// Отображение прочности носилок
        /// </summary>
        private void ShowPercent()
        {
            _percent.text = (_stretcher.Strength > 0 ? _stretcher.Strength.ToString() : "0") + "%";

            // Если значение уменьшилось
            if (_stretcher.Strength < _presentValue)
            {
                // Обновляем текущий процент
                _presentValue = _stretcher.Strength;

                // Отображаем анимацию вычитания
                _animator.enabled = true;
                _animator.Rebind();
            }
        }
    }
}