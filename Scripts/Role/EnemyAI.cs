using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
		public float moveSpeed = 5f;
		public float rotateSpeed = 20f;
		public float attackRange = 2.2f;
		public int attackPower = 10;
		public float attackInterval = 2f;
		private float attackTimer;
		private Vector3 oriPosition; //初始位置
		private bool findPlayer;
		private GameObject player;
		private Health playerHealth;
		private Transform myTransform;
		private float minDistance = 1.6f;
		private float myHeight;
		private float playerHeight;

		void Awake ()
		{
				player = GameObject.FindGameObjectWithTag (Tags.player);
				myTransform = transform;
				playerHealth = player.GetComponent<Health> ();
				myHeight = gameObject.renderer.bounds.size.y;
				playerHeight = GameObject.Find ("Graphics").renderer.bounds.size.y;
				oriPosition = myTransform.position;
		}

		void Update ()
		{
				if (attackTimer > 0) {
						attackTimer -= Time.deltaTime;
				}	

				if (findPlayer) {
						moveToPlayer ();
						if (Vector3.Distance (player.transform.position, myTransform.position) <= attackRange) {
								Attack ();
						}
				} else {
						if (myTransform.position != oriPosition) {
								BackToPos ();
						}					
				}
		}

		void BackToPos ()
		{
				//转向方向
				transform.rotation = Quaternion.Lerp (myTransform.rotation, Quaternion.LookRotation (oriPosition - myTransform.position), rotateSpeed * Time.deltaTime);
				//移动
				if (Vector3.Distance (oriPosition, myTransform.position) > 0.15) {
						myTransform.Translate (Vector3.forward * moveSpeed * 2 * Time.deltaTime);
				}
		}

		void Attack ()
		{
				if (attackTimer <= 0) {
						playerHealth.ChangeHealth (0 - attackPower);
						attackTimer = attackInterval;
				}

				if (playerHealth.curHealth == 0) {
						findPlayer = false;
				}
		}

		void moveToPlayer ()
		{
				//转向玩家方向
				transform.rotation = Quaternion.Lerp (myTransform.rotation, Quaternion.LookRotation (TransformUtil.getFootPos (player.transform.position, playerHeight) - TransformUtil.getFootPos (myTransform.position, myHeight)), rotateSpeed * Time.deltaTime);
				//移动
				if (Vector3.Distance (player.transform.position, myTransform.position) > minDistance) {
						myTransform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
				}
		}

		void OnTriggerEnter (Collider other)
		{
				if (other.gameObject == player && playerHealth.curHealth > 0) {
						findPlayer = true;
				}
		}
}
