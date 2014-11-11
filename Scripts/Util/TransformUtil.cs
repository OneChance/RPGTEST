using UnityEngine;
using System.Collections;

public class TransformUtil
{
		public static Vector3 getFootPos (Vector3 pos, float objHeight)
		{
				return new Vector3 (pos.x, pos.y - objHeight/2, pos.z);
		}
}
