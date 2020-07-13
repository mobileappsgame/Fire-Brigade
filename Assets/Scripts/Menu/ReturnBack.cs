using UnityEngine;

namespace Cubra
{
    public class ReturnBack : MonoBehaviour
    {
        [Header("Сцена для возврата")]
        [SerializeField] private TransitionsManager.Scenes _scene;

        private TransitionsManager _transitionsManager;

        private void Awake()
        {
            _transitionsManager = Camera.main.GetComponent<TransitionsManager>();
        }

        private void Update()
        {
            // Если нажата кнопка возврата
            if (Input.GetKey(KeyCode.Escape))
                // Выполняем переход на сцену
                _transitionsManager.GoToScene((int)_scene);
        }
    }
}