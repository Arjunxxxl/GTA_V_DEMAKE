using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Controller : MonoBehaviour {

	[System.Serializable]
	public class MovementSettings
	{
		public string HorizontalAxis = "Horizontal";
		public string VerticalAxis = "Vertical";
		public string FireButton = "Fire1";
	}
	[SerializeField]
	MovementSettings movemntsettings;

	[System.Serializable]
	public class AnimationSettings
	{
		public string Runing = "isRunning1";
		public string Shooting = "isShooting";
	}
	[SerializeField]
	AnimationSettings animationsettings;

	[System.Serializable]
	public class RotationSettings
	{
		public float lookSpeed = 10.0f;
		public float lookDistance = 100.0f;
		public bool requireInputForTurn = true;
		public LayerMask AimLayer;
	}
	[SerializeField]
	RotationSettings rotationsettings;

	public Camera mainCam;
	public Transform finalpos;

	NavMeshAgent mAgent;
	Animator anim;
	bool isRunning = false;
	bool isFire = false;

	Weapon_controller wc;
	int clipAmmo;

	// Use this for initialization
	void Start () {
		mAgent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator>();
		wc = Weapon_controller.Instance;
		clipAmmo = wc.ammunationSettings.clipAmmo;
	}
	
	// Update is called once per frame
	void Update () {
		if(rotationsettings.requireInputForTurn)
		{	
			Movement();
			Fire();
			if(Input.GetAxis(movemntsettings.HorizontalAxis) != 0 || Input.GetAxis(movemntsettings.VerticalAxis) != 0)
			{
				CharacterLook();
			}else
			{
				//CharacterLook();
			}
		}
	}


	void CharacterLook()
	{
			Transform mainCamT = mainCam.transform;
			Transform pivotT = mainCamT.parent;
			Vector3 pivotPos = pivotT.position;

			Vector3 lookTarget = pivotPos + (pivotT.forward * rotationsettings.lookDistance);
			Vector3 thisPos = transform.position;
			Vector3 lookDir = lookTarget - thisPos;

			Quaternion lookRot = Quaternion.LookRotation(lookDir);
			lookRot.x = 0;
			lookRot.z = 0;
			Quaternion newRot = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotationsettings.lookSpeed);
			transform.rotation = newRot;
	}

	void Movement()
	{	
		float h = Input.GetAxis(movemntsettings.HorizontalAxis);
		float v = Input.GetAxis(movemntsettings.VerticalAxis);
		Vector3 des = new Vector3(h , 0, v);
		//mAgent.Move(des);
		//mAgent.SetDestination(transform.position+des);
		mAgent.destination = finalpos.position;
		if(mAgent.remainingDistance <=  mAgent.stoppingDistance)
		{
			isRunning = false;
		}
		else
		{
			isRunning = true;
		}
		anim.SetBool(animationsettings.Runing, isRunning);
	}

	void Fire()
	{	
		clipAmmo = wc.ammunationSettings.clipAmmo;
		if(Input.GetMouseButton(0))
		{
			CharacterLook();
			if(clipAmmo>0)
			{
				isFire = true;
			}
		}
		else
		{
			isFire = false;
		}

		if(Input.GetMouseButton(1))
		{
			CharacterLook();
		}

		anim.SetBool(animationsettings.Shooting, isFire);
	}

}
