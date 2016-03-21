﻿using UnityEngine;
using System.Collections;

public class EnemySpawnerHandler : MonoBehaviour
{
    public float rate;
    private float timer;
    private float totaltime;
    public Baseenemy[] enemies;
    public Baseenemy[] bosses;
    private int numToSpawnatOnce;

    private int maxnumberofenemes = 250;
    private EnemySpawner[] spawnPoints;
    // Use this for initialization
    void Start()
    {
        timer = rate;
        spawnPoints = FindObjectsOfType<EnemySpawner>();
        numToSpawnatOnce = 1;

    }

    // Update is called once per frame
    void Update()
    {
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

                //while (notReady)
                //{
                    Random.seed = System.DateTime.Now.Millisecond + 1;
                    rand2 = Random.Range(0, spawnPoints.Length);
                 //   if (spawnPoints[rand2].activated)
                    // {
                    //      var player = Tools.findNearest(spawnPoints[rand2].transform.position, "Player");
                    //     Vector3 t = player.position - spawnPoints[rand2].transform.position;
                    //     float distance = Mathf.Sqrt(Mathf.Pow(t.x, 2) + Mathf.Pow(t.y, 2));
                    //     if (distance <= 300)
                    //   {
                   //    notReady = true;
                    //   }

                //}
                Baseenemy enemy = spawnPoints[rand2].spawn(enemies[rand1]);
                enemy.TimeIncrease(totaltime);
                //}
            }
                if ((int)(totaltime) % 300 == 299)
                {
                    if (rate > .5)
                    {
                        rate -= Time.deltaTime;
                    }
                    if (numToSpawnatOnce < 15)
                    {
                        numToSpawnatOnce++;
                    }
                    Random.seed = System.DateTime.Now.Millisecond;
                    int rand1 = Random.Range(0, bosses.Length);
                    Random.seed = System.DateTime.Now.Millisecond + 1;
                    int rand2 = Random.Range(0, spawnPoints.Length);
                    Baseenemy enemy = spawnPoints[rand2].spawn(bosses[rand1]);
                    enemy.TimeIncrease(totaltime);
                }
                timer += rate;
            }

        }
    }
