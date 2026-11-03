using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Wave_Manager : MonoBehaviour
{
    public enum Level
    {
        None,
        Level_1,
        Level_2,
        Level_3
    }

    [System.Serializable]
    public struct LevelComposition
    {
        public Level level;

        [Header("Player Destination")]
        public Transform playerDestination;

        [Header("Score")]
        [Min(0)] public int scoreValue;

        [Header("Enemies")]
        public EnemySpawn[] spawn;
        public Transform[] spawnPoints;
    }

    [System.Serializable]
    public struct EnemySpawn
    {
        [Min(0)] public int amount;
        public GameObject enemyType;
    }

    [Header("Level")]
    [SerializeField] private Level currentLevel = Level.None;
    [SerializeField] private LevelComposition[] level;

    [Header("Player Movement")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float moveDuration = 3f;

    [Header("Spawning")]
    [SerializeField] private float spawnInterval = 1.5f;

    public int HighestLevelReached { get; private set; } = 1;


    private readonly List<GameObject> enemies = new();

    private void Start()
    {
        if (playerTransform == null || level == null || level.Length == 0)
        {
            Debug.LogError("Wave Manager is missing setup references.");
            return;
        }

        StartCoroutine(HandleWaves());
    }

    private IEnumerator HandleWaves()
    {
        for (int i = 0; i < level.Length; i++)
        {
            if (Game_Manager.Instance.IsGameOver) yield break;

            LevelComposition composition = level[i];

            currentLevel = composition.level;

            if (composition.playerDestination != null)
            {
                yield return StartCoroutine(MovePlayer(composition.playerDestination));
            }

            if (Game_Manager.Instance.IsGameOver)
                yield break;

            HighestLevelReached = Mathf.Max(HighestLevelReached, (int)currentLevel);

            if (composition.spawnPoints == null || composition.spawnPoints.Length == 0)
            {
                Debug.LogError($"No spawn points configured for {currentLevel}.");
                yield break;
            }

            yield return StartCoroutine(SpawnLevel(composition));

            if (Game_Manager.Instance.IsGameOver) yield break;

            Game_Manager.Instance.IncreaseScore(composition.scoreValue);

            Debug.Log($"{currentLevel} completed!");
        }

        if (!Game_Manager.Instance.IsGameOver)
        {
            currentLevel = Level.None;
            Game_Manager.Instance.OnVictory();
        }
    }

    private IEnumerator MovePlayer(Transform destination)
    {
        Vector3 startPosition = playerTransform.position;
        Quaternion startRotation = playerTransform.rotation;

        float elapsed = 0f;
        float duration = Mathf.Max(0.01f, moveDuration);

        while (elapsed < duration)
        {
            if (Game_Manager.Instance.IsGameOver) yield break;

            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            playerTransform.position = Vector3.Lerp( startPosition, destination.position, smoothT);

            playerTransform.rotation = Quaternion.Slerp( startRotation, destination.rotation, smoothT);

            yield return null;
        }

        if (Game_Manager.Instance.IsGameOver) yield break;

        playerTransform.SetPositionAndRotation(destination.position, destination.rotation);
    }

    private IEnumerator SpawnLevel(LevelComposition composition)
    {
        enemies.Clear();

        if (composition.spawn != null)
        {
            foreach (EnemySpawn enemy in composition.spawn)
            {
                if (enemy.enemyType == null) continue;

                for (int i = 0; i < enemy.amount; i++)
                {
                    if (Game_Manager.Instance.IsGameOver) yield break;

                    int index = Random.Range(0, composition.spawnPoints.Length);

                    Transform spawnPoint = composition.spawnPoints[index];

                    if (spawnPoint == null)
                    {
                        Debug.LogWarning("A spawn point is missing.");
                        continue;
                    }

                    GameObject spawnedEnemy = Instantiate( enemy.enemyType, spawnPoint.position, spawnPoint.rotation);

                    enemies.Add(spawnedEnemy);

                    yield return new WaitForSeconds(Mathf.Max(0f, spawnInterval));
                }
            }
        }

        while (true)
        {
            if (Game_Manager.Instance.IsGameOver) yield break;

            enemies.RemoveAll(enemy => enemy == null);

            if (enemies.Count == 0) break;

            yield return null;
        }

        enemies.Clear();
    }
}
