using UnityEngine;

public class StretcherChange : MonoBehaviour
{
    [Header("Панель выбора")]
    [SerializeField] private Animator panel;

    [Header("Компонент носилок")]
    [SerializeField] private Stretcher stretcher;

    // Ссылка на компонент
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если персонажи касаются пожарной машины
        if (collision.gameObject.GetComponent<Control>())
        {
            panel.enabled = true;

            // Открываем панель выбора
            panel.SetBool("Opening", true);

            // Замедляем коэффициент падения
            Slowdown.SlowDown?.Invoke(true);

            // Ремонтируем (увеличиваем) прочность носилок
            stretcher.Coroutine = StartCoroutine(stretcher.IncreaseStrength());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Control>())
        {
            // Закрываем панель выбора
            panel.SetBool("Opening", false);

            // Восстанавливаем коэффициент падения
            Slowdown.SlowDown?.Invoke(false);

            // Останавливаем увеличение прочности
            if (stretcher.Coroutine != null) StopCoroutine(stretcher.Coroutine);
        }
    }

    /// <summary>
    /// Включение/отключение коллайдера, вызывающего панель выбора носилок
    /// </summary>
    public void ChangeCollider(bool state)
    {
        boxCollider.enabled = state;
    }
}