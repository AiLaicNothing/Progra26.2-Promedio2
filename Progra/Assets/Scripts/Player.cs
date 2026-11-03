using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask targetLayers;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float reloadTime;
    [SerializeField] private int maxAmmo;
    private int currentAmmo;
    private bool isReloading;

    public int CurrentAmmo => currentAmmo;

    public bool IsReloading => isReloading;


    private void Start()
    {
        currentAmmo = maxAmmo;
        Player_Manager.Instance.OnAmmoChange?.Invoke();
    }

    private void Update()
    {
        if (Player_Manager.Instance.IsDead()) return;

        if (currentAmmo > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleShoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!isReloading) StartCoroutine(HandleReload());
            }
        }
        else
        {
            if (!isReloading) StartCoroutine(HandleReload());
        }
    }

    private void HandleShoot()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            Debug.Log($"{hit.collider.name}");
            var damageable = hit.collider.GetComponent<IDamageable>();

            if (damageable != null) damageable.TakeDamage(damage);
        }

        currentAmmo--;
        Player_Manager.Instance.OnAmmoChange?.Invoke();
    }

    private IEnumerator HandleReload()
    {
        isReloading = true;

        Player_Manager.Instance.OnAmmoChange?.Invoke();

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;

        isReloading = false;

        Player_Manager.Instance.OnAmmoChange?.Invoke();

        yield return null;
    }
}
