using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour 
{
	public static Util Instance { get; private set; }

	public void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	IEnumerator Perform(IEnumerator coroutine)
	{
		yield return StartCoroutine(coroutine);
	}

	static public void DoCoroutine(IEnumerator coroutine)
	{
		Instance.StartCoroutine(Instance.Perform(coroutine));
	}

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
