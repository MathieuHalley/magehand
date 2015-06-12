using UnityEngine;
using System.Collections;

public class SpellBallManager : ElementContainer, IMergable<GameObject>
{
	public event System.Action<GameObject, GameObject> MergeBegin;
	public event System.Action<GameObject, GameObject, GameObject, bool> MergeComplete;
	public event System.Action<GameObject, GameObject> MoveToBegin;
	public event System.Action<GameObject, GameObject> MoveToComplete;

	public float driftSpeed;
	public float moveSpeed;

	public override void OnEnable()
	{
		base.OnEnable();
		MergeBegin += OnMergeBegin;
		MergeComplete += OnMergeComplete;

		MoveToBegin += OnMoveToBegin;
		MoveToComplete += OnMoveToComplete;
	}

	public void OnDisable()
	{
		MergeBegin -= OnMergeBegin;
		MergeComplete -= OnMergeComplete;

		MoveToBegin -= OnMoveToBegin;
		MoveToComplete -= OnMoveToComplete;
	}

	//	Merge this Spell Ball with spellBall
	public GameObject Merge(GameObject spellBall2)
	{
		Vector3 mergePosition = (this.transform.position
					           + spellBall2.transform.position) * 0.5f;

		return Merge(spellBall2, mergePosition);
	}

	//	Merge this Spell Ball with spellBall at mergePosition
	public GameObject Merge(GameObject spellBall2, Vector3 mergePosition)
	{
		GameObject newSpellBall = null;
		GameObject spellBall1;
		ElementProperties newProperties;

		SpellBallManager spellBallManager1;
		SpellBallManager spellBallManager2;

		spellBall1 = this.gameObject;
		spellBallManager1 = this;
		spellBallManager2 = spellBall2.GetComponent<SpellBallManager>();

		spellBallManager1.MergeBegin(spellBall1, spellBall2);
//		spellBallManager2.MergeBegin(spellBall2, spellBall1);

		newProperties = ElementProperties.Merge(spellBallManager1.Properties,
												spellBallManager2.Properties);

		if (newProperties.IsValid)
		{
			newSpellBall = NewSpellBall(newProperties);
			newSpellBall = (GameObject)Instantiate(newSpellBall,
												   mergePosition,
												   Quaternion.identity);
			newSpellBall.SetActive(false);

			StartCoroutine(spellBallManager1.MoveTo(newSpellBall, true));
			StartCoroutine(spellBallManager2.MoveTo(newSpellBall, true));

			spellBallManager1.MergeComplete(spellBall1, spellBall2, newSpellBall, true);
		}
		else
		{
			newSpellBall = null;
			spellBallManager1.MergeComplete(spellBall1, spellBall2, null, true);
		}

		return newSpellBall;
	}

	public bool ValidMerge(GameObject spellBall)
	{		
		return ElementProperties
			   .ValidMerge(this.Properties,
		                   spellBall
		                   .GetComponent<SpellBallManager>()
		                   .Properties);
	}

	public void OnMergeBegin(GameObject spellBall1, GameObject spellBall2)
	{
	}

	public void OnMergeComplete(GameObject spellBall1, GameObject spellBall2, GameObject mergedSpellBall, bool validSpellBall)
	{

	}

	public static GameObject NewSpellBall(ElementProperties elementProperties)
	{
		return PrefabHandler.Instance.SpellBallPrefabs(elementProperties);
	}

	public override string UpdateName()
	{
		name = base.UpdateName() + " - Spell Ball";
		return name;
	}

	public IEnumerator MoveTo(GameObject targetObject, bool accelerate = false)
	{
		float sqrDist;
		float accel;

		MoveToBegin(this.gameObject, targetObject);

		while (this.transform.position != targetObject.transform.position && this != null)
		{
			sqrDist = Mathf.Pow((this.transform.position - targetObject.transform.position).magnitude, 2);
			if (accelerate == true)
				accel = 10.0f * ((Properties.Level * Properties.Level) / sqrDist);
			else
				accel = 0;
//			this
//			.GetComponent<Rigidbody2D>()
//			.MovePosition(Vector3.MoveTowards(this.transform.position, 
//			                                  targetObject.transform.position, 
//			                                  (speed + accel) * Time.deltaTime));
			this.transform.position = Vector3.MoveTowards(this.transform.position, 
				                                          targetObject.transform.position, 
														  (moveSpeed + accel) * Time.deltaTime);
			yield return null;
		}
		MoveToComplete(this.gameObject, targetObject);
	}

	public void OnMoveToBegin(GameObject spellBall, GameObject target) { }
	public void OnMoveToComplete(GameObject spellBall, GameObject target) { }
}