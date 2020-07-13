using UnityEngine;
using Cubra.Heplers;

namespace Cubra.Levels
{
    public class Drop : MonoBehaviour, IPoolable
    {
        [Header("Эффект огненных брызг")]
        [SerializeField] private ParticleSystem _spray;

        // Скорость падения
        private float _speed;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Активация объекта из пула
        /// </summary>
        public void ActivateObject()
        {
            gameObject.SetActive(true);

            // Определяем скорость падения
            _speed = Random.Range(5.0f, 6.5f);
        }

        private void FixedUpdate()
        {
            // Перемещаем каплю вниз с указанной скоростью и коэффициентом замедления
            _rigidbody.MovePosition(_rigidbody.position + (Vector2.down * (_speed * Slowdown.Coefficient) * Time.fixedDeltaTime));
        }

        /// <summary>
        /// Отображение эффекта огненных брызг от капли
        /// </summary>
        public void ShowSplashEffect()
        {
            // Перемещаем эффект брызг к огненной капле
            _spray.transform.position = transform.position;
            _spray.Play();
        }

        /// <summary>
        /// Деактивация объекта при возвращении в пул
        /// </summary>
        public void DeactivateObject()
        {
            gameObject.SetActive(false);
        }
    }
}