using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{

		public GameObject selectedEnemy;
		public float attackRange = 2.2f;
		public float attackInterval = 1f;
		public int attackPower = 10;
		private Transform myTransform;
		private float attackTimer;
		public List<GameObject> enemys;

		void Awake ()
		{
				myTransform = transform;
				GetEnemy ();
		}

		void Update ()
		{
				if (attackTimer > 0) {
						attackTimer -= Time.deltaTime;
				}		
				
				if (Input.GetKeyDown (KeyCode.Tab)) {
						SelecteEnemy ();
				}

				if (Input.GetButtonDown ("Fire1")) {
						if (selectedEnemy != null) {
								if (Vector3.Distance (selectedEnemy.transform.position, myTransform.position) <= attackRange) {
										Attack ();
								}
						}
				}
		}

		void SelecteEnemy ()
		{
				int index = 0;
				if (selectedEnemy != null) {
						selectedEnemy.layer = Layers.defaultLayer;
						index = enemys.IndexOf (selectedEnemy) + 1;
				}
				
				if (index == enemys.Count) {
						index = 0;
				}

				selectedEnemy = enemys [index];
				selectedEnemy.layer = Layers.outlineLayer;
		}

		void GetEnemy ()
		{
				enemys = new List<GameObject> ();
				GameObject[] enemyArray = GameObject.FindGameObjectsWithTag (Tags.enemy);
				foreach (GameObject e in enemyArray) {
						enemys.Add (e);
				}
				//按距离排序
				enemys.Sort (delegate (GameObject go1,GameObject go2) {
						return Vector3.Distance (myTransform.position, go1.transform.position).CompareTo (Vector3.Distance (myTransform.position, go2.transform.position));
				});
		}

		void Attack ()
		{			
				Health enemyHealth = selectedEnemy.GetComponent<Health> ();
				if (attackTimer <= 0) {
						if (Vector3.Dot (myTransform.forward, selectedEnemy.transform.forward) < 0) {
								enemyHealth.ChangeHealth (0 - attackPower);
								attackTimer = attackInterval;
						} else {
								Debug.Log ("Enemy is on your back!!!");
						}
				}
		}
}
