using UnityEngine;

public class ReturnBack : MonoBehaviour
{
    [Header("Сцена для возврата")]
    [SerializeField] private int scene;

    // Ссылка на компонент
    private Transitions transitions;

    private void Awake()
    {
        transitions = Camera.main.GetComponent<Transitions>();
    }

    private void Update()
    {
        // Если нажата кнопка возврата
        if (Input.GetKey(KeyCode.Escape))
            // Выполняем переход на сцену
            transitions.GoToScene(scene);
    }
}