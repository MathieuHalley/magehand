using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellPaletteManager : MonoBehaviour 
{
	private int[] validPairs;
	private bool validPairsExist = false;
	private bool mergeState = false;

	public List<GameObject> spellBalls;
	public GameObject defaultPrefab;
	public GameObject[] airPrefabs;
	public GameObject[] earthPrefabs;
	public GameObject[] firePrefabs;
	public GameObject[] waterPrefabs;
	public GameObject[] icePrefabs;
	public GameObject[] lightningPrefabs;
	public GameObject[] meteorPrefabs;
	public GameObject[] plantPrefabs;

	public bool autoMerge = false;

	public void Start()
	{
		spellBalls = new List<GameObject>();
		InvokeRepeating("ValidPairUpdate", 0, 5f);
	}

	public void Update()
	{
		if (autoMerge == true && validPairsExist == true && mergeState == false)
			FindAndMergeAllValidPairs();
	}

	//	Update the list of valid pairs
	public void ValidPairUpdate()
	{
		ElementProperties sb1Properties;
		ElementProperties sb2Properties;

		validPairsExist = false;
		validPairs = new int[spellBalls.Count];

		for (int i = 0; i < spellBalls.Count; ++i)
		{
			sb1Properties = spellBalls[i].GetComponent<SpellBallManager>().Properties;
			validPairs[i] = -1;

			for (int j = i + 1; j < spellBalls.Count; ++j)
			{
				sb2Properties = spellBalls[j].GetComponent<SpellBallManager>().Properties;
				if (ElementProperties.ValidMerge(sb1Properties, sb2Properties))
				{
					validPairs[i] = j;
					validPairsExist = true;
					break;
				}
			}
		}
	}

	public void FindAndMergeAllValidPairs()
	{
		ValidPairUpdate();
		for (int i = 0; i < validPairs.Length; ++i)
		{
			if (validPairs[i] != -1)
			{
				FindAndMerge(spellBalls[i].GetComponent<SpellBallManager>().Properties,
					spellBalls[validPairs[i]].GetComponent<SpellBallManager>().Properties);
				break;
			}
		}
	}

	public GameObject NewSpellBall(ElementProperties sb)
	{
		GameObject newSpellBallPrefab = null;
		GameObject newSpellBall;
		Vector3 spawnPosition;

		spawnPosition = this.transform.position 
//			+ (Vector3)Random.insideUnitCircle 
			+ (Vector3)Random.insideUnitCircle.normalized
			* this.gameObject.GetComponent<CircleCollider2D>().radius;

		print("new spell ball properties: " + sb.ToString());
		switch (sb.Element.Element)
		{
			case Elements.Air:
				if (airPrefabs.Length >= sb.Level - 1)
				newSpellBallPrefab = airPrefabs[sb.Level - 1];
				break;
			case Elements.Earth:
				if (earthPrefabs.Length >= sb.Level - 1)
					newSpellBallPrefab = earthPrefabs[sb.Level - 1];
				break;
			case Elements.Fire:
				if (firePrefabs.Length >= sb.Level - 1)
					newSpellBallPrefab = firePrefabs[sb.Level - 1];
				break;
			case Elements.Water:
				if (waterPrefabs.Length >= sb.Level - 1)
					newSpellBallPrefab = waterPrefabs[sb.Level - 1];
				break;
			case Elements.Ice:
				if (icePrefabs.Length >= sb.Level - 1)
					newSpellBallPrefab = icePrefabs[sb.Level - 1];
				break;
			case Elements.Lightning:
				if (lightningPrefabs.Length >= sb.Level - 1)
					newSpellBallPrefab = lightningPrefabs[sb.Level - 1];
				break;
			case Elements.Meteor:
				if (meteorPrefabs.Length >= sb.Level - 1)
					newSpellBallPrefab = meteorPrefabs[sb.Level - 1];
				break;
			case Elements.Plants:
				if (plantPrefabs.Length >= sb.Level - 1)
					newSpellBallPrefab = plantPrefabs[sb.Level - 1];
				break;
			default: // Elements.Invalid
				newSpellBallPrefab = defaultPrefab;
				break;
		}

		//	In case one of the other prefabs isn't there
		if (newSpellBallPrefab == null)
			newSpellBallPrefab = defaultPrefab;

		newSpellBall = (GameObject)Instantiate(newSpellBallPrefab, 
			                                   spawnPosition,
											   Quaternion.identity);
		newSpellBall.transform.parent = this.transform;
		return newSpellBall;
	}

	public GameObject AddSpellBall(GameObject spellBall, bool moveTo = true)
	{
		spellBalls.Add(spellBall);
		spellBall.GetComponent<SpellBallManager>().spellPalette = this;
		spellBall.transform.parent = this.transform;

		if (moveTo == true)
			StartCoroutine(spellBall.GetComponent<SpellBallManager>().MoveTo(this.gameObject.transform.position));

		return spellBall;
	}

	public GameObject AddSpellBall(ElementProperties sb)
	{
		GameObject newSpellBall = NewSpellBall(sb);

		spellBalls.Add(newSpellBall);
		newSpellBall.GetComponent<SpellBallManager>().spellPalette = this;
		newSpellBall.transform.parent = this.transform;
		return newSpellBall;
	}

	//	RemoveSpellBall
	//		Remove spellBall from this palette and its list of spell balls
	public void RemoveSpellBall(GameObject spellBall)
	{
		spellBall.GetComponent<SpellBallManager>().spellPalette = null;
		spellBall.transform.parent = null;
		spellBalls.Remove(spellBall);
	}

	//	FindAndMerge
	//		Find 2 spell balls with the matching properties and then merge them
	public bool FindAndMerge(ElementProperties ep1, ElementProperties ep2)
	{
		GameObject sBall1;
		GameObject sBall2;
		Debug.Log("FindAndMerge begin: " + ep1 + ", " + ep2);

		if (spellBalls.Count < 2 || !ElementProperties.ValidMerge(ep1,ep2))
		{
			Debug.Log("FindAndMerge fail: " + ep1 + ", " + ep2);
			return false;
		}

		sBall1 = spellBalls.Find(delegate(GameObject sb1) 
		{ 
			if (sb1.GetComponent<SpellBallManager>().Properties == ep1) 
				return true; 
			else
				return false;
		});
		sBall2 = spellBalls.Find(delegate(GameObject sb2) 
		{
			if (sb2.GetComponent<SpellBallManager>().Properties == ep1 && sb2 != sBall1) 
				return true; 
			else
				return false;
		});

		if (sBall1 != null && sBall2 != null)
		{
			StartCoroutine(MergeSpellBalls(sBall1, sBall2));
			return true;
		}
		else
			return false;
	}

	//	MergeSpellBalls
	//		Validate and then Merge the 2 selected spell balls		
	public IEnumerator MergeSpellBalls(GameObject sb1, GameObject sb2)
	{
		Vector3 mergePoint;
		ElementProperties newBallProperties;
		SpellBallManager spellBall1 = sb1.GetComponent<SpellBallManager>();
		SpellBallManager spellBall2 = sb2.GetComponent<SpellBallManager>();
	

		if ((spellBall1 == null && spellBall2 == null) || !ElementProperties.ValidMerge(spellBall1.Properties, spellBall2.Properties) || mergeState)
		{
			Debug.Log("MergeSpellBalls fail");
			yield break;
		}

		mergeState = true;

		mergePoint = (sb1.transform.position + sb2.transform.position) * 0.5f;

		StartCoroutine(spellBall1.MoveTo(mergePoint, false));
		StartCoroutine(spellBall2.MoveTo(mergePoint, false));

		while (sb1.transform.position != mergePoint && sb2.transform.position != mergePoint)
		{
			yield return null;
		}

		newBallProperties = ElementProperties.Merge(spellBall1.Properties, spellBall2.Properties);

		AddSpellBall(newBallProperties).transform.position = mergePoint;

		RemoveSpellBall(sb1);
		RemoveSpellBall(sb2);
		yield return new WaitForSeconds(Time.deltaTime);
		Destroy(sb1);
		Destroy(sb2);

		mergeState = false;
	}
}
