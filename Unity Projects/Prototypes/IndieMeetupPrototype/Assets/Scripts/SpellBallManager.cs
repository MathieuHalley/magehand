using UnityEngine;
using System.Collections;

public class SpellBallManager : ElementContainer 
{
	public float speed;
	public GameObject spellTransitPrefab;
	public GameObject spellImpactPrefab;
	public SpellPaletteManager spellPalette;

	private GameObject spellTransitGO;
	private GameObject spellImpactGO;
	private Vector3 MoveTarget;

	public bool movingState = false;
	public bool dragState = false;

	public void Update()
	{
		if (dragState == true && Input.GetMouseButtonUp(0))
			EndBallDrag();
	}

	public void OnMouseDown()
	{
		StartBallDrag();
	}

	public void StartBallDrag()
	{
		Debug.Log("MouseDown");
		dragState = true;
	}

	public void EndBallDrag()
	{
		Collider2D dragEndPoint;
		string layerName;
		dragState = false;

		dragEndPoint = 
			Physics2D.OverlapPoint(Util.MouseWorldPosition, 
				~(LayerMask.NameToLayer("Spell Palette") |
				  LayerMask.NameToLayer("Spell Ball")));
		if (dragEndPoint == null)
		{
			layerName = "";
		}
		else
		{
			layerName = LayerMask.LayerToName(dragEndPoint.gameObject.layer);
		}
		Debug.Log("DragEndPoint: " + dragEndPoint.name);

		switch (layerName)
		{
			case "Spell Palette":
				TransferSpellBall(dragEndPoint);
				break;
			case "Spell Ball":
				MergeSpellBalls(dragEndPoint);
				break;
			default:
				break;
		}
	}

	public void TransferSpellBall(Collider2D newSpellPaletteCollider)
	{
		spellPalette.RemoveSpellBall(this.gameObject);
		newSpellPaletteCollider.GetComponent<SpellPaletteManager>().AddSpellBall(this.gameObject, false);
		StartCoroutine(MoveTo(Util.MouseWorldPosition, false));
	}

	public void MergeSpellBalls(Collider2D mergeTargetCollider)
	{
		GameObject spellBall2 = mergeTargetCollider.gameObject;

		if (spellPalette != spellBall2.GetComponent<SpellBallManager>().spellPalette)
		{
			Debug.Log("Merge fail different spellPalettes");
			return;
		}

		Debug.Log("MergeSpellBalls");
		StartCoroutine(spellPalette.MergeSpellBalls(this.gameObject, spellBall2));
	}
	
	public override string UpdateName()
	{
		name = base.UpdateName() + " - Spell Ball";
		return name;
	}

	public IEnumerator MoveTo(Vector3 target, bool accelerate = false)
	{
		float sqrDist;
		float accel;

		movingState = true;

		while (this.transform.position != target && this != null)
		{
			sqrDist = Mathf.Pow((this.transform.position - target).magnitude, 2);
			if (accelerate == true)
				accel = 10.0f * ((Properties.Level * Properties.Level) / sqrDist);
			else
				accel = 0;
//			this.GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(this.transform.position, target, (speed + accel) * Time.deltaTime));
			this.transform.position = Vector3.MoveTowards(this.transform.position, target, (speed + accel) * Time.deltaTime);
			yield return null;
		}
		movingState = false;
	}
}