using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stretcher : MonoBehaviour
{
    // Событие по уничтожению носилок
    public UnityEvent OnDestroyStretcher;

    // Тушение огня на носилках
    public static Action SnuffOut;

    // Прочность носилок
    public int Strength { get; private set; } = 100;

    // Уровень носилок
    private int stretcherLevel;

    // Цветовой статус носилок
    public static string Status { get; private set; }

    // Предщыдущий номер анимации носилок (для возврата после улучшения)
    private int previousNumber;

    // Горят ли носилки
    public static bool IsBurns { get; private set; }

    // Используются ли улучшенные носилки
    public static bool IsSuper { get; private set; }

    [Header("Огоньки на носилках")]
    [SerializeField] private GameObject[] lights;

    // Перечисление анимаций носилок
    private enum State { Green, Orange, Red, Yellow, Different, Destroy, Broken }

    [Header("Компонент выбора")]
    [SerializeField] private StretcherChange stretcherChange;

    // Ссылка на запущенную корутину
    public Coroutine Coroutine { get; set; }

    // Ссылки на компоненты
    private Animator animator;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        SnuffOut += QuantityLights;

        // Устанавливаем зеленый статус носилок
        Status = Statuses.ColorStatuses.Green.ToString();

        // Получаем уровень носилок
        stretcherLevel = PlayerPrefs.GetInt("stretcher");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огненная капля касается носилок
        if (collision.gameObject.GetComponent<Drop>())
        {
            if (IsBurns == false && IsSuper == false)
            {
                // Возвращаем объект в указанный пул объектов
                PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);
                
                // Поджигаем носилки
                LightVisibility(true);
                IsBurns = true;

                // Запускаем уменьшение прочности носилок
                Coroutine = StartCoroutine(ReduceStrength(10 - stretcherLevel));
            }
        }
    }

    /// <summary>
    /// Установка видимости огоньков на носилках
    /// </summary>
    /// <param name="state">Видимость огоньков</param>
    private void LightVisibility(bool state)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(state);
        }
    }

    /// <summary>
    /// Проверка оставшихся огоньков на носилках
    /// </summary>
    private void QuantityLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            // Если есть видимые огоньки, выходим
            if (lights[i].activeInHierarchy) return;
        }

        // Сбрасываем горение
        IsBurns = false;

        // Останавливаем уменьшение прочности
        StopCoroutine(Coroutine);
    }

    /// <summary>
    /// Уменьшение прочности носилок
    /// </summary>
    /// <param name="value">Значение для вычитания</param>
    private IEnumerator ReduceStrength(int value)
    {
        while (Strength > 0)
        {
            yield return new WaitForSeconds(1.0f);
            ChangeStrength(-value);
        }
        
        DestroyStretcher();
    }

    /// <summary>
    /// Увеличение прочности носилок
    /// </summary>
    public IEnumerator IncreaseStrength()
    {
        while (Strength < 100 && IsBurns == false)
        {
            yield return new WaitForSeconds(1.0f);
            ChangeStrength(2);
        }
    }

    /// <summary>
    /// Изменение прочности носилок
    /// </summary>
    /// <param name="value">Значение</param>
    public void ChangeStrength(int value)
    {
        // Изменяем прочность носилок
        Strength += value;

        // Удаляем излишек прочности
        if (Strength > 100) Strength = 100;

        // Обновляем проценты в текстовом поле
        StretcherStrength.StrengthChange?.Invoke();
    }

    /// <summary>
    /// Уничтожение носилок
    /// </summary>
    private void DestroyStretcher()
    {
        // Скрываем все огоньки
        LightVisibility(false);

        // Переключаем анимацию на уничтожение
        ChangeAnimation((int)State.Destroy);

        // Сообщаем об уничтожении
        OnDestroyStretcher?.Invoke();
    }

    /// <summary>
    /// Проверка прочности носилок
    /// </summary>
    public void CheckStrength()
    {
        if (Strength <= 0)
        {
            // Отображаем сломанные носилки
            ChangeAnimation((int)State.Broken);

            // Отключаем коллайдер носилок
            boxCollider.enabled = false;

            // Вызываем событие уничтоженных носилок
            OnDestroyStretcher?.Invoke();
        }
    }

    /// <summary>
    /// Переключение анимации носилок
    /// </summary>
    /// <param name="number">Параметр анимации</param>
    public void ChangeAnimation(int number)
    {
        // Записываем предыдущий номер анимации носилок
        previousNumber = animator.GetInteger("State");

        animator.SetInteger("State", number);

        // Устанавливаем цветовой статус носилок
        Status = ((Statuses.ColorStatuses)number).ToString();
    }

    /// <summary>
    /// Восстановление носилок после использования улучшения
    /// </summary>
    public IEnumerator SuperiorStretcher()
    {
        IsSuper = true;
        ChangeStrength(50);

        // Отключаем панель выбора
        stretcherChange.ChangeCollider(false);

        yield return new WaitForSeconds(20.0f);

        // Восстанавливаем панель выбора
        stretcherChange.ChangeCollider(true);

        // Восстанавливаем предыдущие носилки
        ChangeAnimation(previousNumber);
        IsSuper = false;
    }

    private void OnDestroy()
    {
        SnuffOut = null;
    }
}