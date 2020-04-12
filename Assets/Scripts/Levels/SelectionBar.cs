using UnityEngine;

public class SelectionBar : MonoBehaviour
{
    [Header("Панель выбора")]
    [SerializeField] private Animator selectionBar;

    [Header("Компонент носилок")]
    [SerializeField] private Stretcher stretcher;

    // Ссылка на компонент
    private Slowdown slowdown;

    private void Awake()
    {
        slowdown = Camera.main.GetComponent<Slowdown>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент персонажей у коснувшегося объекта
        var characters = collision.gameObject.GetComponent<Control>();

        // Если персонажи касаются пожарной машины и не используются улучшенные носилки
        if (characters == true && stretcher.IsSuper == false)
        {
            // Открываем панель выбора
            selectionBar.enabled = true;
            selectionBar.SetBool("Opening", true);

            // Замедляем коэффициент падения
            slowdown.ChangeSlowdown(true);

            // Указываем, что персонажи переключают носилки
            characters.IsSwitched = true;

            // Ремонтируем (увеличиваем) прочность носилок
            stretcher.ActiveCoroutine = StartCoroutine(stretcher.IncreaseStrength(1 + stretcher.StretcherLevel));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Получаем компонент персонажей у коснувшегося объекта
        var characters = collision.gameObject.GetComponent<Control>();

        if (collision.gameObject.GetComponent<Control>())
        {
            // Закрываем панель выбора
            selectionBar.SetBool("Opening", false);

            // Восстанавливаем коэффициент падения
            slowdown.ChangeSlowdown(false);

            characters.IsSwitched = false;

            // Останавливаем постепенное увеличение прочности
            if (stretcher.ActiveCoroutine != null) StopCoroutine(stretcher.ActiveCoroutine);
        }
    }
}