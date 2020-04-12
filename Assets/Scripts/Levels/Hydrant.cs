using System.Collections;
using UnityEngine;

public class Hydrant : MonoBehaviour
{
    [Header("Эффект воды")]
    [SerializeField] private ParticleSystem water;

    [Header("Эффект брызг")]
    [SerializeField] private Transform spray;

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
            _ = StartCoroutine(ActiveSnuffOut());
        }
    }

    /// <summary>
    /// Активация объекта тушения огня
    /// </summary>
    private IEnumerator ActiveSnuffOut()
    {
        yield return new WaitForSeconds(0.7f);
        snuffOut.SetActive(true);

        // Перемещаем брызги к объекту тушения
        spray.localPosition = new Vector2(-1.16f, -1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Control>())
        {
            // Восстанавливаем напор воды
            mainModule.startLifetime = 0.3f;
            snuffOut.SetActive(false);

            // Возвращаем брызги в стандартную позицию
            spray.localPosition = new Vector2(-0.814f, -0.458f);
        }  
    }
}