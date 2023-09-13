using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleCollectibleScript : MonoBehaviour {

	// Enum określający typy przedmiotów do zebrania
	public enum CollectibleTypes {NoType, Type1, Type2, Type3, Type4, Type5};

	// Zmienna określająca typ przedmiotu do zebrania.
	public CollectibleTypes CollectibleType; // this gameObject's type

	// Zmienna określająca czy przedmiot do zebrania ma się obracać
	public bool rotate;

	// Prędkość obrotu
	public float rotationSpeed;

	// Dźwięk obrotu
	public AudioClip collectSound;

	// 
	public GameObject collectEffect;

	// Wskaźnik na obiekt gracza
	public Player player;

    // Wskaźnik do skryptu weaponSystem
    public WeaponSystem weaponSystem;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		weaponSystem = GameObject.Find("GunHolder").GetComponent<WeaponSystem>();
    }
	
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

	// Funkcja wywoływana gdy zajedzie kolizja gracza z przedmiotem do podniesienia znajdującym się na mapie. W zależności od typu przedmiotu zostanie wykonana inna akcja.
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
