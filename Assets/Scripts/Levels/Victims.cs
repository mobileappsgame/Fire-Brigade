using System.Collections;
using UnityEngine;

public class Victims : MonoBehaviour, IPoolable
{
    // Падает ли персонаж
    private bool isFall = false;
    // Скорость падения персонажа
    private float speed;

    // Находится ли персонаж на земле
    private bool isGround = false;

    // Слой носилок на сцене
    private int layer;

    // Уровень носилок
    private int stretcherLevel;

    [Header("Цветовой статус")]
    [SerializeField] private string status;

    [Header("Пул объекта")] // ключ пула
    [SerializeField] private string pool;

    [Header("Вес персонажа")]
    [SerializeField] private int weight;

    [Header("Задержка до прыжка")]
    [SerializeField] private float delay;

    [Header("Смещение в окне")]
    [SerializeField] private float offset = 0;

    public float Offset { get { return offset; } }

    // Окно персонажа для прыжка
    public Window Window { get; set; }

    // Ссылки на компоненты
    private Rigidbody2D rigbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake()
    {
        rigbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // Получаем номер слоя носилок
        layer = LayerMask.GetMask("Stretcher");

        // Получаем уровень носилок
        stretcherLevel = PlayerPrefs.GetInt("stretcher");
    }

    /// <summary>
    /// Активация объекта из пула
    /// </summary>
    public void ActivateObject()
    {
        // Активируем объект
        gameObject.SetActive(true);

        // Определяем случайную скорость падения
        speed = Random.Range(4.3f, 5.2f);

        // Определяем задержку до прыжка
        delay += Random.Range(-1.0f, 1.5f);
        // Запускаем отсчет до прыжка из окна
        StartCoroutine(CountdownToJump());
    }

    /// <summary>
    /// Отсчет до прыжка персонажа из окна
    /// </summary>
    private IEnumerator CountdownToJump()
    {
        while (delay > 2)
        {
            yield return new WaitForSeconds(0.1f);
            delay -= 0.1f;
        }

        // Отображаем предупреждение о прыжке
        Window.ShowWarning(true);

        yield return new WaitForSeconds(delay);
        // Скрываем предупреждение о прыжке
        Window.ShowWarning(false);
        // Активируем анимацию прыжка
        animator.SetTrigger("Fall");
    }

    private void FixedUpdate()
    {
        if (isFall)
        {
            // Перемещаем персонажа вниз с указанной скоростью и коэффициентом замедления
            rigbody.MovePosition(rigbody.position + Vector2.down * (speed * Slowdown.coefficient) * Time.fixedDeltaTime);

            // Пускаем луч вниз от персонажа
            RaycastHit2D hit = Physics2D.Raycast(DefinePoint(), Vector2.down, 0.05f, layer);

            // Если найдены носилки и они не горят
            if (hit.collider && Stretcher.IsBurns != true)
            {
                // Отключаем падение
                isFall = false;

                // Создаем небольшой отскок от носилок
                rigbody.AddForce(Vector2.up * 16, ForceMode2D.Impulse);
                // Активируем триггер спасения
                animator.SetTrigger("Save");

                // Если цветовые статусы не совпадают и используются обычные носилки
                if (status != Stretcher.Status && Stretcher.IsSuper == false)
                {
                    var stretcher = hit.collider.GetComponent<Stretcher>();
                    // Уменьшаем прочность носилок
                    stretcher.ChangeStrength(-weight + (int)Mathf.Pow(stretcherLevel, 2));
                    // Проверяем прочность носилок
                    stretcher.CheckStrength();

                    // Уменьшаем количество оставшихся ошибок
                    LevelManager.quantityErrors?.Invoke(-1);

                    // Уменьшаем счет уровня
                    Score.ChangingScore?.Invoke(-weight / 3);
                }
                else
                {
                    // Увеличиваем счет уровня
                    Score.ChangingScore?.Invoke(weight / 2 + 15);
                }
            }
        }

        if (isGround)
        {
            // Даем спасенному персонажу убежать за экран
            rigbody.MovePosition(rigbody.position + Vector2.right * speed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Определяем нижнюю границу персонажа
    /// </summary>
    private Vector2 DefinePoint()
    {
        // Возвращаем точку для испускания луча
        return (Vector2)transform.position - new Vector2(0, 0.5f);
    }

    /// <summary>
    /// Активация падения персонажа
    /// </summary>
    public void ActivateFall()
    {
        isFall = true;

        // Возвращаем окно в список доступных
        StartCoroutine(Window.ReestablishWindow());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Если падающий персонаж касается дороги
        if (collision.gameObject.GetComponent<Road>())
        {
            // Если персонаж был пойман на носилки
            if (isFall == false)
            {
                // Активируем нахождение на земле
                isGround = true;
            }   
            else
            {
                // Активируем триггер смерти
                animator.SetTrigger("Dead");

                // Уменьшаем слой персонажа
                sprite.sortingOrder = 3;

                // Возвращаем персонажа в пул
                StartCoroutine(ReturnToPool());

                // Уменьшаем количество оставшихся ошибок
                LevelManager.quantityErrors?.Invoke(-1);

                // Уменьшаем счет уровня
                Score.ChangingScore?.Invoke(-weight / 3);
            }

            // Уменьшаем жильцов и проверяем количество
            LevelManager.quantityVictims?.Invoke(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если персонаж касается финишного объекта
        if (collision.gameObject.CompareTag("Finish"))
        {
            // Возвращаем персонажа в пул
            StartCoroutine(ReturnToPool());
        }
    }

    /// <summary>
    /// Возвращение персонажа в указанный пул с небольшой задержкой
    /// </summary>
    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(1.2f);
        PoolsManager.PutObjectToPool(pool, gameObject);
    }

    /// <summary>
    /// Деактивация объекта при возвращении в пул
    /// </summary>
    public void DeactivateObject()
    {
        // Сбрасываем переменные
        isFall = false;
        isGround = false;

        // Восстанавливаем слой
        sprite.sortingOrder = 7;

        // Сбрасываем анимацию
        animator.Rebind();

        // Отключаем объект
        gameObject.SetActive(false);
    }
}