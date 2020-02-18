using UnityEngine;

public class Helmet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если капля касается шлема персонажа
        if (collision.gameObject.GetComponent<Drop>())
        {
            // Отображаем эффект огненных брызг (разлетание капли)
            collision.gameObject.GetComponent<Drop>().ShowSplashEffect();

            // Возвращаем каплю в указанный пул объектов
            PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);
        }
    }
}