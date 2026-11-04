using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject main_Panel;
    [SerializeField] private GameObject scoreboard_Panel;

    [SerializeField] private Button scoreboard_Button;

    private void OnEnable()
    {
        scoreboard_Button.onClick.AddListener(ShowScoreboardPanel);
    }

    private void OnDisable()
    {
        scoreboard_Button.onClick.RemoveListener(ShowScoreboardPanel);
    }

    private void Awake()
    {
        main_Panel.SetActive(true);
        scoreboard_Panel.SetActive(false);
    }

    private void ShowScoreboardPanel()
    {
        main_Panel.SetActive(false);
        scoreboard_Panel.SetActive(true);
    }
}
