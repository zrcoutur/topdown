using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	protected int damage;
	protected float duration = float.MaxValue;
	public Vector2 hitImpulse;

	// Respective getters and setters for damage and duration of the attack
	public void setDamage(int dmg) { damage = dmg; }
	public int getDamage() { return damage; }
	public void setDuration(float length) { duration = length; }
	public float getDuration() { return duration; }
}
