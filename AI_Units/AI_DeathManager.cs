using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DeathManager : MonoBehaviour {

	public bool isDead = false;
	ObjectPooler pooler;

	public GameObject []spawn_points;
	int index = 0;

	public string []tags;
	int tag_number = 0;

	string tag;
	AI_health health;

	// Use this for initialization
	void Start () {
		health = GetComponent<AI_health>();
		pooler = ObjectPooler.Instance;
		isDead = health.dead;

		tag_number = 0;

		if(spawn_points.Length == 0)
		{
		spawn_points = GameObject.FindGameObjectsWithTag("spawnpoints");
		}
	}
	
	// Update is called once per frame
	void Update () {
		isDead = health.dead;
		if(isDead)
		{
			tag = gameObject.tag;
			pooler.AddBackToDisctionary(gameObject, tag);
			index = Random.Range(0, spawn_points.Length);

			while(tags[index] != tag)
			{
				index = Random.Range(0, tags.Length);
			}

			pooler.SpawnFrom_Pool(tags[index], spawn_points[index].transform.position, Quaternion.identity);
			gameObject.SetActive(false);
			isDead = false;
		}
	}
}
