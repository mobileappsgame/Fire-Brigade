using System.Collections;
using UnityEngine;
using Cubra.Heplers;

namespace Cubra.Levels
{
    public class Victims : MonoBehaviour, IPoolable
    {
        // Падает ли персонаж
        protected bool _isFall;
        // Скорость падения
        protected float _speed;

        // Находится ли персонаж на земле
        protected bool _isGround;

        [Header("Цветовой статус")]
        [SerializeField] protected ColorStatus.Statuses _status;

        [Header("Пул объекта")]
        [SerializeField] protected ListingPools.Pools _pool;

        [Header("Вес персонажа")]
        [SerializeField] protected int _weight;

        [Header("Задержка до прыжка")]
        [SerializeField] protected float _delay;

        [Header("Смещение в окне")]
        [SerializeField] protected float _offset;

        public float Offset => _offset;

        // Окно, из которого прыгает персонаж
        public Window Window { get; set; }

        protected Rigidbody2D _rigidbody;
        protected Animator _animator;
        protected SpriteRenderer _sprite;

        protected PlayingSound _playingSound;
        protected LevelManager _levelManager;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();

            _playingSound = transform.parent.GetComponentInParent<PlayingSound>();
            _levelManager = Camera.main.GetComponent<LevelManager>();
        }

        /// <summary>
        /// Активация объекта из пула
        /// </summary>
        public void ActivateObject()
        {
            gameObject.SetActive(true);

            // Определяем скорость падения
            _speed = Random.Range(4.3f, 5.2f);

            // Определяем задержку до прыжка
            _delay += Random.Range(-1.0f, 1.5f);
            // Запускаем отсчет до прыжка
            _ = StartCoroutine(CountdownToJump());
        }

        /// <summary>
        /// Отсчет до прыжка персонажа из окна
        /// </summary>
        protected IEnumerator CountdownToJump()
        {
            while (_delay > 2)
            {
                yield return new WaitForSeconds(0.1f);
                _delay -= 0.1f;
            }

            // Отображаем предупреждение
            Window.ShowWarning(true);
            yield return new WaitForSeconds(_delay);
            // Скрываем предупреждение о прыжке
            Window.ShowWarning(false);

            // Активируем триггер прыжка
            _animator.SetTrigger("Fall");
        }

        protected virtual void FixedUpdate()
        {
            if (_isFall) VictimsFall();

            if (_isGround)
            {
                // Даем спасенному персонажу убежать за экран
                _rigidbody.MovePosition(_rigidbody.position + Vector2.right * _speed * Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Активация падения персонажа
        /// </summary>
        public virtual void ActivateFall()
        {
            _isFall = true;
            // Возвращаем текущее окно в список доступных
            _ = StartCoroutine(Window.ReestablishWindow());
        }

        /// <summary>
        /// Падение персонажа
        /// </summary>
        protected void VictimsFall()
        {
            // Перемещаем персонажа вниз с указанной скоростью и коэффициентом замедления
            _rigidbody.MovePosition(_rigidbody.position + Vector2.down * (_speed * Slowdown.Coefficient) * Time.fixedDeltaTime);

            // Пускаем луч вниз от персонажа
            RaycastHit2D hit = Physics2D.Raycast(DefinePoint(), Vector2.down, 0.05f, LayerMask.GetMask("Stretcher"));

            if (hit.collider)
            {
                if (hit.collider.gameObject.TryGetComponent(out Stretcher stretcher))
                {
                    // Если носилки не горят
                    if (stretcher.IsBurns == false)
                    {
                        _isFall = false;

                        // Создаем небольшой отскок от носилок
                        _rigidbody.AddForce(new Vector2(0.5f, 1.0f) * 3, ForceMode2D.Impulse);
                        // Воспроизводим звук
                        stretcher.PlayingSound.PlaySound();

                        // Увеличиваем число спасенных
                        _levelManager.SavedVictims++;

                        // Сравниваем статусы
                        CompareStatuses(stretcher);
                    }
                }
            }
        }

        /// <summary>
        /// Определяем нижнюю границу персонажа
        /// </summary>
        protected Vector2 DefinePoint()
        {
            // Возвращаем точку для испускания луча
            return (Vector2)transform.position - new Vector2(0, 0.5f);
        }

        /// <summary>
        /// Сравнивание статусов персонажа и носилок
        /// </summary>
        /// <param name="stretcher">носилки</param>
        protected void CompareStatuses(Stretcher stretcher)
        {
            // Если цветовые статусы не совпадают и используются обычные носилки
            if (_status != stretcher.Status && stretcher.IsSuper == false)
            {
                // Уменьшаем прочность носилок (с учетом уровня носилок)
                stretcher.ChangeStrength(_weight * (-1) + (int)Mathf.Pow(stretcher.StretcherLevel, 2));
                // Проверяем прочность
                stretcher.CheckStrength();

                // Немного увеличиваем счет уровня
                _levelManager.ChangeScore(_weight / 10);
            }
            else
            {
                // Увеличиваем счет уровня с бонусом
                _levelManager.ChangeScore(_weight / 2 + 15);
            }
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            // Если падающий персонаж касается дороги
            if (collision.gameObject.GetComponent<Road>())
            {
                // Если персонаж был пойман
                if (_isFall == false)
                {
                    _isGround = true;
                    _sprite.flipX = false;

                    // Активируем триггер спасения
                    _animator.SetTrigger("Save");
                }
                else
                {
                    // Активируем триггер смерти
                    _animator.SetTrigger("Dead");
                    // Проигрываем звук смерти
                    _playingSound.PlaySound();

                    // Возвращаем персонажа в пул
                    _ = StartCoroutine(ReturnToPool());

                    // Записываем совершенную ошибку
                    _levelManager.ChangeErrors(1);
                    // Уменьшаем счет уровня
                    _levelManager.ChangeScore(_weight * (-1) / 3);
                }

                // Понижаем слой персонажа
                _sprite.sortingOrder = 3;

                // Уменьшаем количество жильцов
                _levelManager.ReduceQuantityVictims();
            }
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            // Если персонаж касается финишного объекта
            if (collision.gameObject.CompareTag("Finish"))
            {
                // Возвращаем персонажа в пул
                _ = StartCoroutine(ReturnToPool());
            }
        }

        /// <summary>
        /// Возвращение персонажа в указанный пул
        /// </summary>
        protected IEnumerator ReturnToPool()
        {
            yield return new WaitForSeconds(1.2f);
            PoolsManager.PutObjectToPool(_pool.ToString(), gameObject);
        }

        /// <summary>
        /// Деактивация объекта при возвращении в пул
        /// </summary>
        public virtual void DeactivateObject()
        {
            // Сброс значений
            _isFall = false;
            _isGround = false;
            _sprite.sortingOrder = 7;

            _animator.Rebind();
            gameObject.SetActive(false);
        }
    }
}