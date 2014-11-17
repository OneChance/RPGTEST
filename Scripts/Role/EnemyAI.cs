using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
		public float moveSpeed = 0.8f;
		public float runSpeed = 1.3f;
		public float rotateSpeed = 20f;
		public float attackRange = 2.5f;
		public int attackPower = 10;
		public float attackInterval = 2f;
		public float patrolRandomDistance = 10f;
		public float patrolStandTime = 3f;
		private float attackTimer;
		private Vector3 oriPosition; //初始位置
		private bool findPlayer;
		private GameObject player;
		private Health playerHealth;
		private Transform myTransform;
		private float minDistance = 2f;
		private float myHeight;
		private float playerHeight;
		private bool patroling = false;
		private Vector3 newPos; //新的巡逻位置
		private float patrolTimer;
		private bool missPlayer = false;
		private Animator anim;
		private HashIDs hash;
		private float posAdjustSpeed = 1.5f;
		private float posDeviation = 1.2f;

		void Awake ()
		{
				player = GameObject.FindGameObjectWithTag (Tags.player);
				myTransform = transform;
				playerHealth = player.GetComponent<Health> ();
				myHeight = GameObject.Find ("ratkin_body").renderer.bounds.size.y;
				playerHeight = GameObject.Find ("Skeletonl_base").renderer.bounds.size.y;
				oriPosition = myTransform.position;
				newPos = oriPosition;
				patrolTimer = patrolStandTime;
				anim = GetComponent<Animator> ();
				hash = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<HashIDs> ();
				rigidbody.freezeRotation = true;
		}

		void Update ()
		{	
				if (attackTimer > 0) {
						attackTimer -= Time.deltaTime;
				}	

				if (findPlayer) {				
						if (Vector3.Distance (player.transform.position, myTransform.position) <= attackRange) {
								Attack ();
						} else {
								moveToPlayer ();
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

		void Move (Vector3 targetPos, bool changeToFoot, float distanceRestrict, float speed)
		{
				Vector3 curPos = myTransform.position;

				bool moveable = false;
				if (distanceRestrict > 0f) {
						if (Vector3.Distance (targetPos, curPos) > distanceRestrict) {
								moveable = true;
						}
				} else {
						if (Mathf.Abs (targetPos.x - curPos.x) > posDeviation || Mathf.Abs (targetPos.z - curPos.z) > posDeviation) {
								moveable = true;
						}
				}

				if (moveable) {

						if (changeToFoot) {
								targetPos = TransformUtil.getFootPos (targetPos, playerHeight);
								curPos = TransformUtil.getFootPos (curPos, myHeight);
						}
			
						myTransform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (targetPos - curPos), rotateSpeed * Time.deltaTime);
						myTransform.Translate (Vector3.forward * speed * 2 * Time.deltaTime);
						anim.SetFloat (hash.speedFloat, speed);
						//Debug.DrawLine (myTransform.position, targetPos, Color.red);
				} else {
						anim.SetFloat (hash.speedFloat, 0f);
						patroling = false;
						patrolTimer += Time.deltaTime;
				}
		}

		void BackToPos ()
		{
				Move (oriPosition, false, 0f, runSpeed);
		}

		void moveToPlayer ()
		{
				Move (player.transform.position, true, minDistance, runSpeed);
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
				}
				
				if (myTransform.position.y != newPos.y) {
						newPos = new Vector3 (newPos.x, Mathf.Lerp (newPos.y, myTransform.position.y, posAdjustSpeed * Time.deltaTime), newPos.z);
				}
				Move (newPos, false, 0f, moveSpeed);
		}
}
