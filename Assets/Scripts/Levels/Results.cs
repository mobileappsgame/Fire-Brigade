using UnityEngine;

public class Results : MonoBehaviour
{
    [Header("Панель затемнения")]
    [SerializeField] private GameObject blackout;

    [Header("Персонажи")]
    [SerializeField] private GameObject characters;

    [Header("Игровое меню")]
    [SerializeField] private GameObject menu;

    [Header("Панель результата")]
    [SerializeField] private GameObject results;

    [Header("Панель победы")]
    [SerializeField] private GameObject victory;

    [Header("Панель поражения")]
    [SerializeField] private GameObject lose;

    /// <summary>
    /// Отображение необходимых элементов меню
    /// </summary>
    public void ShowResult()
    {
        blackout.SetActive(true);
        characters.SetActive(true);
        menu.SetActive(true);
        results.SetActive(true);

        if (LevelManager.Mode == "victory")
        {
            victory.SetActive(true);
            // Увеличиваем прогресс викторины
            PlayerPrefs.SetInt("progress", PlayerPrefs.GetInt("progress") + 1);
        }
        else if (LevelManager.Mode == "lose")
        {
            lose.SetActive(true);
        }
    }
}