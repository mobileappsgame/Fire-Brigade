using UnityEngine;

namespace Cubra.Levels
{
    public class FireRoad : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Stretcher stretcher))
            {
                // Если носилки не улучшенные
                if (stretcher.IsSuper == false)
                {
                    // Поджигаем носилки
                    stretcher.SetFireStretcher(stretcher.IsBurns);
                }
            }
        }
    }
}