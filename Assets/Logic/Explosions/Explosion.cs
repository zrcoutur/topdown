using UnityEngine;
using System.Collections;

/**
 * A class designed to hold some values necessary for simulating an explosion.
 * 
 * last modified by : Joshua Hooker
 * 31 March 2016
 */
public abstract class Explosion : MonoBehaviour {
	// Explosion SFX
	public AudioClip X_Explode;
	protected float animDone = 0.45f;

	private int damage = 0;

	/* Set the damage of the explosion. */
	public void setDamage(int dmg) { damage = dmg; }
	/* Return the damage of the explosion. */
	public int getDamage() { return damage; }
}
