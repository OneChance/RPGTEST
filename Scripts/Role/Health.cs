using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
		public int maxHealth = 100;
		public int curHealth = 100;

		public void ChangeHealth (int value)
		{
				curHealth = Mathf.Max (0, curHealth + value);
				curHealth = Mathf.Min (curHealth, maxHealth);
		}

		void Update ()
		{
				if (curHealth <= 0) {
						if (gameObject.tag == Tags.player) {
								gameObject.GetComponent<PlayerAction> ().enabled = false;
						} else {
								//gameObject.GetComponent<EnemyAI> ().enabled = false;
								Destroy (gameObject);
						}
				}
		}
}
