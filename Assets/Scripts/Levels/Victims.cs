using System.Collections;
using UnityEngine;

public class Victims : MonoBehaviour, IPoolable
{
    // Падает ли персонаж
    private bool isFall = false;

    // Находится ли персонаж на земле
    private bool isGround = false;

    // Скорость падения персонажа
    private float speed;

    // Слой носилок на сцене
    private int layer;

    // Окно, из которого прыгает персонаж
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
    }

    private void FixedUpdate()
    {
        if (isFall)
        {
            // Перемещаем персонажа вниз с указанной скоростью (и коэффициентом замедления)
            rigbody.MovePosition(rigbody.position + Vector2.down * (speed * Slowdown.coefficient) * Time.fixedDeltaTime);

            // Нижняя граница персонажа (точка для испускания луча)
            Vector2 position = (Vector2)transform.position - new Vector2(0, 0.5f);

            // Пускаем луч вниз от персонажа
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 0.05f, layer);

            // Если найдены носилки и они не горят
            if (hit.collider && Stretcher.IsBurns != true)
            {
                isFall = false;

                // Создаем небольшой отскок от носилок
                rigbody.AddForce(Vector2.up * 15, ForceMode2D.Impulse);

                // Активируем триггер спасения
                animator.SetTrigger("Save");
            }
        }

        if (isGround)
        {
            // Даем спасенному персонажу убежать за экран
            rigbody.MovePosition(rigbody.position + Vector2.right * speed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Активация переменной падения
    /// </summary>
    public void ActivateFall()
    {
        isFall = true;
    }

    /// <summary>
    /// Возвращение окна в список доступных для персонажей
    /// </summary>
    public void ReestablishWindow()
    {
        // Отправляем в список
        Window.AddToList();
        // Восстанавливаем огненные капли
        Window.Twinkle = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Если падающий персонаж касается дороги
        if (collision.gameObject.GetComponent<Road>())
        {
            // Если персонаж был пойман на носилки
            if (isFall == false)
            {
                // Активируем переменную
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
            }
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
        PoolsManager.PutObjectToPool(ListingPools.Pools.GreenMen.ToString(), gameObject);
    }

    /// <summary>
    /// Деактивация объекта при возвращении в пул
    /// </summary>
    public void DeactivateObject()
    {
        // Сбрасываем переменные
        isFall = false;
        isGround = false;

        // Восстанавливаем слой объекта
        sprite.sortingOrder = 7;

        // Сбрасываем анимацию
        animator.Rebind();

        // Отключаем объект
        gameObject.SetActive(false);
    }
}