﻿using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public int updateWeapon;

	public Sprite swordSprite;
	public Sprite rifleSprite;
	public Sprite shotgunSprite;
	public Sprite lancerRifleSprite;
	public Sprite plasmaGatlingSprite;
	public Sprite twinBlasterSprite;
	public Sprite armageddonSprite;
	public Sprite autoShotgunSprite;
	public Sprite confluxShotSprite;
	public Sprite omegaSwordSprite;

	public Player_Stats stats;

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
				if ( stats.weapon_by_type(WEAPON_TYPE.sword).upgrade_state() == 0 )
					GetComponent<SpriteRenderer>().sprite = swordSprite;
				else
					GetComponent<SpriteRenderer>().sprite = omegaSwordSprite;
					break;

				case (int)weapons.PlasmaRifle:
				if ( stats.weapon_by_type(WEAPON_TYPE.rifle).upgrade_state() == 0 )
					GetComponent<SpriteRenderer>().sprite = rifleSprite;
				else if ( stats.weapon_by_type(WEAPON_TYPE.rifle).upgrade_state()  == 1 )
					GetComponent<SpriteRenderer>().sprite = lancerRifleSprite;
				else if ( stats.weapon_by_type(WEAPON_TYPE.rifle).upgrade_state()  == 2 )
					GetComponent<SpriteRenderer>().sprite = plasmaGatlingSprite;
				else
					GetComponent<SpriteRenderer>().sprite = twinBlasterSprite;
					break;

				case (int)weapons.Shotgun:
				if ( stats.weapon_by_type(WEAPON_TYPE.shotgun).upgrade_state() == 0 )
					GetComponent<SpriteRenderer>().sprite = shotgunSprite;
				else if ( stats.weapon_by_type(WEAPON_TYPE.shotgun).upgrade_state() == 1 )
					GetComponent<SpriteRenderer>().sprite = armageddonSprite;
				else if ( stats.weapon_by_type(WEAPON_TYPE.shotgun).upgrade_state() == 2 )
					GetComponent<SpriteRenderer>().sprite = autoShotgunSprite;
				else
					GetComponent<SpriteRenderer>().sprite = confluxShotSprite;
					break;
			}

			updateWeapon = -1;
		}

	}
}
