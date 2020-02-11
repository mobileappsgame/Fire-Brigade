using UnityEngine;

public class Road : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Если на объекте есть компонент управления
        if (collision.gameObject.GetComponent<Control>())
        {
            // Указываем, что персонажи находятся на земле
            collision.gameObject.GetComponent<Control>().IsGroung = true;
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если на объекте есть компонент капли
        if (collision.gameObject.GetComponent<Drop>())
        {
            // Отображаем эффект огненных брызг
            collision.gameObject.GetComponent<Drop>().ShowSplashEffect();

            // Возвращаем объект в нужный пул
            PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);
            return;
        }
    }
}