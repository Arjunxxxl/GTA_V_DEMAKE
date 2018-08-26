using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;
	public bool auto_target;
	public LayerMask wallLayer;

	public enum Shoulder{
		Right,Left
	}
	public Shoulder shoulder;

	[System.Serializable]
	public class CameraSettings
	{
		[Header("--Positioning--")]
		public Vector3 camoffSetRight;
		public Vector3 camoffSetLeft;

		[Header("--Camera Options--")]
		public float mouseSenX = 5.0f;
		public float mouseSenY = 5.0f;
		public float minAngle = -50.0f;
		public float maxAngle = 10.0f;
		public float rotaionSpeed = 5.0f;
		public float maxCheckDist = 0.1f;

		[Header("--Zoom--")]
		public float zoomFieldOfView = 40.0f;
		public float fieldOfView = 60.0f;
		public float zoomSpeed = 3.0f;

		[Header("--Visual Options--")]
		public float HideMeshWhenDist = 1.5f;
	}
	[SerializeField]
	public CameraSettings camerasettings;

	[System.Serializable]
	public class InputSettings
	{
		public string VerticalAxis = "Mouse X";
		public string HorizontalAxis = "Mouse Y";
		public string AimButton = "Fire2";
		public string SwitchShoulder = "SwitchShoulder";
	}
	[SerializeField]
	public InputSettings inputsettings;

	[System.Serializable]
	public class MovementSetitngs{
		public float movementSpeed = 5.0f;
	}
	[SerializeField]
	public MovementSetitngs movement;

	Transform pivot;
	Camera mainCam;
	float newX = 0f;
	float newY = 0f;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
		pivot = transform.GetChild(0);
		 Cursor.visible = false;
		 Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		if(target)
		{
			if(Application.isPlaying)
			{
				RotateCamera();
				CheckWalls();
				HideMesh();
				ZoomCamera(Input.GetButton(inputsettings.AimButton));
				if(Input.GetButtonDown(inputsettings.SwitchShoulder))
				{
					SwitchShoulder();
				}
			}
		}
	}

	void LateUpdate()
	{
		if(!target)
		{
			TargetPlayer();
		}
		else{
			Vector3 targetPos = target.position;
			Quaternion targetRotation = target.rotation;
			FollowPlayer(targetPos, targetRotation);
		}
	}

	void RotateCamera()
	{
		if(!pivot) return;

		newX += camerasettings.mouseSenX * Input.GetAxis(inputsettings.VerticalAxis);
		newY += camerasettings.mouseSenY * Input.GetAxis(inputsettings.HorizontalAxis);

		Vector3 euilerAngle = new Vector3();
		euilerAngle.x = -newY;
		euilerAngle.y = newX;

		newX = Mathf.Repeat(newX, 360);
		newY = Mathf.Clamp(newY, camerasettings.minAngle, camerasettings.maxAngle);
		Quaternion rot = Quaternion.Slerp(pivot.localRotation, Quaternion.Euler(euilerAngle), Time.deltaTime * camerasettings.rotaionSpeed);
		pivot.localRotation = rot;
	}

	void TargetPlayer()
	{
		if(auto_target)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			if(player)
			{
				Transform tempT = player.transform;
				target = tempT;
			}
		}
	}

	void FollowPlayer(Vector3 tpos, Quaternion trot)
	{
		if(!Application.isPlaying)
		{
			transform.position = tpos;
			transform.rotation = trot;
		}
		else{
			Vector3 newPos = Vector3.Lerp(transform.position, tpos, Time.deltaTime * movement.movementSpeed);
			transform.position = newPos;
		}
	}

	void ZoomCamera(bool isZooming)
	{	//Debug.Log("Working");
		if(!mainCam) return;
		
		if(isZooming)
		{
			float newFieldOfView = Mathf.Lerp(mainCam.fieldOfView, camerasettings.zoomFieldOfView, Time.deltaTime * camerasettings.zoomSpeed);
			mainCam.fieldOfView = newFieldOfView;
		}
		else
		{
			float originalFieldOfView = Mathf.Lerp(mainCam.fieldOfView, camerasettings.fieldOfView, Time.deltaTime * camerasettings.zoomSpeed);
			mainCam.fieldOfView = originalFieldOfView;
		}
	}

	void CheckWalls()
	{
		if(!pivot || !mainCam) return;

		RaycastHit hit;

		Transform miancamT = mainCam.transform;
		Vector3 mainCamPos = miancamT.position;
		Vector3 pivotPos = pivot.position;

		Vector3 start = pivotPos;
		Vector3 dir = mainCamPos - pivotPos;
		float dist = Mathf.Abs(shoulder == Shoulder.Left ? camerasettings.camoffSetLeft.z : camerasettings.camoffSetRight.z);

		if(Physics.SphereCast(start, mainCam.nearClipPlane /*camerasettings.maxCheckDist*/, dir, out hit, dist, wallLayer))
		{
			MoveCamup(hit, pivotPos, dir, miancamT);
		}else
		{
			switch(shoulder)
			{
				case Shoulder.Left: PositonCamera(camerasettings.camoffSetLeft); break;
				case Shoulder.Right: PositonCamera(camerasettings.camoffSetRight); break;
			}
		}
	}

	void MoveCamup(RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform miancamT)
	{
		float hitDist = hit.distance;
		Vector3 radius = pivotPos + (dir.normalized * hitDist);
		miancamT.position = radius;
	}

	void PositonCamera(Vector3 campos)
	{
		if(!mainCam) return;
		Transform miancamT = mainCam.transform;
		Vector3 mainCamPos = miancamT.localPosition;

		Vector3 newpos = Vector3.Lerp(mainCamPos, campos, Time.deltaTime * movement.movementSpeed);
		miancamT.localPosition = newpos;
	}

	void SwitchShoulder()
	{
		switch(shoulder)
		{
			case Shoulder.Left:
				shoulder = Shoulder.Right;
			break;
			case Shoulder.Right:
			shoulder = Shoulder.Left;
			break;
		}
	}

	void HideMesh()
	{
		if(!mainCam || !target) return;

		SkinnedMeshRenderer[] mySkin = target.GetComponentsInChildren<SkinnedMeshRenderer>();
		Transform maincamT = mainCam.transform;
		Vector3 mainCamPos = maincamT.position;
		Vector3 targetPos = target.position;
		float dist = Vector3.Distance(mainCamPos, (targetPos + target.up));

		if(mySkin.Length > 0)
		{
			for(int i = 0; i<mySkin.Length; i++)
			if(dist<camerasettings.HideMeshWhenDist)
			{
				mySkin[i].enabled = false;
			}else
			{
				mySkin[i].enabled = true;
			}
		}
	}

}
