using UnityEngine;

public class Training : MonoBehaviour
{
    [Header("Маска для выделения")]
    [SerializeField] private GameObject mask;

    [Header("Игровые персонажи")]
    [SerializeField] private GameObject characters;

    [Header("Огоньки на носилках")]
    [SerializeField] private GameObject fire;

    [Header("Компонент перевода")]
    [SerializeField] private TextTranslation textTranslation;

    // Этап обучения
    private int stage = 0;

    /// <summary>
    /// Обновления этапа обучения
    /// </summary>
    public void RefreshStage()
    {
        stage++;

        if (stage <= 7)
        {
            // Выводим следующий текст обучения
            textTranslation.ChangeKey("training-" + stage.ToString());

            switch (stage)
            {
                case 3:
                    mask.SetActive(true);
                    SetObjectPositions(mask.transform.position, new Vector3(3, characters.transform.position.y, 0));
                    break;
                case 4:
                    SetObjectPositions(new Vector3(-2, 0.04f, 0), characters.transform.position);
                    break;
                case 5:
                    SetObjectPositions(new Vector3(-8.1f, -1.8f, 0), new Vector3(-8.1f, characters.transform.position.y, 0));
                    break;
                case 6:
                    fire.SetActive(true);
                    SetObjectPositions(new Vector3(7, -2.7f, 0), new Vector3(6.3f, characters.transform.position.y, 0));
                    break;
                case 7:
                    mask.SetActive(false);
                    fire.SetActive(false);
                    SetObjectPositions(mask.transform.position, new Vector3(0, characters.transform.position.y, 0));
                    break;
            }
        }
        else
        {
            // Записываем прохождение обучения
            PlayerPrefs.SetString("training", "yes");
            // Возвращаемся в список уровней
            Camera.main.GetComponent<Transitions>().GoToScene(2);
        } 
    }

    /// <summary>
    /// Установка позиции обучающих объектов
    /// </summary>
    /// <param name="mask">Позиция маски</param>
    /// <param name="characters">Позиция персонажей</param>
    private void SetObjectPositions(Vector3 mask, Vector3 characters)
    {
        this.mask.transform.position = mask;
        this.characters.transform.position = characters;
    }
}