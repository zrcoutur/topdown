using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

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
		updateWeapon();
	}

	public void updateWeapon() {
		switch (stats.current_weapon()) {
		case WEAPON_TYPE.sword:
			if ( stats.weapon_by_type(WEAPON_TYPE.sword).upgrade_state() == 0 )
				GetComponent<SpriteRenderer>().sprite = swordSprite;
			else
				GetComponent<SpriteRenderer>().sprite = omegaSwordSprite;
			break;
		case WEAPON_TYPE.rifle:
			if ( stats.weapon_by_type(WEAPON_TYPE.rifle).upgrade_state() == 0 )
				GetComponent<SpriteRenderer>().sprite = rifleSprite;
			else if ( stats.weapon_by_type(WEAPON_TYPE.rifle).upgrade_state()  == 1 )
				GetComponent<SpriteRenderer>().sprite = lancerRifleSprite;
			else if ( stats.weapon_by_type(WEAPON_TYPE.rifle).upgrade_state()  == 2 )
				GetComponent<SpriteRenderer>().sprite = plasmaGatlingSprite;
			else
				GetComponent<SpriteRenderer>().sprite = twinBlasterSprite;
			break;
		case WEAPON_TYPE.shotgun:
			if ( stats.weapon_by_type(WEAPON_TYPE.shotgun).upgrade_state() == 0 )
				GetComponent<SpriteRenderer>().sprite = shotgunSprite;
			else if ( stats.weapon_by_type(WEAPON_TYPE.shotgun).upgrade_state() == 1 )
				GetComponent<SpriteRenderer>().sprite = armageddonSprite;
			else if ( stats.weapon_by_type(WEAPON_TYPE.shotgun).upgrade_state() == 2 )
				GetComponent<SpriteRenderer>().sprite = autoShotgunSprite;
			else
				GetComponent<SpriteRenderer>().sprite = confluxShotSprite;
			break;
		case WEAPON_TYPE.grenade:
			GetComponent<SpriteRenderer>().sprite = rifleSprite;
			break;
		}
	}
}
