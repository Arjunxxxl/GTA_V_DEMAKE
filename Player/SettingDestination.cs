using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingDestination : MonoBehaviour {

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
	

	public float speed = 8f;
	float x = 0,z = 0;
	Vector3 pos;
	Vector3 initialPos;

	Camera mainCam;


	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
		x = transform.position.x;
		z = transform.position.z;
		
		pos = transform.localPosition;
		initialPos = pos;
	}
	
	// Update is called once per frame
	void Update () {
		
 
         if (Input.GetKey ("w")) {
             pos.z += speed * Time.deltaTime;
         }
         if (Input.GetKey ("s")) {
             pos.z -= speed * Time.deltaTime;
         }
         if (Input.GetKey ("d")) {
             pos.x += speed * Time.deltaTime;
         }
         if (Input.GetKey ("a")) {
             pos.x -= speed * Time.deltaTime;
         }

		 if (Input.GetKeyUp ("w")) {
             pos.z = initialPos.z;
         }
         if (Input.GetKeyUp ("s")) {
            pos.z = initialPos.z;
         }
         if (Input.GetKeyUp ("d")) {
            pos.x = initialPos.x;
         }
         if (Input.GetKeyUp ("a")) {
             pos.x = initialPos.x;
         }

             
		CharacterLook();

         transform.localPosition = pos.normalized*1.5f;

		 
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

}
