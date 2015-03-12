using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour
{

		private NavMeshAgent nav;
		private Animator anim;

		// Use this for initialization
		void Start ()
		{
				nav = GetComponent<NavMeshAgent> ();
				anim = GetComponent<Animator> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				NavAnimSetup ();
		}

		void NavAnimSetup ()
		{
				anim.SetFloat ("speed", Vector3.Project (nav.desiredVelocity, transform.forward).magnitude);
		}

}
