using UnityEngine;

namespace Cubra
{
    public class ExitGame : MonoBehaviour
    {
        [Header("Панель выхода")]
        [SerializeField] private GameObject _exitPanel;

        private void Update()
        {
            // Если нажата кнопка возврата и панель выхода неактивна
            if (Input.GetKey(KeyCode.Escape) && _exitPanel.activeSelf == false)
            {
                _exitPanel.SetActive(true);
            }
        }
    }
}