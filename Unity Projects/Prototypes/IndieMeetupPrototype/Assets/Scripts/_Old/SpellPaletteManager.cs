using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellPaletteManager : MonoBehaviour 
{
	private int[] validPairs;
	private bool validPairsExist = false;
	private bool mergeState = false;

	public List<GameObject> spellBalls;
	public ElementSourceManager elementSource;


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

	public GameObject NewSpellBall(ElementProperties spellBallProperties)
	{
		GameObject newSpellBall;
		GameObject newSpellBallPrefab;
		Vector3 spawnPosition;

		spawnPosition = this.transform.position 
//			+ (Vector3)Random.insideUnitCircle 
			+ (Vector3)Random.insideUnitCircle.normalized
			* this.gameObject.GetComponent<CircleCollider2D>().radius;

		print("new spell ball properties: " + spellBallProperties.ToString());
		newSpellBallPrefab = SpellBallManager.NewSpellBall(spellBallProperties);
		newSpellBall = (GameObject)Instantiate(newSpellBallPrefab, 
			                                   spawnPosition,
											   Quaternion.identity);
		newSpellBall.transform.parent = this.transform;
		return newSpellBall;
	}

	public void AddSpellBall(ElementSourceManager elementSource, ElementProperties elementProperties)
	{
		GameObject newSpellBall = NewSpellBall(elementProperties);

		AddSpellBall(newSpellBall);
		newSpellBall.transform.position = elementSource.transform.position;
	}

	public void AddSpellBall(GameObject spellBall)
	{
		spellBalls.Add(spellBall);
//		spellBall.GetComponent<SpellBallManager>().spellPalette = this;
		spellBall.transform.parent = this.transform;
	}

	public void AddSpellBall(ElementProperties elementProperties)
	{
		GameObject newSpellBall = NewSpellBall(elementProperties);
		AddSpellBall(newSpellBall);
	}

	//	RemoveSpellBall
	//		Remove spellBall from this palette and its list of spell balls
	public void RemoveSpellBall(GameObject spellBall)
	{
//		spellBall.GetComponent<SpellBallManager>().spellPalette = null;
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
//			StartCoroutine(MergeSpellBalls(sBall1, sBall2));
			return true;
		}
		else
			return false;
	}

	////	MergeSpellBalls
	////		Validate and then Merge the 2 selected spell balls		
	//public IEnumerator MergeSpellBalls(GameObject sb1, GameObject sb2)
	//{
	//	Vector3 mergePoint;
	//	ElementProperties newBallProperties;
	//	GameObject newSpellBall;
	//	SpellBallManager spellBall1 = sb1.GetComponent<SpellBallManager>();
	//	SpellBallManager spellBall2 = sb2.GetComponent<SpellBallManager>();
	

	//	if ((spellBall1 == null && spellBall2 == null) || !ElementProperties.ValidMerge(spellBall1.Properties, spellBall2.Properties) || mergeState)
	//	{
	//		Debug.Log("MergeSpellBalls fail");
	//		yield break;
	//	}

	//	mergeState = true;

	//	mergePoint = (sb1.transform.position + sb2.transform.position) * 0.5f;

	//	StartCoroutine(spellBall1.MoveTo(mergePoint, false));
	//	StartCoroutine(spellBall2.MoveTo(mergePoint, false));

	//	while (sb1.transform.position != mergePoint && sb2.transform.position != mergePoint)
	//	{
	//		yield return null;
	//	}

	//	newBallProperties = ElementProperties.Merge(spellBall1.Properties, spellBall2.Properties);

	//	newSpellBall = NewSpellBall(newBallProperties);
	//	newSpellBall.transform.position = mergePoint;
	//	AddSpellBall(newSpellBall);

	//	RemoveSpellBall(sb1);
	//	RemoveSpellBall(sb2);
	//	yield return new WaitForSeconds(Time.deltaTime);
	//	Destroy(sb1);
	//	Destroy(sb2);

	//	mergeState = false;
	//}
}
