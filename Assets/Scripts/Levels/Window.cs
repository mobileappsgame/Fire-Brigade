using System.Collections;
using UnityEngine;

public class Window : MonoBehaviour, IPoolable
{
    // Открытость окна с пожаром
    public bool OpenWindow { get; set; } = false;

    // Активность создания капель
    public bool Twinkle { get; set; } = false;

    [Header("Эффект пожара в окне")]
    [SerializeField] private GameObject fireFX;

    [Header("Пул персонажей")] // ключ пула
    [SerializeField] private string victims;

    // Ссылка на компонент
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
        // Включаем капли
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
            yield return new WaitForSeconds(Random.Range(5f, 12.5f));

            if (Twinkle)
            {
                // Получаем объект из пула и получаем его компонент
                var drop = PoolsManager.GetObjectFromPool(ListingPools.Pools.Twinkle.ToString()).GetComponent<Drop>();

                // Перемещаем каплю к горящему окну (с небольщим смещением)
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
            victim.transform.position = transform.position;

            // Записываем окно, из которого прыгает персонаж
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
}