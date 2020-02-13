using UnityEngine;

public class Lights : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огонек касается объекта тушения
        if (collision.transform.parent.GetComponent<Hydrant>())
        {
            // Скрываем объект
            gameObject.SetActive(false);

            // Вызываем проверку количества огоньков
            Stretcher.SnuffOut?.Invoke();
        }
    }
}