﻿using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        // Находим все объекты с фоновой музыкой
        Music[] objs = FindObjectsOfType<Music>();

        // Если их больше одного, уничтожаем лишние
        if (objs.Length > 1) Destroy(gameObject);

        // Отключаем уничтожение оставшегося объекта
        DontDestroyOnLoad(gameObject);
    }
}