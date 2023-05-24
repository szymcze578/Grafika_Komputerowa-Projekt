using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIShop : MonoBehaviour
{
	public WeaponSystem weaponSystem;

	public GameObject shop;
	public Text shopAlerts;

	public GameObject player;

	public GameObject canvas;
	public GameObject assaultButton;
	public GameObject shotgunButton;

	public bool wave = false;

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
			canvas.SetActive(true);
			weaponSystem.blockShooting = false;
		}
		else
        {
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