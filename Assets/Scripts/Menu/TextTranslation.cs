using Cubra.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Cubra
{
    public class TextTranslation : MonoBehaviour
    {
        [Header("Ключ перевода")]
        [SerializeField] protected string _key;

        protected Text _textComponent;

        protected void Awake()
        {
            _textComponent = GetComponent<Text>();
        }

        protected void Start()
        {
            TranslateText();
        }

        /// <summary>
        /// Вывод текста из словаря
        /// </summary>
        public virtual void TranslateText()
        {
            _textComponent.text = LocalizationController.Translations[_key];
        }

        /// <summary>
        /// Изменение ключа перевода
        /// </summary>
        /// <param name="value">новый ключ</param>
        public void ChangeKey(string value)
        {
            _key = value;
            TranslateText();
        }
    }
}