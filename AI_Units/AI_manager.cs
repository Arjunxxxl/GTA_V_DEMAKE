using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_manager : MonoBehaviour {

	ObjectPooler pooler;

	public GameObject []spawn_points;
	int index = 0;

	// Use this for initialization
	void Start () {
		pooler = ObjectPooler.Instance;

		if(spawn_points.Length == 0)
		{
		spawn_points = GameObject.FindGameObjectsWithTag("spawnpoints");
		}

		for(int i =0; i < 15; i++)
		{	
			index = Random.Range(0, spawn_points.Length);
			pooler.SpawnFrom_Pool("aj", spawn_points[index].transform.position, Quaternion.identity);
		}

		for(int i =0; i < 15; i++)
		{	
			index = Random.Range(0, spawn_points.Length);
			pooler.SpawnFrom_Pool("pearl", spawn_points[index].transform.position, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
