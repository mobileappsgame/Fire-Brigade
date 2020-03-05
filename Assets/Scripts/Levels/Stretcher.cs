using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stretcher : MonoBehaviour
{
    // Событие при сгорании носилок
    public UnityEvent BurnedOut = new UnityEvent();

    // Тушение носилок
    public static Action SnuffOut;

    // Прочность носилок
    public int Strength { get; private set; } = 100;

    public string Status { get; private set; }

    // Горят ли сейчас носилки
    public static bool IsBurns { get; private set; }

    [Header("Огоньки на носилках")]
    [SerializeField] private GameObject[] lights;

    // Перечисление анимаций носилок
    private enum State { Green, Orange, Red, Yellow, Different, Destroy }

    // Ссылка на компонент
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Добавляем проверку огоньков
        SnuffOut += CheckQuantityLight;
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

                // Запускаем отсчет до уничтожения носилок
                StartCoroutine(DestroyStretcher());

                // Активируем переменную огня
                IsBurns = true;
            }
        }
    }

    /// <summary>
    /// Установка видимости огоньков на носилках
    /// </summary>
    private void LightVisibility(bool state)
    {
        for (int i = 0; i < lights.Length; i++)
            lights[i].SetActive(state);
    }

    /// <summary>
    /// Проверка оставшихся огоньков на носилках
    /// </summary>
    private void CheckQuantityLight()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            // Если есть видимые огоньки, выходим из метода
            if (lights[i].activeInHierarchy) return;
        }

        // Сбрасываем горение носилок
        IsBurns = false;
        // Останавливаем отсчет уничтожения
        StopAllCoroutines();
    }

    /// <summary>
    /// Отсчет до уничтожения носилок от огня
    /// </summary>
    private IEnumerator DestroyStretcher()
    {
        while (Strength > 0)
        {
            yield return new WaitForSeconds(1.0f);

            // Уменьшаем прочность носилок
            Strength -= 17;

            // Обновляем проценты в текстовом поле
            StretcherStrength.StrengthChange?.Invoke();
        }

        // Скрываем огонь на носилках
        LightVisibility(false);

        // Переключаем анимацию носилок на уничтожение
        ChangeAnimation((int)State.Destroy);

        // Вызываем событие сгоревших носилок
        BurnedOut?.Invoke();
    }

    /// <summary>
    /// Переключение анимации носилок
    /// </summary>
    /// <param name="number">Значение параметра анимации</param>
    public void ChangeAnimation(int number)
    {
        animator.SetInteger("State", number);
    }
}