using System.Collections.Generic;
using UnityEngine;

public class PoolsManager
{
    // Пул объектов (словарь ключ - список)
    private static Dictionary<string, List<GameObject>> poolsDictionary = new Dictionary<string, List<GameObject>>();

    /// <summary>
    /// Начальное заполнение пула неактивными объектами
    /// </summary>
    /// <param name="pool">Родительский объект, содержащий неактивные объекты для пула</param>
    public static void FillPool(GameObject pool)
    {
        // Ключ по названию родительского объекта
        var key = pool.transform.parent.name;

        // Если в пуле отсутствует набор объектов по указанному ключу
        if (!poolsDictionary.ContainsKey(key))
        {
            // Создаем список по указанному ключу
            poolsDictionary[key] = new List<GameObject>();

            // Заполняем список пула дочерними объектами
            for (int i = 0; i < pool.transform.childCount; i++)
                poolsDictionary[key].Add(pool.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Получение количества объектов в пуле
    /// </summary>
    /// <param name="key">Ключ пула в словаре</param>
    public static int QuantityObjects(string key)
    {
        return poolsDictionary[key].Count;
    }

    /// <summary>
    /// Получение объекта из пула
    /// </summary>
    /// <param name="key">Ключ пула в словаре</param>
    /// <param name="number">Номер объекта, который нужно получить из пула</param>
    public static GameObject GetObjectFromPool(string key, int number = 0)
    {
        // Получаем количество объектов в пуле
        var quantity = poolsDictionary[key].Count;

        // Если в пуле есть доступный объект
        if (quantity > 0 && number < quantity)
        {
            // Получаем объект и удаляем его из списка
            GameObject objectFromPool = poolsDictionary[key][number];
            poolsDictionary[key].RemoveAt(number);
            return objectFromPool;
        }

        return null;
    }

    /// <summary>
    /// Возвращение объекта в пул
    /// </summary>
    /// <param name="key">Ключ пула в словаре</param>
    /// <param name="target">Объект для помещения в пул</param>
    public static void PutObjectToPool(string key, GameObject target)
    {
        // Добавляем объект в словарь
        poolsDictionary[key].Add(target);
        // Вызываем деактивацию объекта
        target.GetComponent<IPoolable>().DeactivateObject();
    }
}