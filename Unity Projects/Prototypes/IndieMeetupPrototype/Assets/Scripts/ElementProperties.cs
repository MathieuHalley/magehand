using UnityEngine;
using System.Collections;

[System.Serializable]
public class ElementProperties : System.Object
{
	[SerializeField]
	private ElementType _element;
	public ElementType Element 
	{ 
		get 
		{ 
			return _element; 
		} 
		set
		{
			_element = value;
		}
	}
	[SerializeField]
	private int _level;
	public int Level
	{
		get
		{
			return _level;
		}
		set
		{
			_level = Mathf.Max(1, value);
		}
	}

	//	Constructors
	public ElementProperties()
	{
		_element = new ElementType();
		_level = 1;
	}

	public ElementProperties(ElementType e, int lvl)
	{
		_element = e;
		_level = lvl;
	}

	public ElementProperties(Elements e, int lvl)
	{
		_element = new ElementType(e);
		_level = lvl;
	}

	//	Merge
	//		ElementPropertiess can be merged into ElementPropertiess with Tier 2 elements and into ElementPropertiess of a higher _level
	//		If they are identical the element stays the same, but the level increases
	//		If they are the same level and both have different elements a Tier 2 element created, level stays the same
	//		If they have different level and element it becomes an Invalid element container
	public static ElementProperties Merge(ElementProperties e1, ElementProperties e2)
	{
		ElementProperties mergedElement;

		if (ValidMerge(e1,e2))
		{
			mergedElement = new ElementProperties(ElementType.Merge(e1.Element, e2.Element), e1.Level);
			if (e1.Element == e2.Element)
				++mergedElement.Level;
		}
		else
			mergedElement = new ElementProperties(Elements.Invalid, Mathf.Min(e1.Level, e2.Level));

		return mergedElement;
	}

	public static bool ValidMerge(ElementProperties e1, ElementProperties e2)
	{
		if (ElementType.Merge(e1.Element, e2.Element).IsValid && e1.Level == e2.Level)
		{
			if (e1 == e2 && e1.Level == 3)
				return false;
			else
				return true;
		}
		else
			return false;
	}

	//
	//	Operators, Comparisons & Common Overrides
	//
	#region operators, comparisons & common overrides

	public static int Compare(ElementProperties e1, ElementProperties e2)
	{
		if (e1.Element == e2.Element && e1.Level == e2.Level)
			return 0;
		else if (e1.Level < e2.Level)
			return -1;
		else // e1Tier > e2Tier
			return 1;
	}
	public override string ToString() { return _element.ToString() + " (" + _level.ToString() + ")"; }
	public bool Equals(ElementProperties e) { return (_element == e.Element && _level == e.Level) ? true : false; }
	public override bool Equals(object o) { return (typeof(ElementProperties) == o.GetType()) ? true : false; }
	public static bool operator ==(ElementProperties e1, ElementProperties e2) { return (Compare(e1, e2) == 0) ? true : false; }
	public static bool operator !=(ElementProperties e1, ElementProperties e2) { return (Compare(e1, e2) != 0) ? true : false; }

	public override int GetHashCode() 
	{ 
		unchecked 
		{ 
			return base.GetHashCode() + (int)2166136261 
				* (16777619 ^ _element.GetHashCode()) 
				* (16777619 ^ _level.GetHashCode()); 
		}
	}
	#endregion
}
