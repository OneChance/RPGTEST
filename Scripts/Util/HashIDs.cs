using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
		public int speedFloat;
		public int hSpeedFloat;
		public int vSpeedFloat;
		public int runBool;
		public int jumpBool;
		public int jumpState;

		void Awake ()
		{
				speedFloat = Animator.StringToHash ("speed");
				hSpeedFloat = Animator.StringToHash ("hSpeed");
				vSpeedFloat = Animator.StringToHash ("vSpeed");
				runBool = Animator.StringToHash ("isRun");
				jumpBool = Animator.StringToHash ("isJump");
				jumpState = Animator.StringToHash ("Base Layer.IdleJump");
		}
}
