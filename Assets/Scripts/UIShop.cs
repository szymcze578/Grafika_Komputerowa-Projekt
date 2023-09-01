using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class UIShop : MonoBehaviour
{
	public WeaponSystem weaponSystem;

	public GameObject shop;

	public Text shopAlerts;
	public Text hudAlerts;

	public int gunInShop;

	public GameObject player;

	public GameObject canvas;
	public GameObject assaultButton;
	public GameObject shotgunButton;

    public GameObject shotgunAmmoButton;
    public GameObject assaultAmmoButton;

    public GameObject molotovButton;

    public bool wave = false;
	bool shopActive;

	public Player gracz;

    private AudioSource audioSource;
    public AudioClip BuyWeaponClip;
    public AudioClip BuyAmmoClip;
    public AudioClip OpenShopClip;

	void Start()
	{
		canvas.SetActive(false);
        Button shotgun = shotgunButton.GetComponent<Button>();
        Button assault = assaultButton.GetComponent<Button>();
        Button shotgunAmmo = shotgunAmmoButton.GetComponent<Button>();
        Button assaultAmmo = assaultAmmoButton.GetComponent<Button>();
        Button molotov = molotovButton.GetComponent<Button>();

        gracz = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        assault.onClick.AddListener(buyAssault);
		shotgun.onClick.AddListener(buyShotgun);
        shotgunAmmo.onClick.AddListener(buyShotgunAmmo);
        assaultAmmo.onClick.AddListener(buyAssaultAmmo);
        molotov.onClick.AddListener(buyMolotov);

        audioSource = gameObject.GetComponent<AudioSource>();
    }

	void Update()
    {
		float distance = Vector3.Distance(player.transform.position, shop.transform.position);
		//Debug.Log(gunInShop + " " + distance);

        if (wave == false && distance < 2.0f)
        {
			if (!shopActive)
				hudAlerts.text = "Press E to enter the shop";

			if (Input.GetKeyDown(KeyCode.E))
            {
				shopActive = true;
				hudAlerts.text = "";
				canvas.SetActive(true);
				weaponSystem.blockShooting = false;
                PlayOpenShopClip(audioSource);
			}

			if (canvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
			{
				shopActive = false;
				canvas.SetActive(false);
				weaponSystem.blockShooting = true;
			}

		}
		else
        {
			shopActive = false;
			hudAlerts.text = "";
			canvas.SetActive(false);
			weaponSystem.blockShooting = true;
		}

    }

	void buyAssault()
	{
		if(!weaponSystem.weaponLock[1] && gracz.points >= 30)
        {
			weaponSystem.weaponLock[1] = true;
			weaponSystem.magazinesLeft[1] = 1;
			gracz.points -= 30;
            shopAlerts.color = Color.green;
            shopAlerts.text = "You bought assault gun!";
            PlayBuyWeaponClip(audioSource);

		} else if(weaponSystem.weaponLock[1])
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You already bought this weapon!";
			
		} else if (gracz.points < 30)
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";
		}
		Invoke("ResetShopAlerts", 2.0f);

	}

	void buyShotgun()
	{
		if (!weaponSystem.weaponLock[2] && gracz.points >= 15)
		{
			weaponSystem.weaponLock[2] = true;
			weaponSystem.magazinesLeft[2] = 1;
			gracz.points -= 15;
            shopAlerts.color = Color.green;
            shopAlerts.text = "You bought shotgun!";
            PlayBuyWeaponClip(audioSource);
		}
		else if (weaponSystem.weaponLock[2])
		{
            shopAlerts.color = Color.red;
            shopAlerts.text = "You already bought this weapon!";
		}
		else if (gracz.points < 15)
		{
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";
		}

		Invoke("ResetShopAlerts", 2.0f);

	}

    void buyShotgunAmmo()
    {
        if (weaponSystem.weaponLock[2] && gracz.points >= 15)
        {
            weaponSystem.magazinesLeft[2] += 1;
            gracz.points -= 15;
            shopAlerts.color = Color.green;
            shopAlerts.text = "You bought ammo to shotgun!";
            PlayBuyAmmoClip(audioSource);
        }
        else if(!weaponSystem.weaponLock[2])
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have shotgun yet!";
        }
		else if (gracz.points < 15)
		{
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";
        }
    }

    void buyAssaultAmmo()
    {
        if (weaponSystem.weaponLock[1] && gracz.points >= 25)
        {
            weaponSystem.magazinesLeft[1] += 1;
            gracz.points -= 25;
			shopAlerts.color = Color.green;
            shopAlerts.text = "You bought ammo to assault!";
            PlayBuyAmmoClip(audioSource);
        }
        else if (!weaponSystem.weaponLock[2])
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have assault yet!";
        }
        else if (gracz.points < 25)
        {
			shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";
        }
    }

    void buyMolotov()
    {
        if (gracz.points >= 50)
        {
            weaponSystem.bulletsLeft[3] += 2;
            gracz.points -= 50;
            shopAlerts.color = Color.green;
            shopAlerts.text = "You bought two molotovs!";
            PlayBuyAmmoClip(audioSource);
        } 
        else if (gracz.points < 50)
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";
        }
    }


    void ResetShopAlerts()
    {
		shopAlerts.text = "";
    }

    public void PlayBuyWeaponClip(AudioSource audioSource) {
        if(BuyWeaponClip != null) {
            audioSource.PlayOneShot(BuyWeaponClip);
        }
    }

    public void PlayBuyAmmoClip(AudioSource audioSource) {
        if(BuyAmmoClip != null) {
            audioSource.PlayOneShot(BuyAmmoClip, 0.5f);
        }
    }

    public void PlayOpenShopClip(AudioSource audioSource) {
        if(OpenShopClip != null) {
            audioSource.PlayOneShot(OpenShopClip);
        }
    }
}