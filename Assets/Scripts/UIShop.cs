using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIShop : MonoBehaviour
{
	public WeaponSystem weaponSystem;

	public GameObject shop;
	public GameObject player;

	public GameObject canvas;
	public GameObject assaultButton;
	public GameObject shotgunButton;

	public bool wave = false;

	void Start()
	{
		canvas.SetActive(false);
		Button assault = assaultButton.GetComponent<Button>();
		Button shotgun = assaultButton.GetComponent<Button>();

		assault.onClick.AddListener(buyAssault);
		shotgun.onClick.AddListener(buyShotgun);
	}

	void Update()
    {
		float distance = Vector3.Distance(player.transform.position, shop.transform.position);
		if (wave == false && distance < 2.0f)
			canvas.SetActive(true);
		else
			canvas.SetActive(false);

	}

	void buyAssault()
	{
		if(weaponSystem.weaponLock[1] == false && weaponSystem.playerPoints >= 30)
        {
			weaponSystem.weaponLock[1] = true;
			weaponSystem.playerPoints -= 30;
        }
	}

	void buyShotgun()
	{
		if (weaponSystem.weaponLock[2] == false && weaponSystem.playerPoints >= 15)
		{
			weaponSystem.weaponLock[2] = true;
			weaponSystem.playerPoints -= 15;
		}
	}
}