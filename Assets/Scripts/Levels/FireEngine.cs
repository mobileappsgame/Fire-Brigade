using UnityEngine;

public class FireEngine : MonoBehaviour
{
    [Header("Аниматор панели выбора")]
    [SerializeField] private Animator panelStretcher;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если персонажи касаются машины
        if (collision.gameObject.GetComponent<Control>())
        {
            // Если аниматор отключен, активируем
            if (panelStretcher.enabled == false)
                panelStretcher.enabled = true;

            // Включаем состояние открытия
            panelStretcher.SetBool("Opening", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Если персонажи больше не касаются машины
        if (collision.gameObject.GetComponent<Control>())
        {
            // Переключаемся на состояние закрытия
            panelStretcher.SetBool("Opening", false);
        }
    }
}