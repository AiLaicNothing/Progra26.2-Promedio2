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
    private Action OnScoreChange;

    [SerializeField] private bool hasWon;

    public float Score => score;

    private void OnEnable()
    {
        OnScoreChange += UpdateScore;
    }

    private void OnDisable()
    {
        OnScoreChange -= UpdateScore;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        victory_Panel.SetActive(false);
        defeat_Panel.SetActive(false);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;

        OnScoreChange?.Invoke();
    }

    public void OnVictory()
    {
        hasWon = true;

        victory_Panel?.SetActive(true);
    }

    public void OnDefeat()
    {
        hasWon = false;

        defeat_Panel?.SetActive(true);
    }

    #region UI RELATED

    private void UpdateScore()
    {
        score_Text.text = $"Score: {score}";
    }

    #endregion
}
