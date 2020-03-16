using UnityEngine;

public class Road : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Получаем компонент персонажей у коснувшегося объекта
        var control = collision.gameObject.GetComponent<Control>();

        // Активируем переменную нахождения на земле
        if (control) control.IsGroung = true;
    }

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