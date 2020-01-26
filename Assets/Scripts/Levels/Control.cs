using UnityEngine;

public class Control : MonoBehaviour
{
    // Активность управления персонажами
    public bool ActiveControl { get; private set; } = false;

    [Header("Инвертирование управления")]
    [SerializeField] private bool inverted = false;

    // Инвертирование управления
    private int Inverted
    {
        get { return inverted ? -1 : 1; }
    }

    // Скорость движения персонажей
    private float speed = 10f;

    // Направление движения персонажей
    public Vector2 Direction { get; private set; }

    // Анимация персонажей
    private Animator brigade;

    private void Awake()
    {
        brigade = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        // Сбрасываем вектор движения
        Direction = Vector2.zero;

        if (ActiveControl)
        {
            // Проверяем позицию
            CheckPosition();

            // Перемещаем персонажей в указанном направлении
            transform.Translate(Direction * speed * Time.deltaTime);
        }

        // Если направление нулевое (с погрешностью)
        if (Direction.x < 0.015f && Direction.x > -0.015f)
        {
            // Устанавливаем стандартную анимацию
            brigade.SetInteger("State", (int)Characters.Animations.Idle);
        }
        else
        {
            // Иначе устанавливаем анимацию бега
            brigade.SetInteger("State", (int)Characters.Animations.Run);
        } 
    }

    /// <summary>
    /// Проверка персонажей на выход за пределы игровой зоны
    /// </summary>
    private void CheckPosition()
    {
        // Если позиция персонажей выходит за границы игрового поля
        if ((transform.position.x < -8f && Input.acceleration.x < 0) ||
                (transform.position.x > 6.2f && Input.acceleration.x > 0))
        {
            // Сбрасываем направление
            Direction *= 0;
        }
        else
        {
            // Устанавливаем направление в зависимости от наклона смартфона
            Direction = new Vector2(Input.acceleration.x * Inverted, 0);
        }

        // Если длина вектора превышает единицу
        if (Direction.sqrMagnitude > 1)
        {
            // Округляем до единицы
            Direction.Normalize();
        }
    }

    /// <summary>
    /// Изменение активности управления
    /// </summary>
    /// <param name="state">Обновленное состояние</param>
    public void ChangeControl(bool state)
    {
        ActiveControl = state;
    }
}