using UnityEngine;
using System.Collections;

public class EnemySpawnerHandler : MonoBehaviour
{
    public float rate;
    private float timer;
    private float totaltime;
    public Baseenemy[] enemies;
    public Baseenemy[] bosses;
    private int numToSpawnatOnce;
	bool stall;
	int num;

    private int maxnumberofenemes = 50;
    private EnemySpawner[] spawnPoints;
    // Use this for initialization
    void Start()
    {
		stall = false;
        timer = rate;
        spawnPoints = FindObjectsOfType<EnemySpawner>();
        numToSpawnatOnce = 1;
		num = 0;

    }

    // Update is called once per frame
    void Update()
    {
		if (stall)
		{
			num++;
			if (num >= 60) {
				stall = false;
			}
		}
        totaltime += Time.deltaTime;
        timer -= Time.deltaTime;
        object[] enemiesonscreen = FindObjectsOfType<Baseenemy>();
        if (timer <= 0)
        {
            for (int i = 0; i < numToSpawnatOnce; i++)
            {
                if (enemiesonscreen.Length > maxnumberofenemes)
                {
                    break;
                }
                Random.seed = System.DateTime.Now.Millisecond;
                int rand1 = Random.Range(0, enemies.Length);

                //bool notReady = true;
                int rand2 = 0;


				Random.seed = System.DateTime.Now.Millisecond + 1;
				rand2 = Random.Range(0, spawnPoints.Length);
                if (spawnPoints[rand2].activated)
				{
					Baseenemy enemy = spawnPoints[rand2].spawn(enemies[rand1]);
					enemy.TimeIncrease(totaltime);
				}

            }
                if ((int)(totaltime) % 300 == 299)
                {
					Debug.Log(totaltime);
                    if (rate > .5)
                    {
                        rate -= Time.deltaTime;
                    }
                    if (numToSpawnatOnce < 15)
                    {
                        numToSpawnatOnce++;
                    }
					if (!stall)
					{
						Random.seed = System.DateTime.Now.Millisecond;
						int rand1 = Random.Range(0, bosses.Length);

						Random.seed = System.DateTime.Now.Millisecond + 1;
						int rand2 = Random.Range(0, spawnPoints.Length);
						Baseenemy enemy = spawnPoints[rand2].spawn(bosses[rand1]);
						enemy.TimeIncrease(totaltime);
						stall = true;
						num = 0;
					}
            	}



                timer += rate;
            }

        }
    }
