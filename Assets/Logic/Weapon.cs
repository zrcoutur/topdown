using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public int updateWeapon;

	public Sprite swordSprite;
	public Sprite rifleSprite;
	public Sprite shotgunSprite;

	// Use this for initialization
	void Start () {
		updateWeapon = -1;

		GetComponent<SpriteRenderer>().sprite = swordSprite;
	}
	
	// Update is called once per frame
	void Update () {

		if (updateWeapon >= 0) {
			
			switch (updateWeapon) {
				case (int)weapons.BeamSword:
				GetComponent<SpriteRenderer>().sprite = swordSprite;
					break;
				case (int)weapons.PlasmaRifle:
				GetComponent<SpriteRenderer>().sprite = rifleSprite;
					break;
				case (int)weapons.Shotgun:
				GetComponent<SpriteRenderer>().sprite = shotgunSprite;
					break;
			}

			updateWeapon = -1;
		}

	}
}
