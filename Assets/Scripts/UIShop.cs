using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIShop : MonoBehaviour
{
	public WeaponSystem weaponSystem;

	public GameObject shop;
	public Text shopAlerts;
	public Text hudAlerts;

	public GameObject player;

	public GameObject canvas;
	public GameObject assaultButton;
	public GameObject shotgunButton;

	public bool wave = false;
	bool shopActive;

	void Start()
	{
		canvas.SetActive(false);
		Button assault = assaultButton.GetComponent<Button>();
		Button shotgun = shotgunButton.GetComponent<Button>();

		assault.onClick.AddListener(buyAssault);
		shotgun.onClick.AddListener(buyShotgun);
	}

	void Update()
    {
		float distance = Vector3.Distance(player.transform.position, shop.transform.position);
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
		if(!weaponSystem.weaponLock[1] && weaponSystem.playerPoints >= 30)
        {
			weaponSystem.weaponLock[1] = true;
			weaponSystem.playerPoints -= 30;
			shopAlerts.text = "You bought assault gun!";

		} else if(weaponSystem.weaponLock[1])
        {
			shopAlerts.text = "You already bought this weapon!";
			
		} else if (weaponSystem.playerPoints < 30)
        {
			shopAlerts.text = "You don't have enough money!";
		}
		Invoke("ResetShopAlerts", 2.0f);

	}

	void buyShotgun()
	{
		if (!weaponSystem.weaponLock[2] && weaponSystem.playerPoints >= 15)
		{
			weaponSystem.weaponLock[2] = true;
			weaponSystem.playerPoints -= 15;
			shopAlerts.text = "You bought shotgun!";
		}
		else if (weaponSystem.weaponLock[2])
		{
			shopAlerts.text = "You already bought this weapon!";

		}
		else if (weaponSystem.playerPoints < 15)
		{
			shopAlerts.text = "You don't have enough money!";
		}

		Invoke("ResetShopAlerts", 2.0f);

	}

	void ResetShopAlerts()
    {
		shopAlerts.text = "";
    }

	

}