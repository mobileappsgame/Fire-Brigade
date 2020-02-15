using UnityEngine;
using UnityEngine.EventSystems;

public class Control : MonoBehaviour, IPointerDownHandler
{
    [Header("Инвертирование управления")]
    [SerializeField] private bool inverted = false;

    // Инвертирование управления
    private int Inverted { get { return inverted ? -1 : 1; } }

    // Скорость движения персонажей
    private float speed = 19.5f;

    // Высота прыжка персонажей
    private float jump = 5f;

    // Находится ли персонажи на земле
    public bool IsGroung { get; set; }

    // Направление движения персонажей
    public Vector2 Direction { get; private set; }

    // Ограничители для персонажей
    private float[] limiters = new float[2];

    // Список ограничителей
    private enum Limiters { Left, Right }

    // Ссылки на компоненты
    private Animator brigade;
    private Rigidbody2D rigdbody;

    private void Awake()
    {
        brigade = GetComponent<Animator>();
        rigdbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Получаем границы экрана и устанавливаем ограничители для персонажей
        limiters[(int)Limiters.Left] = Camera.main.ViewportToWorldPoint(new Vector2(0.05f, 0)).x;
        limiters[(int)Limiters.Right] = Camera.main.ViewportToWorldPoint(new Vector2(0.87f, 0)).x;
    }

    private void Update()
    {
        CheckPosition();
        MoveCharacters();
    }

    /// <summary>
    /// Проверка позиции персонажей и установка вектора движения
    /// </summary>
    private void CheckPosition()
    {
        // Если позиция персонажей выходит за ограничители
        if ((transform.position.x < limiters[(int)Limiters.Left] && Input.acceleration.x < 0) ||
                (transform.position.x > limiters[(int)Limiters.Right] && Input.acceleration.x > 0))
        {
            // Обнуляем направление
            Direction *= 0;
        }
        else
        {
            // Иначе устанавливаем направление в зависимости от наклона смартфона
            Direction = new Vector2(Input.acceleration.x * Inverted, 0);
        }

        // Если длина вектора превышает единицу, округляем
        if (Direction.sqrMagnitude > 1) Direction.Normalize();
    }

    /// <summary>
    /// Перемещение персонажей и установка анимации
    /// </summary>
    private void MoveCharacters()
    {
        // Если направление нулевое (с погрешностью)
        if (Direction.x < 0.015f && Direction.x > -0.015f)
        {
            // Устанавливаем стандартную анимацию
            brigade.SetInteger("State", (int)Characters.Animations.Idle);
        }
        else
        {
            // Иначе устанавливаем анимацию бега персонажей
            brigade.SetInteger("State", (int)Characters.Animations.Run);

            // Перемещаем персонажей в указанном направлении
            transform.Translate(Direction * speed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Прыжок персонажей
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsGroung)
        {
            // Создаем импульсный прыжок персонажей
            rigdbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            // Отключаем нахождение на земле
            IsGroung = false;
        }
    }
}