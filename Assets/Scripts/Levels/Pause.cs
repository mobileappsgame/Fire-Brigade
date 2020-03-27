using UnityEngine;

public class Pause : MonoBehaviour
{
    /// <summary>
    /// Настройка паузы в игре
    /// </summary>
    /// <param name="state">состояние паузы</param>
    public void SetPause(bool state)
    {
        Time.timeScale = state ? 0 : 1;
    }

    private void OnDestroy()
    {
        // Сбрасываем паузу
        SetPause(false);
    }
}