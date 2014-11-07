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
}
