using System;
using TMPro;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public static Player_Manager Instance;

    [SerializeField] private int maxHp;
    private int currentHp;

    private Player player;

    [SerializeField] private TextMeshProUGUI ammo_Text;

    public Action OnAmmoChange;

    private void OnEnable()
    {
        OnAmmoChange += UpdateAmmoUI;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage()
    {
        currentHp--;

        if (currentHp <= 0)
        {
            Game_Manager.Instance.OnDefeat();
        }
    }

    public bool IsDead()
    {
        return currentHp <= 0;
    }

    private void UpdateAmmoUI()
    {
        if (!player.IsReloading)
        {
            ammo_Text.text = $"Ammo: {player.CurrentAmmo}";
        }
        else
        {
            ammo_Text.text = $"Reloading...";
        }
    }
}
