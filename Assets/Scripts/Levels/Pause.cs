using UnityEngine;

public class Pause : MonoBehaviour
{
    /// <summary>
    /// Настройка игровой паузы
    /// </summary>
    /// <param name="state">Состояние паузы</param>
    public void SetPause(bool state)
    {
        Time.timeScale = state ? 0 : 1;
    }
}