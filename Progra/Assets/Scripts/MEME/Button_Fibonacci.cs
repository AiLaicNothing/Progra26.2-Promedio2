using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_Fibonacci : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Button _button;

    private int a = 0;
    private int b = 1;
    private int c;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(FibonacciText);
    }

    private void FibonacciText()
    {
        if (c < 10000)
        {
            text.text = $"Fibonacci: The number is {c}";
        }
        else
        {
            text.text = $"Fibonacci: I dont want to continue boss T_T \n {c}";

        }

        c = a + b;
        a = b;
        b = c;
    }
}
