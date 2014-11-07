using UnityEngine;
using System.Collections;

public class ShowHealthBar : MonoBehaviour
{
		private float healthBarWidth;
		private float healthBarHeight = 20f;
		private GameObject player;
		private Health playHealth;
		public GameObject selectedEnemy;
		private Health enemyHealth;

		void Awake ()
		{
				healthBarWidth = Screen.width / 5; 
				player = GameObject.FindGameObjectWithTag ("Player");
				playHealth = player.GetComponent<Health> ();
		}
	
		void OnGUI ()
		{
				//player health
				GUI.Box (new Rect (10, 10, healthBarWidth * playHealth.curHealth / playHealth.maxHealth, healthBarHeight), playHealth.curHealth + "/" + playHealth.maxHealth);
				//enemy health
				if (selectedEnemy != null) {
						enemyHealth = selectedEnemy.GetComponent<Health> ();
						GUI.Box (new Rect (Screen.width / 2 - healthBarWidth / 2, 10, healthBarWidth * enemyHealth.curHealth / enemyHealth.maxHealth, healthBarHeight), enemyHealth.curHealth + "/" + enemyHealth.maxHealth);
				}
		}
}
