using System.Collections;
using UnityEngine;

public class StretcherChange : MonoBehaviour
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

            StartCoroutine(ChangeTimeScale(0.5f));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Если персонажи больше не касаются машины
        if (collision.gameObject.GetComponent<Control>())
        {
            // Переключаемся на состояние закрытия
            panelStretcher.SetBool("Opening", false);

            StartCoroutine(ChangeTimeScale(1.0f));
        }
    }

    /// <summary>
    /// Изменение течения времени
    /// </summary>
    /// <param name="scale">Значение времени</param>
    private IEnumerator ChangeTimeScale(float scale)
    {
        yield return new WaitForSeconds(0.12f);
        Time.timeScale = scale;
    }
}