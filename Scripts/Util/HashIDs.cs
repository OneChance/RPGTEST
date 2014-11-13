using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
		public int speedFloat;

		void Awake ()
		{
				speedFloat = Animator.StringToHash ("speed");
		}
}
