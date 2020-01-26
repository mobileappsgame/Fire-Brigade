using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [Header("Пожар при старте")]
    [SerializeField] private bool atStart = false;

    // Был ли пожар в текущем окне
    private bool repeatedFire = false;

    // Текущая активность пожара
    private bool burns = false;

    // Эффект огня
    private GameObject fire;

    // Ссылка на анимацию
    private Animator animator;

    private void Awake()
    {
        fire = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (atStart)
        {
            burns = true;
            repeatedFire = true;
            ActivateFire();
        }
    }

    /// <summary>
    /// Запуск пожара в окне
    /// </summary>
    public void ActivateFire()
    {
        if (repeatedFire)
        {
            // Отображаем открытое окно
            animator.Play("Fire");
        }
        else
        {
            burns = true;
            repeatedFire = true;

            // Запускаем постепенную анимацию
            animator.SetBool("Fire", true);
        }
    }

    /// <summary>
    /// Отображение огненного эффекта
    /// </summary>
    public void ShowFireEffect()
    {
        fire.SetActive(true);
    }
}