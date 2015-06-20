using UnityEngine;
using System.Collections;

public class PrefabHandler : MonoBehaviour
{
	private static PrefabHandler _instance;
	public static PrefabHandler Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<PrefabHandler>();
				DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}

	public GameObject defaultPrefab;
	public GameObject airCardPrefab;
	public GameObject earthCardPrefab;
	public GameObject fireCardPrefab;
	public GameObject waterCardPrefab;
	public GameObject cardPilePrefab;

	// Use this for initialization
	void Awake()
	{
		if (_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = GameObject.FindObjectOfType<PrefabHandler>();
			DontDestroyOnLoad(_instance.gameObject);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if (this != _instance)
			{
				Destroy(this.gameObject);
			}
		}
	}



	public GameObject GetCardPrefab(Elements element)
	{
		switch (element)
		{
			case Elements.Air:
				return airCardPrefab;
			case Elements.Earth:
				return earthCardPrefab;
			case Elements.Fire:
				return fireCardPrefab;
			case Elements.Water:
				return waterCardPrefab;
			default:
				return defaultPrefab;
		}
	}

	public GameObject GetCardPilePrefab()
	{
		return cardPilePrefab;
	}
}
