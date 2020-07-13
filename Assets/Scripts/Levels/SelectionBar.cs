using UnityEngine;

namespace Cubra.Levels
{
    public class SelectionBar : MonoBehaviour
    {
        [Header("Кнопки выбора")]
        [SerializeField] private ChangeSelectionButton[] _changeButtons;

        [Header("Компонент носилок")]
        [SerializeField] private Stretcher _stretcher;

        [Header("Улучшенные носилки")]
        [SerializeField] private SuperStretcher _superStretcher;

        private Slowdown _slowdown;

        private void Awake()
        {
            _slowdown = Camera.main.GetComponent<Slowdown>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Controllers.CharacterController character))
            {
                // Если не используются улучшенные носилки
                if (_stretcher.IsSuper == false)
                {
                    for (int i = 0; i < _changeButtons.Length; i++)
                        _changeButtons[i].CustomizeButton(true, Color.white);

                    // Проверяем количество улучшенных носилок
                    _superStretcher.CheckQuantity();

                    // Замедляем коэффициент падения
                    _slowdown.ChangeSlowdown(true);

                    // Указываем, что персонажи переключают носилки
                    character.IsSwitched = true;

                    // Ремонтируем прочность носилок
                    _stretcher.ActiveCoroutine = StartCoroutine(_stretcher.IncreaseStrength(1 + _stretcher.StretcherLevel));
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Controllers.CharacterController character))
            {
                for (int i = 0; i < _changeButtons.Length; i++)
                    _changeButtons[i].CustomizeButton(false, new Color(1, 1, 1, 0.4f));

                _superStretcher.DisableButton();

                // Восстанавливаем коэффициент падения
                _slowdown.ChangeSlowdown(false);

                character.IsSwitched = false;

                if (_stretcher.ActiveCoroutine != null)
                    // Останавливаем постепенное увеличение прочности
                    StopCoroutine(_stretcher.ActiveCoroutine);
            }
        }
    }
}