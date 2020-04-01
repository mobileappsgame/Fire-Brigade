using UnityEngine;

public class Flame : MonoBehaviour
{
    // Ссылка на компонент
    private Flames flames;

    private void Start()
    {
        flames = GetComponentInParent<Flames>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Если огонек с носилок касается воды из гидранта
        if (collision.transform.parent.GetComponent<Hydrant>())
        {
            gameObject.SetActive(false);

            // Проверяем количество огней
            flames.CheckQuantityFlames();
        }
    }
}