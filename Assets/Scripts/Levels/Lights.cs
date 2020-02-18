using UnityEngine;

public class Lights : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огонек с носилок касается объекта тушения
        if (collision.transform.parent.GetComponent<Hydrant>())
        {
            // Скрываем текущий огонек
            gameObject.SetActive(false);

            // Проверяем количество оставшихся огоньков
            Stretcher.SnuffOut?.Invoke();
        }
    }
}