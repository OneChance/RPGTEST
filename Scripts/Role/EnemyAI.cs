using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public float patrolSpeed = 0.8f;
	public float runSpeed = 1.3f;
	public float rotateSpeed = 20f;
	public float attackRange = 2.5f;
	public int attackPower = 10;
	public float attackInterval = 2f;
	private float attackTimer;
	private bool findPlayer;
	private GameObject player;
	private Health playerHealth;
	private Transform myTransform;
	private float minDistance = 2f;
	private NavMeshAgent nav;
	public Transform[] patrolWayPoints;
	private int wayPointIndex;
	private float patrolTimer;
	public float patrolWaitTime = 5f;

	void Awake ()
	{
		nav = GetComponent<NavMeshAgent> ();
		wayPointIndex = 0;
	}

	void Update ()
	{	
		Petrol ();
	}

	void Petrol ()
	{
		if (nav.remainingDistance < nav.stoppingDistance) {
			nav.speed = 0;
			patrolTimer += Time.deltaTime;
			
			if (patrolTimer >= patrolWaitTime) {
				if (wayPointIndex == patrolWayPoints.Length - 1) {
					wayPointIndex = 0;
				} else {
					wayPointIndex++;
				}
				patrolTimer = 0f;
			}
		} else {
			patrolTimer = 0f;
			nav.speed = patrolSpeed;
		}

		nav.destination = patrolWayPoints [wayPointIndex].position;
	}
}
