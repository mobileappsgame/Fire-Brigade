using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stretcher : MonoBehaviour
{
    // Событие при сгорании носилок
    public UnityEvent BurnedOut = new UnityEvent();

    // Событие по тушению носилок
    public static UnityEvent SnuffOut = new UnityEvent();

    // Горят ли сейчас носилки
    public bool IsBurns { get; private set; }

    [Header("Огоньки на носилках")]
    [SerializeField] private GameObject[] lights;

    // Перечисление анимаций носилок
    private enum State { Green, Orange, Red, Yellow, Different, Destroy }

    // Ссылка на компонент
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Подписываем проверку огоньков в событие тушения
        SnuffOut.AddListener(CheckQuantityLight);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огненная капля касается носилок
        if (collision.gameObject.GetComponent<Drop>())
        {
            // Возвращаем объект в нужный пул
            PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);

            // Отображаем огонь на носилках
            LightVisibility(true);

            if (IsBurns == false)
            {
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
            // Если есть активные огоньки, выходим из метода
            if (lights[i].activeInHierarchy) return;
        }

        // Сбрасываем горение носилок
        IsBurns = false;
        // Останавливаем уничтожение
        StopAllCoroutines();
    }

    /// <summary>
    /// Отсчет до уничтожения носилок от огня
    /// </summary>
    private IEnumerator DestroyStretcher()
    {
        yield return new WaitForSeconds(5.5f);

        // Скрываем огонь на носилках
        LightVisibility(false);

        // Переключаем анимацию носилок на уничтожение
        ChangeAnimation((int)State.Destroy);

        // Вызываем событие о сгоревших носилках
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