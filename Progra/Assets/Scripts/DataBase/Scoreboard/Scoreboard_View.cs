using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard_View : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button top10Button;
    [SerializeField] private Button winnersButton;
    [SerializeField] private Button averageButton;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI resultsText;

    private Scoreboard_Controller controller;

    private int selectedRanking;

    private void Awake()
    {
        controller = GetComponent<Scoreboard_Controller>();
    }

    private void Start()
    {
        top10Button.onClick.AddListener(ShowTop10);
        winnersButton.onClick.AddListener(ShowWinners);
        averageButton.onClick.AddListener(ShowAverage);

        ShowTop10();
    }

    private void ShowTop10()
    {
        selectedRanking = 1;
        resultsText.text = "Loading...";

        controller.GetTop10(OnTop10Callback);
    }

    private void OnTop10Callback(ScoreArrayModel array)
    {
        if (selectedRanking != 1) return;

        resultsText.text = "TOP 10\n\n";

        if (array == null || array.data == null ||
            array.data.Length == 0)
        {
            resultsText.text += "No scores found";
            return;
        }

        for (int i = 0; i < array.data.Length; i++)
        {
            ScoreModel user = array.data[i];

            resultsText.text +=$"{i + 1}. {user.nickname} - " + $"{user.score} Points - Level {user.lvReached}\n";
        }
    }

    private void ShowWinners()
    {
        selectedRanking = 2;
        resultsText.text = "Loading...";

        controller.GetWinners(OnWinnersCallback);
    }

    private void OnWinnersCallback(ScoreArrayModel array)
    {
        if (selectedRanking != 2) return;

        resultsText.text = "WINNERS\n\n";

        if (array == null || array.data == null ||
            array.data.Length == 0)
        {
            resultsText.text += "No winners found";
            return;
        }

        for (int i = 0; i < array.data.Length; i++)
        {
            ScoreModel user = array.data[i];

            resultsText.text += $"{i + 1}. {user.nickname} - " + $"{user.score} Points\n";
        }
    }

    private void ShowAverage()
    {
        selectedRanking = 3;
        resultsText.text = "Loading...";

        controller.GetAllUsers(OnAverageCallback);
    }

    private void OnAverageCallback(ScoreArrayModel array)
    {
        if (selectedRanking != 3) return;

        resultsText.text = "AVERAGE SCORE BY LEVEL\n\n";

        if (array == null || array.data == null)
        {
            resultsText.text += "No scores found";
            return;
        }

        for (int level = 1; level <= 3; level++)
        {
            long totalScore = 0;
            int players = 0;

            foreach (ScoreModel user in array.data)
            {
                if (user.lvReached == level)
                {
                    totalScore += user.score;
                    players++;
                }
            }

            if (players > 0)
            {
                float average = (float)totalScore / players;

                resultsText.text +=
                    $"Level {level}: {average:F2} Points\n";
            }
            else
            {
                resultsText.text +=
                    $"Level {level}: No scores\n";
            }
        }
    }

    private void OnDestroy()
    {
        top10Button.onClick.RemoveListener(ShowTop10);
        winnersButton.onClick.RemoveListener(ShowWinners);
        averageButton.onClick.RemoveListener(ShowAverage);
    }
}
