using System.Collections;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    [Header("Максимум открытых окон")]
    [SerializeField] private int maximum;

    private void Start()
    {
        // Определяем количество окон с пожаром при старте
        var amount = Random.Range(2, maximum + 1);

        for (int i = 0; i < amount; i++)
        {
            // Определяем номер случайного окна (из всех окон в пуле)
            var number = Random.Range(0, PoolsManager.QuantityObjects(ListingPools.Pools.Windows.ToString()));

            // Получаем объект из пула и получаем его компонент
            var window = PoolsManager.GetObjectFromPool(ListingPools.Pools.Windows.ToString(), number).GetComponent<Window>();

            // Открываем выбранное окно
            window.OpenWindow = true;
            // Активируем объект
            window.ActivateObject();
        }

        StartCoroutine(OpenWindows());
    }

    /// <summary>
    /// Переодическое открытие окон (появление пожара)
    /// </summary>
    private IEnumerator OpenWindows()
    {
        while (PoolsManager.QuantityObjects(ListingPools.Pools.Windows.ToString()) > 0)
        {
            var seconds = Random.Range(5, 12);
            yield return new WaitForSeconds(seconds);

            // Определяем номер случайного окна (из всех окон в пуле)
            var number = Random.Range(0, PoolsManager.QuantityObjects(gameObject.name));

            // Получаем объект из пула и получаем его компонент
            var window = PoolsManager.GetObjectFromPool(ListingPools.Pools.Windows.ToString(), number).GetComponent<Window>();
            // Активируем объект
            window.ActivateObject();
        }
    }
}