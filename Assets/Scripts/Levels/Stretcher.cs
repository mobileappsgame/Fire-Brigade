using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stretcher : MonoBehaviour
{
    // Событие по уничтожению носилок
    public UnityEvent OnDestroyStretcher = new UnityEvent();

    // Тушение огня на носилках
    public static Action SnuffOut;

    // Прочность носилок
    public int Strength { get; private set; } = 100;

    // Цветовой статус носилок (изначально зеленый)
    public static string Status { get; private set; } = Statuses.ColorStatuses.Green.ToString();

    // Горят ли носилки
    public static bool IsBurns { get; private set; }

    [Header("Огоньки на носилках")]
    [SerializeField] private GameObject[] lights;

    // Перечисление анимаций носилок
    private enum State { Green, Orange, Red, Yellow, Different, Destroy, Broken }

    // Ссылка на запущенную корутину
    public Coroutine Coroutine { get; set; } = null;

    // Ссылки на компоненты
    private Animator animator;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        SnuffOut += QuantityLights;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огненная капля касается носилок
        if (collision.gameObject.GetComponent<Drop>())
        {
            if (IsBurns == false)
            {
                // Возвращаем объект в указанный пул объектов
                PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);
                
                // Поджигаем носилки
                LightVisibility(true);
                IsBurns = true;

                // Запускаем уменьшение прочности носилок
                StartCoroutine(ReduceStrength(10));
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
            // Если есть видимые огоньки, выходим из метода
            if (lights[i].activeInHierarchy) return;
        }

        // Сбрасываем горение
        IsBurns = false;

        // Останавливаем уменьшение прочности
        StopAllCoroutines();
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
    /// <param name="number">Значение параметра анимации</param>
    public void ChangeAnimation(int number)
    {
        animator.SetInteger("State", number);

        // Устанавливаем цветовой статус носилок
        Status = ((Statuses.ColorStatuses)number).ToString();
    }

    private void OnDestroy()
    {
        SnuffOut = null;
    }
}