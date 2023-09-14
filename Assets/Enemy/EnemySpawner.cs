using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    private UIShop Shop;
    public Transform Player;
    public int NumberOfEnemiesToSpawn = 5;
    public float SpawnDelay = 1f;
    //public List<Enemy> EnemyPrefabs = new List<Enemy>();
    public List<EnemyScriptableObject> Enemies = new List<EnemyScriptableObject>();
    public SpawnMethod EnemySpawnMethod = SpawnMethod.RoundRobin;
    public bool ContinousSpawning;
    public ScalingScriptableObject Scaling;
    [Space]
    [Header("Read At Runtime")]
    [SerializeField]
    private int Level = 0;
    [SerializeField]
    private List<EnemyScriptableObject> ScaledEnemies = new List<EnemyScriptableObject>();
    [SerializeField]
    private int LimitLevel = 1;
    
    [SerializeField]
    private int LimitLevel2 = 6;
    [SerializeField]
    private int RestTime = 30;

    [SerializeField]
    private GameObject UIShop;
    [SerializeField]
    private NavMeshSurface oldNavMeshSurface;

    private int EnemiesAlive = 0;
    private int SpawnedEnemies = 0;
    private int InitialEnemiesToSpawn;
    private float InitialSpawnDelay;

    private NavMeshTriangulation Triangulation;
    private Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    public Text EnemiesLeftText;
    public Text RestTimeText;
    public Text WaveAlerts;

    public Image winScreen;
    public AudioSource musicManager;

    private void Awake()
    {
        for(int i = 0; i < Enemies.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(Enemies[i].Prefab, NumberOfEnemiesToSpawn));
        }
        InitialEnemiesToSpawn = NumberOfEnemiesToSpawn;
        InitialSpawnDelay = SpawnDelay;
    }

    private void Start()
    {
        //Debug.Log(GameObject.Find("UIShop"));
        Shop = GameObject.Find("chair (6)").GetComponent<UIShop>();
        Triangulation = NavMesh.CalculateTriangulation();
        for(int i = 0; i < Enemies.Count; i++)
        {
            ScaledEnemies.Add(Enemies[i].scaleUpForLevel(Scaling, Level));
        }
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        StartCoroutine(WaveAlertsTextBlink("Wave has started!"));
        Shop.wave = true;
        Level++;
        SpawnedEnemies = 0;
        EnemiesAlive = 0;

        for (int i = 0; i < Enemies.Count; i++)
        {
            ScaledEnemies[i] = Enemies[i].scaleUpForLevel(Scaling, Level);
        }

        WaitForSeconds Wait = new WaitForSeconds(SpawnDelay);


        while(SpawnedEnemies < NumberOfEnemiesToSpawn)
        {
            if (EnemySpawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(SpawnedEnemies);
            } else if (EnemySpawnMethod == SpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }

            SpawnedEnemies++;
            yield return Wait;
        }

        if (ContinousSpawning)
        {
            ScaleUpSpawns();
            StartCoroutine(SpawnEnemies());
        }
    }

    private void SpawnRoundRobinEnemy(int SpawnedEnemies)
    {
        int SpawnIndex = SpawnedEnemies % Enemies.Count;

        DoSpawnEnemy(SpawnIndex);
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, Enemies.Count));
    }

    private void DoSpawnEnemy(int SpawnIndex)
    {
        PoolableObject poolableObject = EnemyObjectPools[SpawnIndex].GetObject();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();
            ScaledEnemies[SpawnIndex].SetupEnemy(enemy);

            int VertexIndex = Random.Range(0, Triangulation.vertices.Length);

            NavMeshHit Hit;
            if(NavMesh.SamplePosition(Triangulation.vertices[VertexIndex], out Hit, 2f, -1))
            {
                enemy.Agent.Warp(Hit.position);
                enemy.Movement.Target = Player;
                enemy.Agent.enabled = true;
                enemy.Movement.StartChasing();
                enemy.OnDie += HandleEnemyDeath;

                EnemiesAlive++;

                SetEnemiesLeftText(EnemiesAlive);
            }
            else
            {
                Debug.LogError($"Nie udalo sie umiescic NavMeshAgent na NavMesh. Probowano uzyc {Triangulation.vertices[VertexIndex]}");
            }
        } 
        else
        {
            Debug.LogError($"Nie mozna pobrac przeciwnika typu {SpawnIndex} z puli obiektow.");
        }
    }


    private void ScaleUpSpawns()
    {
        NumberOfEnemiesToSpawn = Mathf.FloorToInt(InitialEnemiesToSpawn * Scaling.SpawnCountCurve.Evaluate(Level + 1));
        SpawnDelay = InitialSpawnDelay * Scaling.SpawnRateCurve.Evaluate(Level + 1);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        EnemiesAlive--;

        SetEnemiesLeftText(EnemiesAlive);

        if (EnemiesAlive == 0 && SpawnedEnemies == NumberOfEnemiesToSpawn && Level == LimitLevel2)
        {
            Player.gameObject.SetActive(false);
            winScreen.gameObject.SetActive(true);
            musicManager.enabled = false;
        }

        if (EnemiesAlive == 0 && SpawnedEnemies == NumberOfEnemiesToSpawn && Level == LimitLevel)
        {
            ChangeScene();
        }

        if (EnemiesAlive == 0 && SpawnedEnemies == NumberOfEnemiesToSpawn && Level != LimitLevel)
        {
            StartCoroutine(DoNextRound());
        }
    }

    private IEnumerator DoNextRound()
    {
        Shop.wave = false;
        for (int i = RestTime ; i > 0 ; i--)
        {
            ShowRestTime(i.ToString());
            yield return new WaitForSeconds(1);
        }
        RestTimeText.text = "Rest time: --";
        ScaleUpSpawns();
        StartCoroutine(SpawnEnemies());
    }

    public enum SpawnMethod 
    { 
        RoundRobin,
        Random
    }

    /* TODO */
    // pokazuje ilosc przeciwnikow
    public void SetEnemiesLeftText(int enemies)
    {
        if (enemies > 0)
        {
            EnemiesLeftText.text = "Enemies left: " + enemies.ToString();
        }
        else
        {
            EnemiesLeftText.text = "Enemies left: --";
        }

    }


    // timer przerwy miedzy falami
    // wywolanie -> StartCoroutine(SetRestTimer(RestTime));
    public void ShowRestTime(string time)
    {
        RestTimeText.text = "Rest time: " + time;
    }

    // info o rozpoczeciu/zakonczeniu fali
    // wywolanie -> StartCoroutine(WaveAlertsTextBlink("Wave has started!"));
    public IEnumerator WaveAlertsTextBlink(string WaveAlertsText)
    {
        int i = 0;
        while (i < 3)
        {
            WaveAlerts.text = WaveAlertsText;
            yield return new WaitForSeconds(.5f);
            WaveAlerts.text = "";
            yield return new WaitForSeconds(.5f);
            i++;
        }

    }

    private IEnumerator GoToScene(int nextSceneIndex, List<GameObject> objectsList)
    {
        SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);

        Scene nextScene = SceneManager.GetSceneByBuildIndex(nextSceneIndex);

        foreach (GameObject obj in objectsList)
        {
            SceneManager.MoveGameObjectToScene(obj, nextScene);
        }

        yield return null;

        SceneManager.UnloadSceneAsync(nextSceneIndex - 1);

        oldNavMeshSurface.enabled = false;
        Triangulation = NavMesh.CalculateTriangulation();
        EnemyObjectPools.Clear();

        for (int i = 0; i < Enemies.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(Enemies[i].Prefab, NumberOfEnemiesToSpawn));
        }

        StartCoroutine(DoNextRound());
    }

    private void ChangeScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch(currentSceneName)
        {
            case "Level01":
                Vector3 v = new Vector3();
                List<GameObject> objects = new List<GameObject>()
                {
                    GameObject.Find("Main Camera"),
                    GameObject.Find("Player"),
                    GameObject.Find("Enemy Manager"),
                    GameObject.Find("Music Manager"),
                    GameObject.Find("Directional Light"),
                    GameObject.Find("HUD"),
                    GameObject.Find("chair (6)"),
                    UIShop
                };

                v = objects[0].transform.localPosition;
                v.x = (float)-57.3;
                v.z = (float)-44.36501;
                objects[0].transform.localPosition = v;

                v = objects[1].transform.localPosition;
                v.x = (float)-57.3;
                v.z = (float)-40.8;
                objects[1].transform.localPosition = v;

                v = objects[4].transform.localPosition;
                v.x = (float)-57.3;
                v.z = (float)-40.78502;
                objects[4].transform.localPosition = v;


                objects[6].transform.parent = null;
                DontDestroyOnLoad(objects[6]);

                v = objects[6].transform.localPosition;
                v.x = (float)-64.419;
                v.z = (float)-51.79;
                objects[6].transform.localPosition = v;
                var scale = objects[6].transform.localScale;
                scale.x = 0.8f;
                scale.y = 0.8f;
                scale.z = 0.8f;
                objects[6].transform.localScale = scale;

                StartCoroutine(GoToScene(2, objects));
                break;
        }
    }

}
