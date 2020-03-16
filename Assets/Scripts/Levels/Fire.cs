using UnityEngine;

public class Fire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огонек с носилок касается воды из гидранта
        if (collision.transform.parent.GetComponent<Hydrant>())
        {
            // Скрываем огонек
            gameObject.SetActive(false);

            // Проверяем оставшиеся огоньки
            Stretcher.SnuffOut?.Invoke();
        }
    }
}