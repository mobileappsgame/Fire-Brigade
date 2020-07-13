using UnityEngine;
using Cubra.Controllers;

namespace Cubra
{
    public class TextValue : TextTranslation
    {
        [Header("Ключ сохранения")]
        [SerializeField] private string _saveKey;

        /// <summary>
        /// Вывод переведенного текста из словаря с добавлением сохраненного значения
        /// </summary>
        public override void TranslateText()
        {
            _textComponent.text = (_key != "") ? LocalizationController.Translations[_key] + " " : "";
            _textComponent.text += PlayerPrefs.GetInt(_saveKey);
        }
    }
}