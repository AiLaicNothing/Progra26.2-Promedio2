using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Zombie : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHp;
    private int currentHp;
    [SerializeField] private Material og;
    [SerializeField] private Material damaged;

    [Header("Attack")]
    [SerializeField] private float attackDistance;
    [SerializeField] private float timeBtwAttack;
    private float timer;

    [SerializeField] private int valuePoints;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private GameObject target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        currentHp = maxHp;
    }

    void Update()
    {
        HandleAI();
    }

    private void HandleAI()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            TakeDamage(1);
        }

        float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);

        if (distance > attackDistance)
        {
            agent.isStopped = false;

            agent.SetDestination(target.transform.position);
        }
        else if (distance <= attackDistance)
        {
            agent.isStopped = true;
            DoDamage();
        }
    }

    private void DoDamage()
    {
        if (timer < timeBtwAttack)
        {
            timer += Time.deltaTime;
        }

        if (timer >= timeBtwAttack)
        {
            timer = 0;
            //Debug.Log("Do damage to player");
            //Player_Manager.Instance.TakeDamage();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        StartCoroutine(ChangeMaterial());

        //Debug.Log($"{name} has taken damage {damage}");

        if (currentHp <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Game_Manager.Instance.IncreaseScore(valuePoints);

        Destroy(gameObject);
    }

    private IEnumerator ChangeMaterial()
    {
        var _renderer = GetComponent<MeshRenderer>();

        float timer = 0f;

        timer += Time.deltaTime;

        if (_renderer != null)
        {
            _renderer.material = damaged;
        }

        yield return new WaitForSeconds(0.15f);

        _renderer.material = og;

        yield return null;
    }
}
