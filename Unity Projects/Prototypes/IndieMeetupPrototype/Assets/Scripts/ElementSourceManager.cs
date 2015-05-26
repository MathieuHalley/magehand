using UnityEngine;
using System.Collections;

public class ElementSourceManager : ElementContainer
{
	private bool harvestState;
	private float harvestClock;
	private int harvestCount;

	public int curLevel;
	public int ballsAvailable;
	public bool Empty { get { return (curLevel == 0) ? true : false; } }

	public SpellPaletteManager spellPalette;

	public float timeToHarvestBall = 1f;

	void Start()
	{

		spellPalette.autoMerge = true;
		StartElementHarvest();
	}

	void Update()
	{
		if (harvestState)
		{
			OnElementHarvestUpdate();
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		Reset();
	}

	public override void OnValidate()
	{
		base.OnValidate();
		Reset();
	}

	void OnMouseDown()
	{
		//		StartElementHarvest();
	}

	void OnMouseUp()
	{
		//		EndElementHarvest();
	}

	public void Reset()
	{
		curLevel = _properties.Level;
		ResetBallsAvailable();
	}

	public void ResetBallsAvailable()
	{
		ballsAvailable = (int)(Mathf.Pow(curLevel, 2) * 0.5f);
	}

	public override string UpdateName()
	{
		name = base.UpdateName() + " - Element Source";
		return name;
	}

	#region ElementHarvest
	public void StartElementHarvest()
	{
		harvestState = true;
		harvestClock = 0f;
		harvestCount = 0;
	}

	public void EndElementHarvest()
	{
		harvestState = false;
		harvestClock = 0f;
		harvestCount = 0;
	}

	//	At harvest time create a new level 1 spellBall and add it to the source's spellPalette
	//	If harvestCount % 2 == 0 merge two level 1 spellBalls into a level 2 spellBall
	//	If harvestCount % 4 == 0 merge two level 2 spellBalls into a level 3 spellBall
	private void OnElementHarvestUpdate()
	{
		harvestClock += Time.deltaTime;
		if (harvestClock >= timeToHarvestBall)
		{
			harvestClock -= timeToHarvestBall;
			if (HarvestBall())
			{
				++harvestCount;
				spellPalette.AddSpellBall(new ElementProperties(Properties.Element, 1));
				spellPalette.FindAndMergeAllValidPairs();
			}
		}

		if (Empty)
			EndElementHarvest();
	}

	public bool HarvestBall()
	{
		if (curLevel == 0 || ballsAvailable == 0)
			return false;

		--ballsAvailable;
		if (ballsAvailable <= Mathf.Pow(curLevel - 1, 2) * 0.5f)
		{
			--curLevel;
			ResetBallsAvailable();
		}
		return true;
	}
	#endregion
}
