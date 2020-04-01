using UnityEngine;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// Нажатие на кнопку выбора носилок
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Уменьшаем масштаб кнопки
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    /// <summary>
    /// Отпускание кнопки выбора носилок
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        // Восстанавливаем масштаб кнопки
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
}