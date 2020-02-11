using UnityEngine;

public class Drop : MonoBehaviour, IPoolable
{
    [Header("Эффект огненных брызг")]
    [SerializeField] private ParticleSystem spray;

    // Скорость падения капли
    private float speed;

    public void ActivateObject()
    {
        // Перемещаем взятый из пула объект в раздел активных объектов
        gameObject.transform.parent = transform.parent.parent.Find("ActiveObjects");

        // Активируем объект
        gameObject.SetActive(true);

        // Определяем случайную скорость падения
        speed = Random.Range(5f, 6.5f);
    }

    private void Update()
    {
        // Движение капли вниз
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    /// <summary>
    /// Отображение эффекта огненных брызг от капли
    /// </summary>
    public void ShowSplashEffect()
    {
        // Перемещаем эффект к капле
        spray.transform.position = transform.position;
        // Воспроизводим эффект
        spray.Play();
    }
}