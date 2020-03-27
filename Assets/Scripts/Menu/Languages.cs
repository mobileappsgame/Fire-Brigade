using System.Collections.Generic;
using UnityEngine;

public class Languages : FileProcessing
{
    // Объект для работы с json по переводам
    private LangJson language = new LangJson();

    // Словарь с переводом слов на выбранный язык
    public static Dictionary<string, string> translations = new Dictionary<string, string>();

    private void Awake()
    {
        LanguageSetting();
    }

    /// <summary>
    /// Загрузка перевода из файла
    /// </summary>
    public void LanguageSetting()
    {
        // Читаем файл с переводом
        var json = ReadJsonFile(PlayerPrefs.GetString("language"));

        // Преобразовываем json в объект
        ConvertToObject(ref language, json);

        // Заполняем словарь
        WriteToDictionary();
    }

    /// <summary>
    /// Заполнение словаря переводом
    /// </summary>
    private void WriteToDictionary()
    {
        // Очищаем словарь
        translations.Clear();

        for (int i = 0; i < language.language.Length; i++)
        {
            var key = language.language[i].Key;
            // При отсутствии ключа, добавляем перевод в словарь
            if (!translations.ContainsKey(key)) translations.Add(key, language.language[i].Value);
        }
    }
}