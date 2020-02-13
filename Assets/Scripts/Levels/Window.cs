using System.Collections;
using UnityEngine;

public class Window : MonoBehaviour, IPoolable
{
    // Открыто ли окно при старте
    public bool OpenWindow { get; set; } = false;

    [Header("Эффект пожара в окне")]
    [SerializeField] private GameObject fireFX;

    // Ссылка на анимацию окна
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
        // Перемещаем взятый из пула объект в раздел активных объектов
        gameObject.transform.parent = transform.parent.parent.Find("ActiveObjects");

        if (OpenWindow)
            // Отображаем открытое окно
            animator.Play("Fire");
        else
            // Запускаем постепенную анимацию пожара
            animator.SetBool("Fire", true);
    }

    /// <summary>
    /// Отображение огненного эффекта
    /// </summary>
    public void ShowFireEffect()
    {
        fireFX.SetActive(true);
        // Создаем огненные капли
        StartCoroutine(DropsFalling());
    }

    /// <summary>
    /// Создание огненных капель в горящем окне
    /// </summary>
    private IEnumerator DropsFalling()
    {
        yield return new WaitForSeconds(Random.Range(3f, 6.5f));
        // Получаем объект из пула и получаем его компонент
        var drop = PoolsManager.GetObjectFromPool(ListingPools.Pools.Twinkle.ToString()).GetComponent<Drop>();

        // Перемещаем каплю к горящему окну (с небольщим смещением)
        drop.transform.position = fireFX.transform.position + new Vector3(-0.15f, Random.Range(-0.2f, 0.2f), 0);

        // Активируем объект
        drop.ActivateObject();
    }
}