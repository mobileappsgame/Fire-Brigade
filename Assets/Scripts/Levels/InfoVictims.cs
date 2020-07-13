using UnityEngine;
using UnityEngine.UI;

namespace Cubra.Levels
{
    public class InfoVictims : MonoBehaviour
    {
        private Text _quantity;
        private LevelManager _levelManager;

        private void Awake()
        {
            _quantity = GetComponent<Text>();

            _levelManager = Camera.main.GetComponent<LevelManager>();
            _levelManager.VictimsChanged += ShowQuantity;
        }

        private void Start()
        {
            ShowQuantity();
        }

        /// <summary>
        /// Отображение количества оставшихся персонажей
        /// </summary>
        private void ShowQuantity()
        {
            _quantity.text = _levelManager.CurrentVictims.ToString();
        }
    }
}