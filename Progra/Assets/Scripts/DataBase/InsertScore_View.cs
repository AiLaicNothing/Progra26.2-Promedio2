using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsertScore_View : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject nicknamePanel;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private Button sendButton;
    [SerializeField] private TextMeshProUGUI messageText;

    private InsertScore_Controller controller;

    private int score;
    private int level;
    private bool hasWon;

    private void Awake()
    {
        controller = GetComponent<InsertScore_Controller>();
    }

    private void Start()
    {
        nicknameInput.characterLimit = 3;

        sendButton.onClick.AddListener(OnSendButtonClicked);

        nicknamePanel.SetActive(false);
    }

    public void Show(int finalScore, bool won, int levelReached)
    {
        score = finalScore;
        hasWon = won;
        level = levelReached;

        nicknameInput.text = "";
        nicknameInput.interactable = true;
        sendButton.interactable = true;
        messageText.text = "";

        nicknamePanel.SetActive(true);
    }

    private void OnSendButtonClicked()
    {
        string nickname = nicknameInput.text.Trim().ToUpper();

        if (nickname.Length == 0 || nickname.Length > 3)
        {
            messageText.text = "Enter 1 to 3 letters";
            return;
        }

        foreach (char letter in nickname)
        {
            if (!char.IsLetter(letter))
            {
                messageText.text = "Only letters allowed";
                return;
            }
        }

        sendButton.interactable = false;
        messageText.text = "Saving...";

        controller.SendRequest(nickname, score, hasWon, level, OnCallback);
    }

    private void OnCallback(bool success)
    {
        if (success)
        {
            messageText.text = "Score saved!";
            nicknameInput.interactable = false;
        }
        else
        {
            messageText.text = "Error saving score";
            sendButton.interactable = true;
        }
    }

    private void OnDestroy()
    {
        sendButton.onClick.RemoveListener(OnSendButtonClicked);
    }
}
