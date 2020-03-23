using UnityEngine;

public class Control : MonoBehaviour
{
    [Header("Инвертирование управления")]
    [SerializeField] private bool inverted = false;

    private int Inverted { get { return inverted ? -1 : 1; } }

    // Скорость движения персонажей
    private float speed = 18.5f;

    // Высота прыжка персонажей
    private float jump = 5f;

    // Находится ли персонажи на земле
    public bool IsGroung { get; set; } = false;

    // Направление движения персонажей
    public Vector2 Direction { get; private set; }

    // Ограничители для персонажей
    private float[] limiters = new float[2];

    // Перечисление ограничителей
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

    private void FixedUpdate()
    {
        SetMotionVector();
        MoveCharacters();
    }

    /// <summary>
    /// Установка вектора движения персонажей
    /// </summary>
    private void SetMotionVector()
    {
        // Если позиция персонажей выходит за ограничители
        if ((transform.position.x < limiters[(int)Limiters.Left] && Input.acceleration.x < 0) ||
                (transform.position.x > limiters[(int)Limiters.Right] && Input.acceleration.x > 0))
        {
            Direction *= 0;
        }
        else
        {
            // Устанавливаем направление в зависимости от наклона смартфона
            Direction = new Vector2(Input.acceleration.x * Inverted, 0);
        }

        // Если длина вектора превышает единицу, округляем
        if (Direction.sqrMagnitude > 1) Direction.Normalize();
    }

    /// <summary>
    /// Перемещение персонажей
    /// </summary>
    private void MoveCharacters()
    {
        // Если направление нулевое (с небольшой погрешностью)
        if (Direction.x < 0.015f && Direction.x > -0.015f)
        {
            // Устанавливаем стандартную анимацию
            ChangeAnimation(Characters.Animations.Idle);
        }
        else
        {
            // Перемещаем персонажей в указанном направлении с указанной скоростью
            rigdbody.transform.Translate(Direction * speed * Time.fixedDeltaTime);

            // Устанавливаем анимацию бега персонажей
            ChangeAnimation(Characters.Animations.Run);
        }
    }

    /// <summary>
    /// Прыжок персонажей
    /// </summary>
    private void OnMouseDown()
    {
        if (IsGroung)
        {
            // Создаем импульсный прыжок персонажей
            rigdbody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);

            // Отключаем нахождение на земле
            IsGroung = false;
        }
    }

    /// <summary>
    /// Переключение анимации персонажей
    /// </summary>
    /// <param name="animation">Анимация персонажа</param>
    private void ChangeAnimation(Characters.Animations animation)
    {
        brigade.SetInteger("State", (int)animation);
    }

    /// <summary>
    /// Переключение анимации персонажей
    /// </summary>
    /// <param name="animation">Номер анимации</param>
    public void ChangeAnimation(int animation)
    {
        brigade.SetInteger("State", animation);
    }
}