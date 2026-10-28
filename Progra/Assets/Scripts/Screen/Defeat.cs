using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Defeat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score_Text;
    [SerializeField] private InputField username_Inputfield;
    private Button sendScore_Button;

    private void Start()
    {
        score_Text.text = $"SCORE: {Game_Manager.Instance.Score}";
    }
}
