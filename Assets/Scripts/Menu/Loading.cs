using System.Collections;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private Transitions transitions;

    private void Awake()
    {
        transitions = GetComponent<Transitions>();
    }

    private void Start()
    {
        StartCoroutine(GoToMenu());
    }

    /// <summary>
    /// Переход в главное меню
    /// </summary>
    private IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(2.0f);
        transitions.GoToScene(Transitions.Scene.Menu);
    }
}