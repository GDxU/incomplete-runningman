using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent (typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
	public int startingHealth = 100;                            // The amount of health the player starts the game with.
	public int currentHealth;                                   // The current health the player has.
	public Slider healthSlider;                                 // Reference to the UI's health bar.
	public AudioClip deathClip;                                 // The audio clip to play when the player dies.
	public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
	
	
	Animator anim;                                              // Reference to the Animator component.
	AudioSource playerAudio;                                    // Reference to the AudioSource component.
	bool isDead;                                                // Whether the player is dead.
	bool damaged;                                               // True when the player gets damaged.
	
	
	void Awake ()
	{
		playerAudio = GetComponent <AudioSource> ();
		// Set the initial health of the player.
		currentHealth = startingHealth;
	}
	
	
	void Update ()
	{

	}
	

	public void setVal(int h){
		healthSlider.value = h;
	}
	
	void Death ()
	{
		// Set the death flag so this function won't be called again.
		isDead = true;

		// Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
		playerAudio.clip = deathClip;
		playerAudio.Play ();

	}       
}