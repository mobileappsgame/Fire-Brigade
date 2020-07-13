using System.Collections;
using UnityEngine;
using Cubra.Heplers;

namespace Cubra.Levels
{
    public class Window : MonoBehaviour, IPoolable
    {
        // Открытость текущего окна
        public bool OpenWindow { get; set; }

        // Активность создания огненных капель
        public bool Twinkle { get; set; }

        [Header("Эффект пожара в окне")]
        [SerializeField] private GameObject _fireFX;

        [Header("Пул персонажей")]
        [SerializeField] private ListingPools.Pools _victims;

        [Header("Промежуток для капель")]
        [SerializeField] private float _minSeconds;
        [SerializeField] private float _maxSeconds;

        [Header("Предупреждение о прыжке")]
        [SerializeField] private GameObject _exclamation;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Активация объекта после взятия из пула
        /// </summary>
        public void ActivateObject()
        {
            Twinkle = true;

            // Если окно открыто
            if (OpenWindow)
            {
                // Отображаем пожар сразу
                _animator.Play("Fire");
            }
            else
            {
                OpenWindow = true;
                // Запускаем постепенный пожар
                _animator.SetTrigger("Fire");
            }
        }

        /// <summary>
        /// Добавление окна в список доступных для жильцов
        /// </summary>
        public void AddToList()
        {
            WindowsManager.windows.Add(this);
        }

        /// <summary>
        /// Отображение огненного эффекта
        /// </summary>
        public void ShowFireEffect()
        {
            _fireFX.SetActive(true);

            // Запускаем создание огненных капель
            _ = StartCoroutine(DropsFalling());
        }

        /// <summary>
        /// Создание огненных капель в горящем окне
        /// </summary>
        private IEnumerator DropsFalling()
        {
            // Если окно открыто и активен игровой режим
            while (OpenWindow && LevelManager.GameMode == LevelManager.GameModes.Play)
            {
                yield return new WaitForSeconds(Random.Range(_minSeconds, _maxSeconds));
                
                // Вероятность падения капли
                var probability = Random.Range(1, 3);
                
                if (Twinkle && probability == 1)
                {
                    // Получаем объект из пула и получаем его компонент
                    var drop = PoolsManager.GetObjectFromPool(ListingPools.Pools.Twinkle.ToString()).GetComponent<Drop>();
                    
                    // Перемещаем каплю к горящему окну (с небольшим горизонтальным смещением)
                    drop.transform.position = _fireFX.transform.position + new Vector3(-0.15f, Random.Range(-0.2f, 0.2f), 0);
                    
                    // Активируем объект
                    drop.ActivateObject();
                }
            }
        }

        /// <summary>
        /// Получение персонажа из пула объектов
        /// </summary>
        public void ShowVictims()
        {
            // Если в указанном пуле есть персонажи
            if (PoolsManager.QuantityObjects(_victims.ToString()) > 0)
            {
                Twinkle = false;
                
                //Получаем объект персонажа из пула и получаем его компонент
                var victim = PoolsManager.GetObjectFromPool(_victims.ToString()).GetComponent<Victims>();

                // Перемещаем персонажа в текущее окно (с указанным смещением)
                victim.transform.position = transform.position + new Vector3(0, victim.Offset, 0);

                // Записываем персонажу его окно
                victim.Window = this;

                // Активируем объект
                victim.ActivateObject();
            }
        }

        /// <summary>
        /// Отображение/скрытие предупреждения о прыжке персонажа
        /// </summary>
        /// <param name="state">видимость объекта</param>
        public void ShowWarning(bool state)
        {
            _exclamation.SetActive(state);
        }

        /// <summary>
        /// Возвращение окна в список доступных для персонажей
        /// </summary>
        public IEnumerator ReestablishWindow()
        {
            yield return new WaitForSeconds(1.7f);
            AddToList();
            Twinkle = true;
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