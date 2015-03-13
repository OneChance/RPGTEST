using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
		public int speedFloat;
		public int hSpeedFloat;
		public int vSpeedFloat;
		public int runBool;
		public int jumpTrigger;
		public int jumpState;

		void Awake ()
		{
				speedFloat = Animator.StringToHash ("speed");
				hSpeedFloat = Animator.StringToHash ("hSpeed");
				vSpeedFloat = Animator.StringToHash ("vSpeed");
				runBool = Animator.StringToHash ("isRun");
				jumpTrigger = Animator.StringToHash ("jump");
				jumpState = Animator.StringToHash ("Base Layer.IdleJump");
		}
}
