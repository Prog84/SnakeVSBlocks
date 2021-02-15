using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform _container;
    [SerializeField] private int _repeatCount;
    [SerializeField] private int _ditanceBetweenFullLine;
    [SerializeField] private int _ditanceBetweenRandomLine;
    [Header("Block")]
    [SerializeField] private Block _blockTemplate;
    [SerializeField] private int _blockSpawnChance;
    [Header("Wall")]
    [SerializeField] private Wall _wallTemplate;
    [SerializeField] private int _wallSpawnChance;
    [Header("Bonus")]
    [SerializeField] private Bonus _bonusTemplate;
    [SerializeField] private int _bonusSpawnChance;

    private BlockSpawnPoint[] _blockSpawnPoints;
    private WallSpawnPoint[] _wallSpawnPoints;
    private BonusSpawnPoint[] _bonusSpawnPoints;

    private void Start()
    {
        _blockSpawnPoints = GetComponentsInChildren<BlockSpawnPoint>();
        _wallSpawnPoints = GetComponentsInChildren<WallSpawnPoint>();
        _bonusSpawnPoints = GetComponentsInChildren<BonusSpawnPoint>();

        GenerateBorderWalls(_repeatCount);

        for (int i = 0; i < _repeatCount; i++)
        {
            MoveSpawner(_ditanceBetweenFullLine);
            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _ditanceBetweenFullLine, _ditanceBetweenFullLine / 2f);
            GenerateFullLine(_blockSpawnPoints, _blockTemplate.gameObject);
            MoveSpawner(_ditanceBetweenRandomLine);
            GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance, _ditanceBetweenRandomLine, _ditanceBetweenRandomLine / 2f);
            GenerateRandomElements(_blockSpawnPoints, _blockTemplate.gameObject, _blockSpawnChance);
            MoveSpawner(_ditanceBetweenRandomLine);
            GenerateRandomElements(_bonusSpawnPoints, _bonusTemplate.gameObject, _bonusSpawnChance);
        }
    }

    private void GenerateFullLine(SpawnPoint[] spawnPoints, GameObject generatedElement)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GenerateElement(spawnPoints[i].transform.position, generatedElement);
        }
    }

    private void GenerateRandomElements(SpawnPoint[] spawnPoints, GameObject generatedElement, int spawnChance, float scaleY = 0.17f, float offsetY = 0)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Random.Range(0, 100) < spawnChance)
            {
                GameObject element = GenerateElement(spawnPoints[i].transform.position, generatedElement, offsetY);
                element.transform.localScale = new Vector3(element.transform.localScale.x, scaleY, element.transform.localScale.z); 
            }
        }
    }

    private GameObject GenerateElement(Vector3 spawnPoint, GameObject generatedElement, float offsetY = 0)
    {
        spawnPoint.y -= offsetY;
        return Instantiate(generatedElement, spawnPoint, Quaternion.identity, _container);
    }

    private void MoveSpawner(int distanceY)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + distanceY, transform.position.z);
    }

    private void GenerateBorderWalls(int countWall)
    {
        float startAndEndScreenCount = 2;
        Vector2 leftTopScreen = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        Vector2 rightTopScreen = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 screenHeight = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)) - Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        for (int currentWall = 0; currentWall < countWall + startAndEndScreenCount; currentWall++)
        {
            GameObject element = Instantiate(_wallTemplate.gameObject, new Vector3(leftTopScreen.x, currentWall * screenHeight.y, 0), Quaternion.identity, _container);
            element.transform.localScale = new Vector3(element.transform.localScale.x, screenHeight.y, element.transform.localScale.z);

            element = Instantiate(_wallTemplate.gameObject, new Vector3(rightTopScreen.x, currentWall * screenHeight.y, 0), Quaternion.identity, _container);
            element.transform.localScale = new Vector3(element.transform.localScale.x, screenHeight.y, element.transform.localScale.z);
        }
    }
}
