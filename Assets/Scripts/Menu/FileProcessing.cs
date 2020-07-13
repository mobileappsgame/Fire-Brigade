using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Cubra
{
    public class FileProcessing : MonoBehaviour
    {
        /// <summary>
        /// Чтение json файла
        /// </summary>
        /// <param name="fileName">имя файла</param>
        /// <returns>текст из файла</returns>
        protected string ReadJsonFile(string fileName)
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName + ".json");

            // Получаем данные по указанному пути
            UnityWebRequest reader = UnityWebRequest.Get(path);
            // Выполняем обработку полученнных данных
            reader.SendWebRequest();
            // Ждем завершения обработки
            while (!reader.isDone) {}

            return reader.downloadHandler.text;
        }

        /// <summary>
        /// Преобразование json строки в объект
        /// </summary>
        /// <param name="obj">объект для записи</param>
        /// <param name="json">json строка</param>
        protected void ConvertToObject<T>(ref T obj, string json)
        {
            obj = JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// Преобразование объекта в json файл
        /// </summary>
        /// <param name="fileName">файл для записи</param>
        /// <param name="obj">объект</param>
        protected void WriteToFile<T>(string fileName, ref T obj)
        {
            var path = Path.Combine(Application.streamingAssetsPath, fileName + ".json");

            File.WriteAllText(path, JsonUtility.ToJson(obj));
        }
    }
}