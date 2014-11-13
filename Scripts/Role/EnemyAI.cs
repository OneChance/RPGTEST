using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
		public float moveSpeed = 2f;
		public float rotateSpeed = 20f;
		public float attackRange = 2.2f;
		public int attackPower = 10;
		public float attackInterval = 2f;
		public float patrolRandomDistance = 10f;
		public float patrolStandTime = 2f;
		private float attackTimer;
		private Vector3 oriPosition; //初始位置
		private bool findPlayer;
		private GameObject player;
		private Health playerHealth;
		private Transform myTransform;
		private float minDistance = 1.6f;
		private float myHeight;
		private float playerHeight;
		private bool patroling = false;
		private Vector3 newPos; //新的巡逻位置
		private float patrolTimer;
		private bool missPlayer = false;

		void Awake ()
		{
				player = GameObject.FindGameObjectWithTag (Tags.player);
				myTransform = transform;
				playerHealth = player.GetComponent<Health> ();
				myHeight = GameObject.Find ("ratkin_body").renderer.bounds.size.y;
				playerHeight = GameObject.Find ("Graphics").renderer.bounds.size.y;
				oriPosition = myTransform.position;
				newPos = oriPosition;
				patrolTimer = patrolStandTime;
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
						if (myTransform.position != oriPosition && missPlayer) {
								BackToPos ();
						} else {
								Patrol ();
						}				
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
						missPlayer = true;
				}
		}

		void Move (Vector3 targetPos, bool changeToFoot, float distanceRestrict)
		{

				Vector3 curPos = myTransform.position;

				bool moveable = false;
				if (distanceRestrict > 0f) {
						if (Vector3.Distance (targetPos, curPos) > distanceRestrict) {
								moveable = true;
						}
				} else {
						if (Mathf.Abs (targetPos.x - curPos.x) > 1 || Mathf.Abs (targetPos.z - curPos.z) > 1) {
								moveable = true;
						}
				}

				if (moveable) {
						if (changeToFoot) {
								targetPos = TransformUtil.getFootPos (targetPos, playerHeight);
								curPos = TransformUtil.getFootPos (curPos, myHeight);
						}
			
						transform.rotation = Quaternion.Lerp (myTransform.rotation, Quaternion.LookRotation (targetPos - curPos), rotateSpeed * Time.deltaTime);
						myTransform.Translate (Vector3.forward * moveSpeed * 2 * Time.deltaTime);
				} else {
						patroling = false;
						patrolTimer += Time.deltaTime;
				}
		}

		void BackToPos ()
		{
				Move (oriPosition, false, 0f);
		}

		void moveToPlayer ()
		{
				Move (player.transform.position, true, minDistance);
		}

		void OnTriggerEnter (Collider other)
		{
				if (other.gameObject == player && playerHealth.curHealth > 0) {
						findPlayer = true;
				}
		}

		void generatePatrolPos ()
		{
				//以初始地点为中心，生成一个随机位置
				//随机X
				float randomX = Random.Range (oriPosition.x - patrolRandomDistance, oriPosition.x + patrolRandomDistance);
				float randomZ = Random.Range (oriPosition.z - patrolRandomDistance, oriPosition.z + patrolRandomDistance);
				newPos = new Vector3 (randomX, oriPosition.y, randomZ);
		}

		void Patrol ()
		{	
				if (!patroling && patrolTimer >= patrolStandTime) {
						Vector3 curPos = myTransform.position;
						while (Vector3.Distance(curPos,newPos)<5f) {
								generatePatrolPos ();
						}
						patroling = true;	
						patrolTimer = 0f;
						Debug.Log ("new pos");
				}
				Move (newPos, false, 0f);
		}
}
