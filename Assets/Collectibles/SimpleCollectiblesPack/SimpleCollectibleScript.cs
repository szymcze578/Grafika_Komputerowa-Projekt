using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	public enum CollectibleTypes {NoType, Type1, Type2, Type3, Type4, Type5}; // you can replace this with your own labels for the types of collectibles in your game!

	public CollectibleTypes CollectibleType; // this gameObject's type

	public bool rotate; // do you want it to rotate?

	public float rotationSpeed;

	public AudioClip collectSound;

	public GameObject collectEffect;

	public Player player;
	public WeaponSystem weaponSystem;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		weaponSystem = GameObject.Find("GunHolder").GetComponent<WeaponSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (rotate)
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter(Collider other)
	{     
        if (other.tag=="Player"){
			Collect ();
		}
	}

	public void Collect()
	{
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);

		//Below is space to add in your code for what happens based on the collectible type

		if (CollectibleType == CollectibleTypes.NoType) {

			//Add in code here;
			Debug.Log ("Do NoType Command");
		}
		if (CollectibleType == CollectibleTypes.Type1) {

			//Bandages
			if (player.increaseHealth(20)) {
                Destroy(gameObject);
                Debug.Log("Health increased by 20");
            }
			
		}
		if (CollectibleType == CollectibleTypes.Type2) {

			//50$ of cash
			player.points += 50;
			Debug.Log ("Cash increased by 50");
            Destroy(gameObject);
        }
		if (CollectibleType == CollectibleTypes.Type3) {

			//100$ of cash;
			player.points += 100;
            Destroy(gameObject);
        }
		if (CollectibleType == CollectibleTypes.Type4) {

			weaponSystem.magazinesLeft[weaponSystem.selectedWeapon - 1] += 1;
			Debug.Log ("Magazin added");
            Destroy(gameObject);

        }
		if (CollectibleType == CollectibleTypes.Type5) {

			//Add in code here;

			Debug.Log ("Do NoType Command");
		}


	}
}
