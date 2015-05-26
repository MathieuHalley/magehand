using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour {

	public static Vector3 MouseWorldPosition
	{
		get
		{
			return Camera.main.ScreenToWorldPoint(
				new Vector3(Input.mousePosition.x,
					   Input.mousePosition.y,
					   -Camera.main.transform.position.z));
		}
	}
}
