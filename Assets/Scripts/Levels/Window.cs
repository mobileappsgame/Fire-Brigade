using System.Collections;
using UnityEngine;

public class Window : MonoBehaviour, IPoolable
{
    // Открытость текущего окна
    public bool OpenWindow { get; set; } = false;

    // Активность создания огненных капель
    public bool Twinkle { get; set; } = false;

    [Header("Эффект пожара в окне")]
    [SerializeField] private GameObject fireFX;

    [Header("Пул персонажей")] // ключ пула
    [SerializeField] private string victims;

    // Промежуток для создания капель
    [Header("Минимальное время для капель")]
    [SerializeField] private float minSeconds;
    [Header("Максимальное время для капель")]
    [SerializeField] private float maxSeconds;

    [Header("Предупреждение о прыжке")]
    [SerializeField] private GameObject exclamation;

    // Ссылка на анимацию
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    /// <summary>
    /// Активация объекта после взятия из пула
    /// </summary>
    public void ActivateObject()
    {
        // Активируем капли
        Twinkle = true;

        // Если окно уже открыто
        if (OpenWindow)
        {
            // Отображаем пожар сразу
            animator.Play("Fire");
        }
        else
        {
            // Открываем окно
            OpenWindow = true;
            // Запускаем постепенный пожар
            animator.SetBool("Fire", true);
        }
    }

    /// <summary>
    /// Добавление окна в список доступных для жильцов
    /// </summary>
    public void AddToList()
    {
        WindowsManager.windows.Add(this);
    }

    /// <summary>
    /// Отображение огненного эффекта
    /// </summary>
    public void ShowFireEffect()
    {
        fireFX.SetActive(true);

        // Запускаем создание огненных капель
        StartCoroutine(DropsFalling());
    }

    /// <summary>
    /// Создание огненных капель в горящем окне
    /// </summary>
    private IEnumerator DropsFalling()
    {
        while (OpenWindow)
        {
            yield return new WaitForSeconds(Random.Range(minSeconds, maxSeconds));

            if (Twinkle)
            {
                // Получаем объект из пула и получаем его компонент
                var drop = PoolsManager.GetObjectFromPool(ListingPools.Pools.Twinkle.ToString()).GetComponent<Drop>();

                // Перемещаем каплю к горящему окну (с небольшим горизонтальным смещением)
                drop.transform.position = fireFX.transform.position + new Vector3(-0.15f, Random.Range(-0.2f, 0.2f), 0);

                // Активируем объект
                drop.ActivateObject();
            }
        }
    }

    /// <summary>
    /// Получение персонажа из пула объектов
    /// </summary>
    public void ShowVictims()
    {
        // Если в указанном пуле есть персонажи
        if (PoolsManager.QuantityObjects(ListingPools.Pools.GreenMen.ToString()) > 0)
        {
            // Отключаем капли
            Twinkle = false;

            // Получаем объект персонажа из пула и получаем его компонент
            var victim = PoolsManager.GetObjectFromPool(victims).GetComponent<Victims>();

            // Перемещаем персонажа в текущее окно
            victim.transform.position = transform.position + new Vector3(0, victim.Offset, 0);

            // Записываем персонажу его окно
            victim.Window = this;

            // Активируем объект
            victim.ActivateObject();
        }
    }

    /// <summary>
    /// Деактивация объекта при возвращении в пул
    /// </summary>
    public void DeactivateObject()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Отображение/скрытие предупреждения о прыжке персонажа
    /// </summary>
    /// <param name="state">Видимость объекта</param>
    public void ShowWarning(bool state)
    {
        exclamation.SetActive(state);
    }

    /// <summary>
    /// Возвращение окна в список доступных для персонажей
    /// </summary>
    public IEnumerator ReestablishWindow()
    {
        yield return new WaitForSeconds(1.0f);
        // Отправляем окно в список
        AddToList();
        // Восстанавливаем огненные капли
        Twinkle = true;
    }
}