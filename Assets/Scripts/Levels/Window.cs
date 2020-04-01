﻿using System.Collections;
using UnityEngine;

public class Window : MonoBehaviour, IPoolable
{
    // Открытость текущего окна
    public bool OpenWindow { get; set; } = false;

    // Активность создания огненных капель
    public bool Twinkle { get; set; } = false;

    [Header("Эффект пожара в окне")]
    [SerializeField] private GameObject fireFX;

    [Header("Пул персонажей")] // ключ пула
    [SerializeField] private string victims;

    [Header("Промежуток для капель")]
    [SerializeField] private float minSeconds;
    [SerializeField] private float maxSeconds;

    [Header("Предупреждение о прыжке")]
    [SerializeField] private GameObject exclamation;

    // Ссылка на компонент
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    /// <summary>
    /// Активация объекта после взятия из пула
    /// </summary>
    public void ActivateObject()
    {
        Twinkle = true;

        // Если окно изначально открыто
        if (OpenWindow)
        {
            // Отображаем пожар сразу
            animator.Play("Fire");
        }
        else
        {
            OpenWindow = true;

            // Запускаем постепенный пожар
            animator.SetTrigger("Fire");
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
        fireFX.SetActive(true);

        // Запускаем создание огненных капель
        _ = StartCoroutine(DropsFalling());
    }

    /// <summary>
    /// Создание огненных капель в горящем окне
    /// </summary>
    private IEnumerator DropsFalling()
    {
        // Если окно открыто и активен игровой режим
        while (OpenWindow && LevelManager.GameMode == "play")
        {
            yield return new WaitForSeconds(Random.Range(minSeconds, maxSeconds));

            // Вероятность падения капли
            var probability = Random.Range(1, 3);

            if (Twinkle && probability == 1)
            {
                // Получаем объект из пула и получаем его компонент
                var drop = PoolsManager.GetObjectFromPool(ListingPools.Pools.Twinkle.ToString()).GetComponent<Drop>();

                // Перемещаем каплю к горящему окну (с небольшим горизонтальным смещением)
                drop.transform.position = fireFX.transform.position + new Vector3(-0.15f, Random.Range(-0.2f, 0.2f), 0);

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
        if (PoolsManager.QuantityObjects(victims) > 0)
        {
            Twinkle = false;

            // Получаем объект персонажа из пула и получаем его компонент
            var victim = PoolsManager.GetObjectFromPool(victims).GetComponent<Victims>();

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
        exclamation.SetActive(state);
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