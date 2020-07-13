using System.Collections;
using UnityEngine;

namespace Cubra.Levels
{
    public class Slowdown : MonoBehaviour
    {
        // Коэффициент замедления падения
        public static float Coefficient;

        private Coroutine _coroutine;

        private void Start()
        {
            Coefficient = 1;
        }

        /// <summary>
        /// Изменение коэффициента замедления
        /// </summary>
        /// <param name="activity">активность замедления</param>
        public void ChangeSlowdown(bool activity)
        {
            if (activity)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                Coefficient = 0.45f;
            }
            else
            {
                // Запускаем восстановление коэффициента
                _coroutine = StartCoroutine(IncreaseCoefficient());
            }
        }

        /// <summary>
        /// Постепенное восстановление коэффициента падения
        /// </summary>
        private IEnumerator IncreaseCoefficient()
        {
            while (Coefficient < 1)
            {
                yield return new WaitForSeconds(0.05f);
                Coefficient += 0.1f;
            }
        }
    }
}