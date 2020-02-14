using UnityEngine;

public class Helmet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если капля касается шлема персонажа
        if (collision.gameObject.GetComponent<Drop>())
        {
            // Отображаем эффект огненных брызг
            collision.gameObject.GetComponent<Drop>().ShowSplashEffect();

            // Возвращаем объект в нужный пул
            PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);
        }
    }
}