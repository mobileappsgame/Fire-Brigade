using UnityEngine;
using Cubra.Heplers;

namespace Cubra.Levels
{
    public class Helmet : MonoBehaviour
    {
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