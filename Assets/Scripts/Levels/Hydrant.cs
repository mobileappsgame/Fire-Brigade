using UnityEngine;

public class Hydrant : MonoBehaviour
{
    [Header("Эффект воды")]
    [SerializeField] private ParticleSystem water;

    [Header("Объект тушения")]
    [SerializeField] private GameObject snuffOut;

    // Ссылка на компонент частиц
    private ParticleSystem.MainModule mainModule;

    private void Awake()
    {
        mainModule = water.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если персонаж касается гидранта
        if (collision.gameObject.GetComponent<Control>())
        {
            // Увеличиваем напор воды
            mainModule.startLifetime = 0.54f;
            // Активируем объект тушения
            Invoke("ActiveSnuffOut", 0.5f);
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Если персонаж больше не касается гидранта
        if (collision.gameObject.GetComponent<Control>())
        {
            // Восстанавливаем стандартный напор воды
            mainModule.startLifetime = 0.3f;
            // Отключаем объект тушения
            snuffOut.SetActive(false);
        }  
    }

    /// <summary>
    ///  Активация объекта тушения огня
    /// </summary>
    private void ActiveSnuffOut()
    {
        snuffOut.SetActive(true);
    }
}