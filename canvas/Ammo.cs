using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ammo : MonoBehaviour {

	public TMP_Text currentAmmoT;
	public TMP_Text totoalammoT;
	Weapon_controller wc;
	int maxammo;
	int currentammo;
	int totalammo;

	public GameObject help;

	// Use this for initialization
	void Start () {
		wc = Weapon_controller.Instance;
		maxammo = wc.ammunationSettings.maxClipAmmo;
		currentammo = wc.ammunationSettings.clipAmmo;
		totalammo = wc.ammunationSettings.carryingAmmo;

		currentAmmoT.text = "Loaded Ammo: " + currentammo + " / " + maxammo;
		totoalammoT.text = "Total Ammo: " + totalammo;

		help.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0))
		{	
			maxammo = wc.ammunationSettings.maxClipAmmo;
			currentammo = wc.ammunationSettings.clipAmmo;
			totalammo = wc.ammunationSettings.carryingAmmo;
			currentAmmoT.text = "Loaded Ammo: " + currentammo + " / " + maxammo;
			totoalammoT.text = "Total Ammo: " + totalammo;
		}

		if(Input.GetKeyDown(KeyCode.R))
		{	
			StartCoroutine(Reload());
		}

		if(Input.GetKeyDown(KeyCode.H))
		{	
			 Help();
		}

		if(Input.GetKeyDown(KeyCode.K))
		{	
			ok();
		}

	}

	IEnumerator Reload()
	{	
		yield return new WaitForSeconds(2.5f);

			maxammo = wc.ammunationSettings.maxClipAmmo;
			currentammo = wc.ammunationSettings.clipAmmo;
			totalammo = wc.ammunationSettings.carryingAmmo;
			currentAmmoT.text = "Loaded Ammo: " + currentammo + " / " + maxammo;
			totoalammoT.text = "Total Ammo: " + totalammo;
	}


	public void Help()
	{
		help.SetActive(true);
	}

	public void ok()
	{
		help.SetActive(false);
	}

}
