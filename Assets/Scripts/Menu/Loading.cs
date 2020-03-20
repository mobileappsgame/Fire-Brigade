using System.Collections;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private Transitions transitions;

    private void Awake()
    {
        transitions = Camera.main.GetComponent<Transitions>();
    }

    private void Start()
    {
        // Игровой прогресс
        if (!PlayerPrefs.HasKey("progress")) PlayerPrefs.SetInt("progress", 1);

        StartCoroutine(GoToMenu());
    }

    /// <summary>
    /// Переход в главное меню
    /// </summary>
    private IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(2.0f);
        transitions.GoToScene(1);
    }
}