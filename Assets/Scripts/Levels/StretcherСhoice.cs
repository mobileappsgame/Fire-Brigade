using UnityEngine;
using UnityEngine.EventSystems;

public class StretcherСhoice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Нажатие на кнопку выбора носилок
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Уменьшаем масштаб кнопки
        rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    /// <summary>
    /// Отпускание кнопки выбора носилок
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        // Восстанавливаем масштаб кнопки
        rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
}