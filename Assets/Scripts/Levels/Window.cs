using System.Collections;
using UnityEngine;

public class Window : MonoBehaviour, IPoolable
{
    // Открытость окна с пожаром
    public bool OpenWindow { get; set; } = false;

    [Header("Эффект пожара в окне")]
    [SerializeField] private GameObject fireFX;

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
        // Перемещаем взятый из пула объект в раздел активных объектов
        gameObject.transform.parent = transform.parent.parent.Find("ActiveObjects");

        // Если окно уже открыто
        if (OpenWindow)
            // Отображаем пожар сразу
            animator.Play("Fire");
        else
            // Иначе запускаем постепенный пожар
            animator.SetBool("Fire", true);
    }

    /// <summary>
    /// Отображение огненного эффекта
    /// </summary>
    public void ShowFireEffect()
    {
        fireFX.SetActive(true);

        // Запускаем создание огненных капель
        StartCoroutine(DropsFalling());
    }

    /// <summary>
    /// Создание огненных капель в горящем окне
    /// </summary>
    private IEnumerator DropsFalling()
    {
        while (OpenWindow)
        {
            yield return new WaitForSeconds(Random.Range(4f, 8.5f));
            // Получаем объект из пула и получаем его компонент
            var drop = PoolsManager.GetObjectFromPool(ListingPools.Pools.Twinkle.ToString()).GetComponent<Drop>();

            // Перемещаем каплю к горящему окну (с небольщим смещением)
            drop.transform.position = fireFX.transform.position + new Vector3(-0.15f, Random.Range(-0.2f, 0.2f), 0);

            // Активируем объект
            drop.ActivateObject();
        }
    }
}