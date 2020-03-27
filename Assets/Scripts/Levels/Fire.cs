using UnityEngine;

public class Fire : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огонек с носилок касается воды из гидранта
        if (collision.transform.parent.GetComponent<Hydrant>())
        {
            gameObject.SetActive(false);

            // Проверяем количество оставшихся огоньков
            Stretcher.SnuffOut?.Invoke();
        }
    }
}