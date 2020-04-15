using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Appodeal.Editor.AppodealManager
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T> {Items = array};
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            var wrapper = new Wrapper<T> {Items = array};
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }
        
        public static string fixJson(string value)
        {
            value = "{\"Items\":" + value + "}";
            return value;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}