using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;

    float spawnTimer;

    int spawnCount;

    bool startSpawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (startSpawning == true)
        {
            if (spawnCount <= numToSpawn && spawnTimer >= timeBetweenSpawns)
            {
                spawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = false;
        }
    }

    void spawn()
    {
        int arrPos = Random.Range(0, spawnPos.Length);

        Instantiate(objectToSpawn, spawnPos[arrPos].position, spawnPos[arrPos].rotation);
        spawnCount++;
        spawnTimer = 0;
    }
}
