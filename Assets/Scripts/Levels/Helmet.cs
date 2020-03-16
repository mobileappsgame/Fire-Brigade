using UnityEngine;

public class Helmet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент капли у коснувшегося объекта
        var drop = collision.gameObject.GetComponent<Drop>();

        if (drop)
        {
            // Отображаем эффект брызг
            drop.ShowSplashEffect();

            // Возвращаем каплю в указанный пул объектов
            PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);
        }
    }
}