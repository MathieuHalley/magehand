using UnityEngine;
using System.Collections;

public class PrefabHandler : MonoBehaviour 
{
	public static PrefabHandler Instance { get; private set; }

	public GameObject defaultPrefab;
	public GameObject[] airPrefabs;
	public GameObject[] earthPrefabs;
	public GameObject[] firePrefabs;
	public GameObject[] waterPrefabs;
	public GameObject[] icePrefabs;
	public GameObject[] lightningPrefabs;
	public GameObject[] meteorPrefabs;
	public GameObject[] plantPrefabs;

	public void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public GameObject SpellBallPrefabs(ElementProperties elementProperties)
	{
		GameObject newSpellBallPrefab = null;

		switch (elementProperties.Element)
		{
			case Elements.Air:
				if (airPrefabs.Length >= elementProperties.Level - 1)
					newSpellBallPrefab = airPrefabs[elementProperties.Level - 1];
				break;
			case Elements.Earth:
				if (earthPrefabs.Length >= elementProperties.Level - 1)
					newSpellBallPrefab = earthPrefabs[elementProperties.Level - 1];
				break;
			case Elements.Fire:
				if (firePrefabs.Length >= elementProperties.Level - 1)
					newSpellBallPrefab = firePrefabs[elementProperties.Level - 1];
				break;
			case Elements.Water:
				if (waterPrefabs.Length >= elementProperties.Level - 1)
					newSpellBallPrefab = waterPrefabs[elementProperties.Level - 1];
				break;
			case Elements.Ice:
				if (icePrefabs.Length >= elementProperties.Level - 1)
					newSpellBallPrefab = icePrefabs[elementProperties.Level - 1];
				break;
			case Elements.Lightning:
				if (lightningPrefabs.Length >= elementProperties.Level - 1)
					newSpellBallPrefab = lightningPrefabs[elementProperties.Level - 1];
				break;
			case Elements.Meteor:
				if (meteorPrefabs.Length >= elementProperties.Level - 1)
					newSpellBallPrefab = meteorPrefabs[elementProperties.Level - 1];
				break;
			case Elements.Plants:
				if (plantPrefabs.Length >= elementProperties.Level - 1)
					newSpellBallPrefab = plantPrefabs[elementProperties.Level - 1];
				break;
			default: // Elements.Invalid
				newSpellBallPrefab = defaultPrefab;
				break;
		}

		//	In case one of the other prefabs isn't there
		if (newSpellBallPrefab == null)
			newSpellBallPrefab = defaultPrefab;

		return newSpellBallPrefab;
	}
}
