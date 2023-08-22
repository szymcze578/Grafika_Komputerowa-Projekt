using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Audio Config", menuName = "ScriptableObject/Weapon Audio Config")]
public class WeaponAudioConfig : ScriptableObject
{
    [Range(0, 1f)]
   public float Volume = 1f;
   public AudioClip[] ShootingClip;
   public AudioClip ReloadingClip;
   public AudioClip EmptyClip;

   public AudioClip EquipGun;



    public void PlayShootingClip(AudioSource audioSource) {
        audioSource.PlayOneShot(ShootingClip[Random.Range(0, ShootingClip.Length)], Volume);
    }

    public void PlayReloadingClip(AudioSource audioSource) {
        if(ReloadingClip != null) {
            audioSource.PlayOneShot(ReloadingClip, Volume);
        }
    }

    public void PlayOutOfAmmoClip(AudioSource audioSource) {
        if(EmptyClip != null) {
            audioSource.PlayOneShot(EmptyClip, Volume);
        }
    }

    public void PlayEquipGunClip(AudioSource audioSource) {
        if(EmptyClip != null) {
            audioSource.PlayOneShot(EquipGun, Volume);
        }
    }
}
