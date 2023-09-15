using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

/// <summary>
/// Ustawienia sklepu
/// </summary>
public class UIShop : MonoBehaviour
{
    /// <summary>
    /// Wskaźnik do skryptu weaponSystem z systemem broni
    /// </summary>
    public WeaponSystem weaponSystem;

    /// <summary>
    /// Obiekt sklepu na mapie
    /// </summary>
    public GameObject shop;

    /// <summary>
    /// Pole tekstowe z komunikatami sklepu
    /// </summary>
    public Text shopAlerts;

    /// <summary>
    /// Pole tekstowe z komunikatami interfejsu uzytkownika
    /// </summary>
    public Text hudAlerts;

    /// <summary>
    /// Obiekt gracza na scenie
    /// </summary>
    public GameObject player;

    /// <summary>
    /// Okno sklepu w interfejsie użytkownika
    /// </summary>
    public GameObject canvas;

    /// <summary>
    /// Przycisk do zakupu karabinu
    /// </summary>
    public GameObject assaultButton;

    /// <summary>
    /// Przycisk do zakupu strzelby
    /// </summary>
    public GameObject shotgunButton;

    /// <summary>
    /// Przycisk do zakupu amunicji do strzelby
    /// </summary>
    public GameObject shotgunAmmoButton;

    /// <summary>
    /// Przycisk do zakupu amunicji do karabinu
    /// </summary>
    public GameObject assaultAmmoButton;

    /// <summary>
    /// Przycisk do zakupu amunicji granatu zapalajacego
    /// </summary>
    public GameObject molotovButton;

    /// <summary>
    /// Zmienna informujaca czy fala przeciwnikow jest aktywna
    /// </summary>
    public bool wave = false;

    /// <summary>
    /// Zmienna informujaca czy okno sklepu jest aktywne
    /// </summary>
    bool shopActive;

    /// <summary>
    /// Wskaznik do skryptu gracza
    /// </summary>
    public Player gracz;

    /// <summary>
    /// Komponent źródła dźwięku
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Efekt dźwiękowy zakupu broni
    /// </summary>
    public AudioClip BuyWeaponClip;

    /// <summary>
    /// Efekt dźwiękowy zakupu amunicji
    /// </summary>
    public AudioClip BuyAmmoClip;

    /// <summary>
    /// Efekt dźwiękowy otwarcia sklepu
    /// </summary>
    public AudioClip OpenShopClip;

    /// <summary>
    /// Funkcja inicjalizujaca sklep - ustawia odpowiednie zmienne, oraz dezaktywuje okno sklepu w interfejsie uzytkownika
    /// </summary>
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

    /// <summary>
    /// Funkcja, ktora sprawdza odleglosc gracza od sklepu i aktywuje interakcje
    /// </summary>
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

    /// <summary>
    /// Funkcja sluzaca do zakupu karabinu w sklepie
    /// </summary>
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

    /// <summary>
    /// Funkcja sluzaca do zakupu strzelby w sklepie
    /// </summary>
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

    /// <summary>
    /// Funkcja sluzaca do zakupu amunicji do strzelby w sklepie
    /// </summary>
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

        Invoke("ResetShopAlerts", 2.0f);
    }

    /// <summary>
    /// Funkcja sluzaca do zakupu amunicji do karabinu w sklepie
    /// </summary>
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

        Invoke("ResetShopAlerts", 2.0f);
    }

    /// <summary>
    /// Funkcja sluzaca do zakupu granata zapalajacego w sklepie
    /// </summary>
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

        Invoke("ResetShopAlerts", 2.0f);
    }

    /// <summary>
    /// Funkcja resetujaca pole tekstowe z komunikatami
    /// </summary>
    void ResetShopAlerts()
    {
        shopAlerts.text = "";
    }

    /// <summary>
    /// Funkcja odtwarzająca efekt dźwiękowy zakupu broni
    /// </summary>
    /// <param name="audioSource"> Źródło dźwięku </param>
    public void PlayBuyWeaponClip(AudioSource audioSource) {
        if(BuyWeaponClip != null) {
            audioSource.PlayOneShot(BuyWeaponClip);
        }
    }

    /// <summary>
    /// Funkcja odtwarzająca efekt dźwiękowy zakupu amunicji
    /// </summary>
    /// <param name="audioSource"> Źródło dźwięku </param>
    public void PlayBuyAmmoClip(AudioSource audioSource) {
        if(BuyAmmoClip != null) {
            audioSource.PlayOneShot(BuyAmmoClip, 0.5f);
        }
    }

    /// <summary>
    /// Funkcja odtwarzająca efekt dźwiękowy otwarcia sklepu
    /// </summary>
    /// <param name="audioSource"> Źródło dźwięku </param>
    public void PlayOpenShopClip(AudioSource audioSource) {
        if(OpenShopClip != null) {
            audioSource.PlayOneShot(OpenShopClip);
        }
    }
}