using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon_controller : MonoBehaviour {

	[System.Serializable]
	public class WeaponSettings
	{
		[Header("--Bullet Options--")]
		public Transform bulletSpawn;
		public float buttetDmg = 20.0f;
		public float butterSpread = 0.05f;
		public float FireRate = 0.9f;
		public LayerMask BulletLayer;
		public float butterRange = 1000f;

		[Header("--Effects--")]
		public GameObject muzzleFlash;
		public GameObject decal;
		public GameObject clip;
		public GameObject shell;

		[Header("--Others--")]
		public float reloadDuration = 2.0f;
		public GameObject reloadIndicator;
		public Transform shellEhjectPos;
		public float shellEjectSpeed = 2.0f;
		public Transform clipEjectPos;
		public GameObject clipGo;
	}
	[SerializeField]
	public WeaponSettings weaponsettings;

	[System.Serializable]
	public class AmunationSettings
	{
		public int carryingAmmo = 500;
		public int maxClipAmmo = 30;
		public int  clipAmmo = 15;
	}
	[SerializeField]
	public AmunationSettings ammunationSettings;

	public bool pullingTrigger = false;
	public bool resittngCatridge = false;

	public Ray shootRay{protected get; set;}

	Ray shootRay1;
	RaycastHit shootHit;

	GameObject enemy_Refrence;
	public static Weapon_controller Instance;
	private void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		weaponsettings.reloadIndicator.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		TriggerState();

		if(Input.GetKeyDown(KeyCode.R))
		{
			StartCoroutine(Reload());
			weaponsettings.reloadIndicator.SetActive(true);
		}

		if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0)
		{return;}

		enemy_Refrence = null;

		if(pullingTrigger)
		{
			Fire(shootRay);
			//Shoot ();
		}

	}

	void TriggerState()
	{
		if(Input.GetMouseButton(0))
		{
			pullingTrigger = true;
			//Debug.Log("pulling");
		}
		else
		{
			pullingTrigger = false;
			//Debug.Log("NOT pulling");
		}

	}

	void Fire(Ray ray)
	{
		if(ammunationSettings.clipAmmo <=0 || resittngCatridge || !weaponsettings.bulletSpawn) {return;}

		RaycastHit hit;
		Transform bulletSpawn = weaponsettings.bulletSpawn;
		Vector3 bulletSpawnPos = bulletSpawn.position;
		Vector3 dir =ray.GetPoint(weaponsettings.butterRange);

		dir += (Vector3)Random.insideUnitCircle * weaponsettings.butterSpread;
		if(Physics.Raycast(bulletSpawnPos, dir, out hit, weaponsettings.butterRange, weaponsettings.BulletLayer))
		{	
			#region Damaging the Enemy
			if(hit.collider.gameObject.CompareTag("aj") || hit.collider.gameObject.CompareTag("pearl"))
			{
				enemy_Refrence = hit.collider.gameObject;
			}
			#endregion

			#region muzzleFlash
			if(weaponsettings.muzzleFlash)
			{
			Vector3 bullelspawnpos = weaponsettings.bulletSpawn.position;
			GameObject muzzle = Instantiate(weaponsettings.muzzleFlash, bullelspawnpos, Quaternion.identity) as GameObject;
			Transform muzzleT = muzzle.transform;
			muzzleT.SetParent(weaponsettings.bulletSpawn);
			Destroy(muzzle, 1.0f);
			}
		#endregion

			ammunationSettings.clipAmmo--;
			resittngCatridge = true;
			StartCoroutine(LoadNextBullet());
		}
	}

	
	IEnumerator Reload()
	{	
		yield return new WaitForSeconds(weaponsettings.reloadDuration);
		
		weaponsettings.reloadIndicator.SetActive(false);

		int ammoNeeded = ammunationSettings.maxClipAmmo - ammunationSettings.clipAmmo;
		if(ammunationSettings.carryingAmmo <= ammoNeeded)
		{
			ammunationSettings.clipAmmo = ammunationSettings.carryingAmmo;
			ammunationSettings.carryingAmmo = 0;
		}
		else
		{
			ammunationSettings.clipAmmo = ammunationSettings.maxClipAmmo;
			ammunationSettings.carryingAmmo -= ammoNeeded;
		}
	}

	public GameObject Pass_Enemy()
	{	
		return enemy_Refrence;
	}


	void Shoot ()
    {
		if(ammunationSettings.clipAmmo <=0 || resittngCatridge || !weaponsettings.bulletSpawn) {return;}

        shootRay1.origin = transform.position;
        shootRay1.direction = transform.forward;
        if(Physics.Raycast (shootRay1, out shootHit, weaponsettings.butterRange, weaponsettings.BulletLayer))
        {
			Debug.Log("shoot");

			#region Damaging the Enemy
			if(shootHit.collider.gameObject.CompareTag("cube") || shootHit.collider.gameObject.CompareTag("cube"))
			{
				enemy_Refrence = shootHit.collider.gameObject;
			}
			#endregion

			ammunationSettings.clipAmmo--;
			resittngCatridge = true;
			StartCoroutine(LoadNextBullet());
        }
    }


	IEnumerator LoadNextBullet(){
		yield return new WaitForSeconds(weaponsettings.FireRate);
		resittngCatridge = false;
	}


}
