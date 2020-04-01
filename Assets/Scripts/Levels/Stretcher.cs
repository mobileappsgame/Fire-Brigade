using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stretcher : MonoBehaviour
{
    // Событие по уничтожению носилок
    public UnityEvent OnDestroyStretcher;

    public delegate void ValueChanged();
    // Событие по изменению прочности
    public event ValueChanged StrengthChanged;

    // Прочность носилок
    public int Strength { get; private set; } = 100;

    // Уровень носилок
    public int StretcherLevel { get; private set; }

    // Цветовой статус носилок
    public string Status { get; private set; }

    // Предыдущий цветовой статус
    private int previousStatus;

    // Горят ли носилки
    public bool IsBurns { get; private set; }

    // Используются ли улучшенные носилки
    public bool IsSuper { get; private set; }

    // Перечисление анимаций носилок
    private enum State { Green, Orange, Red, Yellow, Different, Destroy, Broken }

    // Ссылка на активную корутину
    public Coroutine ActiveCoroutine { get; set; }

    // Ссылки на компоненты
    private Animator animator;
    private BoxCollider2D boxCollider;
    private Flames flames;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        flames = GetComponentInChildren<Flames>();
        flames.Extinguished += PutOutStretcher;

        // Устанавливаем зеленый статус носилок
        Status = Statuses.ColorStatuses.Green.ToString();

        // Получаем уровень носилок
        StretcherLevel = PlayerPrefs.GetInt("stretcher");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огненная капля касается носилок
        if (collision.gameObject.GetComponent<Drop>())
        {
            // Если носилки не горят и не улучшенные
            if (IsBurns == false && IsSuper == false)
            {
                // Возвращаем каплю в указанный пул объектов
                PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);

                // Поджигаем носилки
                flames.FlameVisibility(true);
                IsBurns = true;

                // Запускаем уменьшение прочности носилок (с учетом уровня носилок)
                ActiveCoroutine = StartCoroutine(ReduceStrength(10 - StretcherLevel));
            }
        }
    }

    /// <summary>
    /// Тушение носилок
    /// </summary>
    private void PutOutStretcher()
    {
        IsBurns = false;
        // Сбрасываем уменьшение прочности
        StopCoroutine(ActiveCoroutine);
    }

    /// <summary>
    /// Постепенное уменьшение прочности носилок
    /// </summary>
    /// <param name="value">значение для вычитания</param>
    private IEnumerator ReduceStrength(int value)
    {
        while (Strength > 0)
        {
            yield return new WaitForSeconds(1.0f);
            ChangeStrength(-value);
        }

        CheckStrength();
    }

    /// <summary>
    /// Постепенное увеличение прочности носилок
    /// </summary>
    /// <param name="value">значение для добавления</param>
    public IEnumerator IncreaseStrength(int value)
    {
        while (Strength < 100 && IsBurns == false)
        {
            yield return new WaitForSeconds(1.0f);
            ChangeStrength(value);
        }
    }

    /// <summary>
    /// Изменение прочности носилок
    /// </summary>
    /// <param name="value">значение</param>
    public void ChangeStrength(int value)
    {
        Strength += value;

        // Удаляем излишек прочности
        if (Strength > 100) Strength = 100;

        // Сообщаем об изменении
        StrengthChanged?.Invoke();
    }

    /// <summary>
    /// Проверка прочности носилок
    /// </summary>
    public void CheckStrength()
    {
        if (Strength <= 0)
        {
            if (IsBurns == true)
            {
                // Скрываем все огоньки
                flames.FlameVisibility(false);

                // Отображаем сгоревшие носилки
                ChangeAnimation((int)State.Destroy);
            }
            else
            {
                // Отображаем сломанные носилки
                ChangeAnimation((int)State.Broken);
                boxCollider.enabled = false;
            }

            // Сообщаем об уничтожении
            OnDestroyStretcher?.Invoke();
        }
    }

    /// <summary>
    /// Переключение анимации носилок
    /// </summary>
    /// <param name="number">номер анимации</param>
    public void ChangeAnimation(int number)
    {
        // Записываем предыдущий номер анимации
        previousStatus = animator.GetInteger("State");

        animator.SetInteger("State", number);
        Status = ((Statuses.ColorStatuses)number).ToString();
    }

    /// <summary>
    /// Восстановление носилок после использования улучшения
    /// </summary>
    public IEnumerator SuperiorStretcher()
    {
        IsSuper = true;
        Strength = 100;

        yield return new WaitForSeconds(20.0f);

        // Восстанавливаем предыдущие носилки
        ChangeAnimation(previousStatus);
        IsSuper = false;
    }

    private void OnDestroy()
    {
        // Сбрасываем подписчиков
        OnDestroyStretcher = null;
        StrengthChanged = null;
    }
}