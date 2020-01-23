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

    // Направление движения
    private Vector2 direction;

    private void Update()
    {
        // Сбрасываем вектор движения
        direction = Vector2.zero;

        if (ActiveControl)
        {
            // Проверяем позицию
            CheckPosition();

            // Перемещаем персонажей в указанном направлении
            transform.Translate(direction * speed * Time.deltaTime);
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
            direction.x *= 0;
        }
        else
        {
            // Устанавливаем направление в зависимости от наклона смартфона
            direction.x = Input.acceleration.x * Inverted;
        }

        // Если длина вектора превышает единицу
        if (direction.sqrMagnitude > 1)
        {
            // Округляем до единицы
            direction.Normalize();
        }
    }

    /// <summary>
    /// Изменение активности управления
    /// </summary>
    /// <param name="state">Состояние</param>
    public void ChangeControl(bool state)
    {
        ActiveControl = state;
    }
}