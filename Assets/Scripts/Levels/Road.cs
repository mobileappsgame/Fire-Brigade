using UnityEngine;
using Cubra.Heplers;

namespace Cubra.Levels
{
    public class Road : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Controllers.CharacterController character))
            {
                // Активируем переменную нахождения на земле
                character.IsGroung = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Drop drop))
            {
                // Отображаем эффект брызг
                drop.ShowSplashEffect();

                // Возвращаем каплю в указанный пул объектов
                PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);
            }
        }
    }
}