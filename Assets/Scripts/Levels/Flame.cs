using UnityEngine;

namespace Cubra.Levels
{
    public class Flame : MonoBehaviour
    {
        private Flames _flames;

        private void Start()
        {
            _flames = GetComponentInParent<Flames>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Если огонек с носилок касается воды из гидранта
            if (collision.transform.parent.GetComponent<Hydrant>())
            {
                gameObject.SetActive(false);
                
                // Проверяем количество огней
                _flames.CheckQuantityFlames();
            }
        }
    }
}