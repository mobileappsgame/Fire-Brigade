using UnityEngine;

public class Flames : MonoBehaviour
{
    public delegate void Extinguishing();
    // Событие по тушению носилок
    public event Extinguishing Extinguished;

    // Огоньки на носилках
    private GameObject[] flames;

    private void Start()
    {
        flames = new GameObject[transform.childCount];

        // Заполняем массив дочерними объектами
        for (int i = 0; i < transform.childCount; i++)
            flames[i] = transform.GetChild(i).gameObject;
    }

    /// <summary>
    /// Установка видимости огоньков на носилках
    /// </summary>
    /// <param name="state">видимость</param>
    public void FlameVisibility(bool state)
    {
        for (int i = 0; i < flames.Length; i++)
            flames[i].SetActive(state);
    }

    /// <summary>
    /// Проверка оставшихся огоньков на носилках
    /// </summary>
    public void CheckQuantityFlames()
    {
        for (int i = 0; i < flames.Length; i++)
        {
            // Если есть огоньки, выходим из метода
            if (flames[i].activeInHierarchy) return;
        }

        Extinguished?.Invoke();
    }

    private void OnDestroy()
    {
        // Очищаем подписчиков
        Extinguished = null;
    }
}