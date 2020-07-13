using UnityEngine;

namespace Cubra.Levels
{
    public class RunningVictims : Victims
    {
        // Бегает ли персонаж
        private bool _isRun;
        // Направление движения
        private int _direction;

        [Header("Скорость бега")]
        [SerializeField] private float _runningSpeed;

        [Header("Ограничители для бега")]
        [SerializeField] private Transform[] _limiters;

        // Ограничители для бега по этажу
        private float[] _limitersX;

        private void Start()
        {
            _limitersX = new float[2];
            // Записываем позицию ограничителей
            _limitersX[0] = _limiters[0].position.x;
            _limitersX[1] = _limiters[1].position.x;

            // Определяем начальное направление бега
            _direction = Random.Range(-1f, 1f) > 0 ? 1 : -1;
            _sprite.flipX = _direction <= 0;

            _isRun = true;
        }

        protected override void FixedUpdate()
        {
            if (_isRun) RunningOnFloor();

            base.FixedUpdate();
        }

        /// <summary>
        /// Бег персонажа по этажу
        /// </summary>
        private void RunningOnFloor()
        {
            // Перемещаем персонажа в указанном направлении с коэффициентом замедления
            transform.Translate(Vector3.right * _direction * _runningSpeed * Slowdown.Coefficient * Time.fixedDeltaTime);

            // Если персонаж достигает ограничителя, разворачиваем его
            if (transform.position.x < _limitersX[0] || transform.position.x > _limitersX[1])
            {
                _direction *= -1;
                _sprite.flipX = !_sprite.flipX;
            }
        }

        /// <summary>
        /// Активация падения персонажа
        /// </summary>
        public override void ActivateFall()
        {
            _isRun = false;
            base.ActivateFall();
        }

        /// <summary>
        /// Деактивация объекта при возвращении в пул
        /// </summary>
        public override void DeactivateObject()
        {
            _isRun = true;
            base.DeactivateObject();
        }
    }
}