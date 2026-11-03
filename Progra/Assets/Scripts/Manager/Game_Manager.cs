using System;
using TMPro;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance;

    [Header("UI - Screens")]
    [SerializeField] private GameObject victory_Panel;
    [SerializeField] private GameObject defeat_Panel;

    [Header("Score")]
    [SerializeField] private int score;
    [SerializeField] private TextMeshProUGUI score_Text;

    [Header("Victory Bonus")]
    [SerializeField] private int pointsPerRemainingHp = 100;

    [Header("Game State")]
    [SerializeField] private bool hasWon;
    private bool isGameOver;

    [Header("Database")]
    [SerializeField] private InsertScore_View insertScoreView;
    [SerializeField] private Wave_Manager waveManager;

    private Action OnScoreChange;

    public int Score => score;
    public bool IsGameOver => isGameOver;
    public bool HasWon => hasWon;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        OnScoreChange += UpdateScore;
    }

    private void OnDisable()
    {
        OnScoreChange -= UpdateScore;
    }

    private void Start()
    {
        victory_Panel?.SetActive(false);
        defeat_Panel?.SetActive(false);

        UpdateScore();
    }

    public void IncreaseScore(int amount)
    {
        if (isGameOver) return;

        score += amount;

        OnScoreChange?.Invoke();
    }

    public void OnVictory()
    {
        if (isGameOver) return;

        hasWon = true;

        int remainingHp = Player_Manager.Instance.CurrentHp;

        int bonusScore = remainingHp * pointsPerRemainingHp;

        IncreaseScore(bonusScore);

        hasWon = true;
        isGameOver = true;

        victory_Panel?.SetActive(true);

        insertScoreView.Show(score, true,waveManager.HighestLevelReached);

        Debug.Log($"Victory! Health bonus: {bonusScore}");
    }

    public void OnDefeat()
    {
        if (isGameOver) return;

        hasWon = false;
        isGameOver = true;

        defeat_Panel?.SetActive(true);

        insertScoreView.Show(score, false, waveManager.HighestLevelReached);
    }

    #region UI RELATED

    private void UpdateScore()
    {
        if (score_Text != null)
        {
            score_Text.text = $"Score: {score}";
        }
    }

    #endregion
}