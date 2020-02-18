using UnityEngine;

public class StretcherChange : MonoBehaviour
{
    [Header("Анимация панели выбора")]
    [SerializeField] private Animator panelStretcher;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если персонажи касаются пожарной машины
        if (collision.gameObject.GetComponent<Control>())
        {
            // Если аниматор отключен, активируем
            if (panelStretcher.enabled == false)
                panelStretcher.enabled = true;

            // Включаем состояние открытия
            panelStretcher.SetBool("Opening", true);

            // Вызываем изменение коэффициента падения
            Slowdown.SlowDown?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Control>())
        {
            // Переключаемся на состояние закрытия
            panelStretcher.SetBool("Opening", false);

            // Вызываем изменение коэффициента падения
            Slowdown.SlowDown?.Invoke(false);
        }
    }
}