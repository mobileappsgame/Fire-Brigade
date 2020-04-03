using UnityEngine;

public class FireRoad : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Получаем компонент носилок у коснувшегося объекта
        var stretcher = collision.gameObject.GetComponent<Stretcher>();

        if (stretcher)
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