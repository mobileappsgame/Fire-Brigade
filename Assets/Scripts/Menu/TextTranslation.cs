using UnityEngine;
using UnityEngine.UI;

public class TextTranslation : MonoBehaviour
{
    [Header("Ключ перевода")]
    [SerializeField] protected string key;

    // Ссылка на компонент
    protected Text textComponent;

    protected void Awake()
    {
        textComponent = GetComponent<Text>();
    }

    protected virtual void Start()
    {
        TranslateText();
    }

    /// <summary>
    /// Вывод переведенного текста из словаря
    /// </summary>
    public virtual void TranslateText()
    {
        textComponent.text = Languages.translations[key];
    }

    /// <summary>
    /// Изменение ключа перевода
    /// </summary>
    /// <param name="value">новый ключ</param>
    public void ChangeKey(string value)
    {
        key = value;
        TranslateText();
    }
}