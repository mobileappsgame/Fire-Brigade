using UnityEngine;

namespace Cubra
{
    public class Training : MonoBehaviour
    {
        [Header("Маска для выделения")]
        [SerializeField] private GameObject _mask;

        [Header("Панель выбора")]
        [SerializeField] private GameObject _stretcher;

        [Header("Игровые персонажи")]
        [SerializeField] private GameObject _characters;

        [Header("Огоньки на носилках")]
        [SerializeField] private GameObject _fire;

        [Header("Огоньки на дороге")]
        [SerializeField] private GameObject _fireRoad;

        [Header("Перевод текста")]
        [SerializeField] private TextTranslation _textTranslation;

        // Этап обучения
        private int _stage = 0;

        /// <summary>
        /// Обновления этапа обучения
        /// </summary>
        public void RefreshStage()
        {
            _stage++;

            if (_stage <= 8)
            {
                // Выводим следующий текст обучения
                _textTranslation.ChangeKey("training-" + _stage.ToString());

                switch (_stage)
                {
                    case 3:
                        _mask.SetActive(true);
                        SetObjectPositions(_mask.transform.position, new Vector3(3, _characters.transform.position.y, 0));
                        break;
                    case 4:
                        SetObjectPositions(new Vector3(-2, 0.04f, 0), _characters.transform.position);
                        break;
                    case 5:
                        _stretcher.SetActive(true);
                        SetObjectPositions(new Vector3(-8.1f, -0.9f, 0), new Vector3(-8.1f, _characters.transform.position.y, 0));
                        break;
                    case 6:
                        _fire.SetActive(true);
                        SetObjectPositions(new Vector3(7, -2.7f, 0), new Vector3(6.3f, _characters.transform.position.y, 0));
                        break;
                    case 7:
                        _fireRoad.SetActive(true);
                        _fire.SetActive(false);
                        SetObjectPositions(new Vector3(1.8f, -3.35f, 0), new Vector3(-0.3f, _characters.transform.position.y, 0));
                        break;
                    case 8:
                        _mask.SetActive(false);
                        _fireRoad.SetActive(false);
                        break;
                }
            }
            else
            {
                // Записываем прохождение обучения
                PlayerPrefs.SetString("training", "yes");
                // Возвращаемся в список уровней
                Camera.main.GetComponent<TransitionsManager>().GoToScene((int)TransitionsManager.Scenes.Levels);
            }
        }

        /// <summary>
        /// Установка позиции обучающих объектов
        /// </summary>
        /// <param name="mask">позиция маски</param>
        /// <param name="characters">позиция персонажей</param>
        private void SetObjectPositions(Vector3 mask, Vector3 characters)
        {
            _mask.transform.position = mask;
            _characters.transform.position = characters;
        }
    }
}