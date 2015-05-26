using UnityEngine;
using System.Collections;

public enum Elements
{
	//	Invalid Elements
	Invalid = -1,
	AirEarth = 3,
	FireWater = 12,
	//	Valid Elements
	//		Tier 1
	Air = 1,
	Earth = 2,
	Fire = 4,
	Water = 8,
	//		Tier 2
	Lightning = 5,
	Ice = 9,
	Meteor = 6,
	Plants = 10
}

[System.Serializable]
public class ElementType : System.Object 
{
	[SerializeField]
	private Elements _element;
	public Elements Element 
	{ 
		get 
		{
			return _element; 
		} 
		set
		{
			if (IsValidElement(value))
				_element = value;
		}
	}
	public int Tier { get { return ElementTier(_element); } }
	public bool IsValid { get { return IsValidElement(_element); } }

	//	Constuctors
	public ElementType() { _element = Elements.Air; }
	public ElementType(Elements e) { _element = e; }

	//	Merge
	//		Tier 1 elements can be merged into Tier 2 elements
	//		but only if they'll form a valid Tier 2 _element
	//		neither Air + Earth nor Water + Fire can be merged
	public static ElementType Merge(ElementType e1, ElementType e2)
	{
		ElementType e = new ElementType();

		if (ElementTier(e1._element) == 1 && ElementTier(e2._element) == 1)
		{
			if (e1._element == e2._element)
				e._element = e1._element;
			else if (IsValidElement((int)e1._element + (int)e2._element))
				e._element = (Elements)((int)e1._element + (int)e2._element);
			else
				e._element = Elements.Invalid;
		}
		else
			e._element = Elements.Invalid;

		return e;
	}

	public static int ElementTier(Elements e) 
	{
		if (Mathf.IsPowerOfTwo((int)e))
			return 1;
		else if (IsValidElement(e))
			return 2;
		else
			return -1;
	}

	public static bool IsValidElement(Elements e) 
	{
		return (e != Elements.AirEarth
			 && e != Elements.FireWater
			 && e != Elements.Invalid) ? true : false;
	}

	public static bool IsValidElement(int e) 
	{ 
		return (e != (int)Elements.AirEarth 
			 && e != (int)Elements.FireWater 
			 && e != (int)Elements.Invalid ) ? true : false; 
	}

	//
	//	Operators, Comparisons & Common Overrides
	//
	#region operators, comparisons & common overrides

	public static int Compare(ElementType e1, ElementType e2)
	{
		if (e1.Element == e2.Element)
			return 0;
		else if (e1.Tier < e2.Tier)
			return -1;
		else // e1Tier > e2Tier
			return 1;
	}

	public override string ToString() { return _element.ToString(); }
	public bool Equals(ElementType e) { return (_element == e._element) ? true : false; }
	public override bool Equals(object o) { return (typeof(ElementType) == o.GetType()) ? true : false; }
	public override int GetHashCode() { unchecked { return (int)2166136261 * (16777619 ^ _element.GetHashCode()); } }
	public static bool operator ==(ElementType e1, ElementType e2) { return (Compare(e1, e2) == 0) ? true : false; }
	public static bool operator !=(ElementType e1, ElementType e2) { return (Compare(e1, e2) != 0) ? true : false; }
	public static ElementType operator +(ElementType e1, ElementType e2) { return Merge(e1, e2); }
	#endregion
}
