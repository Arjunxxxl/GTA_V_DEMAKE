using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_health : MonoBehaviour {

	Weapon_controller wc;
	public float health = 80.0f;
	GameObject o;

	public bool dead = false;

	public static AI_health Instance;
	private void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		wc= Weapon_controller.Instance;
		health = 80.0f;
		dead = false;
		//Debug.Log(dead);
	}

	private void OnEnable() {
		dead = false;
		health = 80.0f;
	}

	private void OnDisable() {
		dead = false;
		health = 80.0f;
	}
	
	// Update is called once per frame
	void Update () {
		o = wc.Pass_Enemy();
		if(o==this.gameObject)
		{
			health-= 20f;
		}

		if(health<=0f)
		{
			dead = true;
		}

		//Debug.Log("Testing : "+dead);

	}

	public bool Death()
	{
		return dead;
	}

}
