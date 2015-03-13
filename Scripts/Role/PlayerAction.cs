using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : MonoBehaviour
{

		public GameObject selectedEnemy;
		public float attackRange = 2.2f;
		public float attackInterval = 1f;
		public int attackPower = 10;
		public bool run = false;
		private Transform myTransform;
		private float attackTimer;
		public List<GameObject> enemys;
		public float moveVerticalSpeed = 2f;
		public float moveHorizontalSpeed = 2f;
		public float runSpeed = 6f;
		private Animator anim;
		private HashIDs hash;
		private float h;
		private float v;

		void Awake ()
		{
				myTransform = transform;
				GetEnemy ();
				anim = GetComponent<Animator> ();
				hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();
		}

		void Update ()
		{

				if (attackTimer > 0) {
						attackTimer -= Time.deltaTime;
				}		
				
				if (Input.GetKeyDown (KeyCode.Tab)) {
						SelecteEnemy ();
				}

				if (Input.GetButtonDown ("RunSwitch")) {
						SwitchRunWalk ();
				}

				if (Input.GetButtonDown ("Jump")) {	
						Jump ();
				}

				if (Input.GetButtonDown ("Fire1")) {
						if (selectedEnemy != null) {
								if (Vector3.Distance (selectedEnemy.transform.position, myTransform.position) <= attackRange) {
										Attack ();
								}
						}
				}

				PlayerMovement ();
		}

		void SwitchRunWalk ()
		{
				run = !run;
				anim.SetBool (hash.runBool, run);
		}

		bool CheckGrounded ()
		{
				//检测是否在地面
				RaycastHit hit;
				if (Physics.Raycast (transform.position, -Vector3.up, out hit, 100.0F)) {
						float distanceToGround = hit.distance;
						if (distanceToGround < 0.1) {
								return true;
						}
				}

				return false;
		}

		void Jump ()
		{			
				if (CheckGrounded ()) {
						GetComponent<Rigidbody> ().AddForce (Vector3.up * 300);				

						if (Mathf.Abs (h) != 1 && Mathf.Abs (v) != 1) {
							anim.SetTrigger(hash.jumpTrigger);
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

		void PlayerMovement ()
		{
				h = Input.GetAxis ("Horizontal");
				v = Input.GetAxis ("Vertical");

				var newHSpeed = moveHorizontalSpeed;
				var newVSpeed = moveVerticalSpeed;


				if (Mathf.Abs (v) == 1f && Mathf.Abs (h) == 1f) {
						newHSpeed = moveHorizontalSpeed * 0.5f;
						newVSpeed = moveVerticalSpeed * 0.5f;
				}

				if (run) {
						newHSpeed *= 3;
						if (v > 0)
								newVSpeed *= 3;
				}
							
				transform.Translate (Vector3.forward * v * newVSpeed * Time.deltaTime);
				transform.Translate (Vector3.right * h * newHSpeed * Time.deltaTime);

				//未找到斜线走跑动画，融合效果不好，暂时屏蔽横向移动
				if (Mathf.Abs (v) == 1f) {
						h = 0f;
				}

				anim.SetFloat (hash.hSpeedFloat, h);
				anim.SetFloat (hash.vSpeedFloat, v);
				
		}
}
