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
        public Level Level;
        public EnemySpawn[] spawn;
    }

    [System.Serializable]
    public struct EnemySpawn
    {
        public int amount;
        public GameObject enemyType;
    }

    [SerializeField] private Level currentLevel = Level.None;
    [SerializeField] private LevelComposition[] level;
    [SerializeField] private Transform[] spawnpoints;

    private List<GameObject> enemies = new();

    private void Start()
    {
        StartCoroutine(HandleWave());
    }

    private IEnumerator HandleWave()
    {
        currentLevel = Level.Level_1;

        if (currentLevel == Level.Level_1)
        {
            LevelComposition composition = level[0];

            foreach (EnemySpawn enemy in composition.spawn)
            {
                for (int i = 0; i< enemy.amount; i++)
                {
                    int selectSpawnRandomly = Random.Range(0, spawnpoints.Length);

                    Transform spawnpoint = spawnpoints[selectSpawnRandomly];

                    GameObject asd = Instantiate(enemy.enemyType, spawnpoint);

                    Debug.Log(asd.name);

                    enemies.Add(asd);

                    yield return new WaitForSeconds(1.5f);
                }
            }

            while (enemies.Count > 0)
            {
                enemies.RemoveAll(enemy => enemy == null);
                yield return null;
            }

            currentLevel = Level.Level_2;
        }

        if (currentLevel == Level.Level_2)
        {
            Debug.Log("Pasaste de nivel");
        }

        yield return null;
    }
}
