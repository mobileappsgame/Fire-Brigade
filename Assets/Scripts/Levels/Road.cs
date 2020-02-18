using UnityEngine;

public class Road : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Если персонажи касаются дороги
        if (collision.gameObject.GetComponent<Control>())
        {
            // Активируем переменную нахождения на земле
            collision.gameObject.GetComponent<Control>().IsGroung = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огненная капля касается дороги
        if (collision.gameObject.GetComponent<Drop>())
        {
            // Отображаем эффект огненных брызг
            collision.gameObject.GetComponent<Drop>().ShowSplashEffect();

            // Возвращаем каплю в указанный пул объектов
            PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);
        }
    }
}