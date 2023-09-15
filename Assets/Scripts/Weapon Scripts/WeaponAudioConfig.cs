using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Konfigurator efektów dźwiękowych broni
/// </summary>
[CreateAssetMenu(fileName = "Weapon Audio Config", menuName = "ScriptableObject/Weapon Audio Config")]
public class WeaponAudioConfig : ScriptableObject
{
    /// <summary>
    /// Głośność efektów dźwiękowych
    /// </summary>
    [Range(0, 1f)]
   public float Volume = 1f;
   
   /// <summary>
   /// Zmiennna tablicowa z efektami dźwiękowymi wystrzału broni
   /// </summary>
   public AudioClip[] ShootingClip;

   /// <summary>
   /// Efekt dźwiekowy przeładowania broni
   /// </summary>
   public AudioClip ReloadingClip;

   /// <summary>
   /// Efekt dźwiękowy pustego magazynka
   /// </summary>
   public AudioClip EmptyClip;

    /// <summary>
    /// Efekt dźwiękowy wyciągnięcia broni
    /// </summary>
   public AudioClip EquipGun;

    /// <summary>
    /// Metoda odtwarza losowy efekt dźwiękowy wystrzału z tablicy ShootingClip
    /// </summary>
    /// <param name="audioSource"> Źródło dźwięku </param>
    public void PlayShootingClip(AudioSource audioSource) {
        audioSource.PlayOneShot(ShootingClip[Random.Range(0, ShootingClip.Length)], Volume);
    }

    /// <summary>
    /// Metoda odtwarza efekt dźwiękowy przeładowania
    /// </summary>
    /// <param name="audioSource"> Źródło dźwięku </param>
    public void PlayReloadingClip(AudioSource audioSource) {
        if(ReloadingClip != null) {
            audioSource.PlayOneShot(ReloadingClip, Volume);
        }
    }

    /// <summary>
    /// Metoda odtwarza efekt dźwiękowy pustego magazynka
    /// </summary>
    /// <param name="audioSource"> Źródło dźwięku </param>
    public void PlayOutOfAmmoClip(AudioSource audioSource) {
        if(EmptyClip != null) {
            audioSource.PlayOneShot(EmptyClip, Volume);
        }
    }

    /// <summary>
    /// Metoda odtwarza efekt dźwiękowy wyciągnięcia broni
    /// </summary>
    /// <param name="audioSource"> Źródło dźwięku </param>
    public void PlayEquipGunClip(AudioSource audioSource) {
        if(EmptyClip != null) {
            audioSource.PlayOneShot(EquipGun, Volume);
        }
    }
}
