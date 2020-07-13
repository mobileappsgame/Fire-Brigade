using System.Collections;
using UnityEngine;

namespace Cubra.Levels
{
    public class Hydrant : MonoBehaviour
    {
        [Header("Эффект воды")]
        [SerializeField] private ParticleSystem _water;

        [Header("Эффект брызг")]
        [SerializeField] private Transform _spray;

        [Header("Объект тушения")]
        [SerializeField] private GameObject _snuffOut;

        // Ссылка на основной компонент частиц
        private ParticleSystem.MainModule _mainModule;

        private void Awake()
        {
            _mainModule = _water.main;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Controllers.CharacterController>())
            {
                // Увеличиваем напор воды
                _mainModule.startLifetime = 0.54f;

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

            _snuffOut.SetActive(true);
            // Перемещаем брызги к объекту тушения
            _spray.localPosition = new Vector2(-1.16f, -1f);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Controllers.CharacterController>())
            {
                // Восстанавливаем напор воды
                _mainModule.startLifetime = 0.3f;
                _snuffOut.SetActive(false);

                // Возвращаем брызги в стандартную позицию
                _spray.localPosition = new Vector2(-0.814f, -0.458f);
            }
        }
    }
}