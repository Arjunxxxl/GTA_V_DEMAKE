using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_mover : MonoBehaviour {

	NavMeshAgent agent;
	Animator anim;
	bool isDead, isRunning;
	int index = 0;

	public GameObject []des;

	AI_health health;

	// Use this for initialization
	void Start () {
		health = GetComponent<AI_health>();
		isDead = health.Death();

		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();

		if(des.Length == 0)
		{
			des = GameObject.FindGameObjectsWithTag("spawnpoints");
		}

		index = Random.Range(0, des.Length);

		agent.destination = des[index].transform.position;
		if(agent.remainingDistance <=  agent.stoppingDistance)
		{
			isRunning = false;
		}
		else
		{
			isRunning = true;
		}
		anim.SetBool("isWalking", isRunning);
	}
	
	// Update is called once per frame
	void Update () {
		isDead = health.Death();
		if(isDead) 
		{
			return;
		}

		if(!isRunning)
		{
			index = Random.Range(0, des.Length);

			agent.destination = des[index].transform.position;
			if(agent.remainingDistance <=  agent.stoppingDistance)
			{
				isRunning = false;
			}
			else
			{
				isRunning = true;
			}
		anim.SetBool("isWalking", isRunning);
		}
		else
		{
			if(agent.remainingDistance <=  agent.stoppingDistance)
			{
				isRunning = false;
			}
			else
			{
				isRunning = true;
			}
		anim.SetBool("isWalking", isRunning);
		}

	}
}
