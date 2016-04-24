using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class EnemySpawnerHandler : MonoBehaviour
{
    public float rate;
	private float r_timer;

    private float timer;
	int cnt=0;
    private static float totaltime;
    public Baseenemy[] enemies;
    public Baseenemy[] bosses;
    private int numToSpawnatOnce;
	int bossSpawn = 300;

    private int maxnumberofenemes = 50;
    private EnemySpawner[] spawnPoints;
    // Use this for initialization
    void Start() {
		rate = 3.5f;
		r_timer = 60f;

		totaltime = 0f;
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

		// Boss spawn.
		if (totaltime > (float)bossSpawn) {

			Random.seed = System.DateTime.Now.Millisecond;
			int rand1 = Random.Range (0, bosses.Length);
			int rand2 = Random.Range (0, spawnPoints.Length);

			// Attempt to spawn a boss
			if (spawnPoints [rand2].activated) {
				bossSpawn+=300;
				Baseenemy enemy = spawnPoints [rand2].spawn (bosses [rand1]);
				enemy.TimeIncrease(totaltime);
			}

		}
			
		if (rate > 0.5f) {
			// Increase difficulty
			if (r_timer <= 0f) {
				rate -= 0.1f;
				r_timer = 60f;

				if (numToSpawnatOnce < 12) {
					numToSpawnatOnce += 1;
				}
			} else {
				r_timer -= Time.deltaTime;
			}
		}

        }
    }
