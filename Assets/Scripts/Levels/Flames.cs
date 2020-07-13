using System;
using UnityEngine;

namespace Cubra.Levels
{
    public class Flames : MonoBehaviour
    {
        // Событие по тушению носилок
        public event Action Extinguished;

        // Огоньки на носилках
        private GameObject[] _flames;

        private void Start()
        {
            _flames = new GameObject[transform.childCount];

            // Заполняем массив дочерними объектами
            for (int i = 0; i < transform.childCount; i++)
                _flames[i] = transform.GetChild(i).gameObject;
        }

        /// <summary>
        /// Установка видимости огоньков на носилках
        /// </summary>
        /// <param name="state">видимость</param>
        public void FlameVisibility(bool state)
        {
            for (int i = 0; i < _flames.Length; i++)
                _flames[i].SetActive(state);
        }

        /// <summary>
        /// Проверка оставшихся огоньков на носилках
        /// </summary>
        public void CheckQuantityFlames()
        {
            for (int i = 0; i < _flames.Length; i++)
            {
                // Если есть огоньки, выходим из метода
                if (_flames[i].activeInHierarchy) return;
            }

            Extinguished?.Invoke();
        }

        private void OnDestroy()
        {
            Extinguished = null;
        }
    }
}