using System.Collections;
using UnityEngine;

public class Hydrant : MonoBehaviour
{
    [Header("Эффект воды")]
    [SerializeField] private ParticleSystem water;

    [Header("Объект тушения")]
    [SerializeField] private GameObject snuffOut;

    // Ссылка на основной компонент частиц
    private ParticleSystem.MainModule mainModule;

    private void Awake()
    {
        mainModule = water.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если персонажи касаются пожарного гидранта
        if (collision.gameObject.GetComponent<Control>())
        {
            // Увеличиваем напор воды
            mainModule.startLifetime = 0.54f;

            // Активируем объект тушения
            StartCoroutine(ActiveSnuffOut());
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Control>())
        {
            // Восстанавливаем стандартный напор воды
            mainModule.startLifetime = 0.3f;

            // Отключаем объект тушения
            snuffOut.SetActive(false);
        }  
    }

    /// <summary>
    /// Активация объекта тушения огня на носилках
    /// </summary>
    private IEnumerator ActiveSnuffOut()
    {
        yield return new WaitForSeconds(0.7f);
        snuffOut.SetActive(true);
    }
}