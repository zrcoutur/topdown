using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class EnemySpawnerHandler : MonoBehaviour
{
    public float rate;
    private float timer;
	int cnt=0;
    private static float totaltime;
    public Baseenemy[] enemies;
    public Baseenemy[] bosses;
    private int numToSpawnatOnce;
	bool stall;
	int bossSpawn = 300;

    private int maxnumberofenemes = 50;
    private EnemySpawner[] spawnPoints;
    // Use this for initialization
    void Start()
    {
		totaltime = 0f;
		stall = false;
        timer = rate;
        spawnPoints = FindObjectsOfType<EnemySpawner>();
        numToSpawnatOnce = 1;

    }

    // Update is called once per frame
    void Update ()
	{

		totaltime += Time.deltaTime;
		timer -= Time.deltaTime;

		int noSpawns = 10;

		while (timer <= 0 && noSpawns >= 0) {

			noSpawns--;

			object[] enemiesonscreen = FindObjectsOfType<Baseenemy> ();

			if (enemiesonscreen.Length > maxnumberofenemes) {
				break;
			}

			Random.seed = System.DateTime.Now.Millisecond;
			int rand1 = Random.Range (0, enemies.Length);

			int rand2 = 0;

			rand2 = Random.Range (0, spawnPoints.Length);

			// Spawn an enemy
			if (spawnPoints [rand2].activated) {

				Baseenemy enemy = spawnPoints [rand2].spawn (enemies [rand1]);
				enemy.TimeIncrease (totaltime);
				timer += rate;
				print (cnt++);

			}

		}

			// Boss spawn + difficulty increase every 5 minutes, roughly.
			if (totaltime > (float)bossSpawn) {

				Random.seed = System.DateTime.Now.Millisecond;
				int rand1 = Random.Range (0, bosses.Length);
				int rand2 = Random.Range (0, spawnPoints.Length);
	
				// Attempt to spawn a boss
				if (spawnPoints [rand2].activated) {
					bossSpawn+=300;
					Baseenemy enemy = spawnPoints [rand2].spawn (bosses [rand1]);
					enemy.TimeIncrease(totaltime);

					// Increase difficulty
					if (rate > .5) {
					rate -= 0.2f;
					}
					if (numToSpawnatOnce < 15) {
						numToSpawnatOnce++;
					}

				}

			}

        }
    }
