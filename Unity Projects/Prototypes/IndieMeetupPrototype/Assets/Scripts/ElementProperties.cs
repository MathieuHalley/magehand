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

/// <summary>
///		ElementProperties : System.Object
///			private Elements _element
///			private int _level
///			public Elements Element
///			public int Level
///			public int Tier
///			----
///			private ElementProperties(EType, int)
///			public ElementProperties()
///			public ElementProperties(Elements, int)
///			----
///			public static ElementProperties Merge(ElementProperties, ElementProperties)
///			public static bool ValidMerge(ElementProperties, ElementProperties)
///			public static int Compare(ElementProperties, ElementProperties)
///			public override string ToString() 
///			public bool Equals(ElementProperties)
///			public override bool Equals(object)
///			public static bool operator ==(ElementProperties, ElementProperties)
///			public static bool operator !=(ElementProperties, ElementProperties)
///			public override int GetHashCode()
///			----
///			private class EType : System.Object
/// </summary>
[System.Serializable]
public class ElementProperties : System.Object, IMergable<ElementProperties>
{
	public event System.Action<ElementProperties, ElementProperties> MergeBegin;
	public event System.Action<ElementProperties, ElementProperties, ElementProperties, bool> MergeComplete;

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
	public int Tier
	{
		get
		{
			return EType.ElementTier(_element);
		}
	}
	public bool IsValid
	{
		get
		{
			return EType.IsValidElement(_element);
		}
	}

	//	Constructors
	private ElementProperties(EType e, int lvl)
	{
		_element = e.Element;
		_level = lvl;
	}

	public ElementProperties()
	{
		_element = Elements.Air;
		_level = 1;
	}

	public ElementProperties(Elements e, int lvl)
	{
		_element = e;
		_level = lvl;
	}

	//	Merge
	//		ElementPropertiess can be merged into ElementProperties with Tier 2 elements and into ElementProperties of a higher _level
	//		If they are identical the element stays the same, but the level increases
	//		If they are the same level and both have different elements a Tier 2 element created, level stays the same
	//		If they have different level and element it becomes an Invalid element container
	public ElementProperties Merge(ElementProperties e)
	{
		ElementProperties mergedElement;
		mergedElement = ElementProperties.Merge(this, e);
		return mergedElement;
	}

	public static ElementProperties Merge(ElementProperties e1, ElementProperties e2)
	{
		ElementProperties mergedElement;

		e1.MergeBegin(e1, e2);
		e2.MergeBegin(e2, e1);

		if (ValidMerge(e1, e2))
		{
			mergedElement = new ElementProperties(EType.Merge(e1.Element, e2.Element), e1.Level);
			if (e1.Element == e2.Element)
				++mergedElement.Level;
		}
		else
		{
			mergedElement = new ElementProperties(Elements.Invalid, Mathf.Min(e1.Level, e2.Level));
		}
		e1.MergeComplete(e1, e2, mergedElement, mergedElement.IsValid);

		return mergedElement;
	}

	public bool ValidMerge(ElementProperties e)
	{
		return ElementProperties.ValidMerge(this, e);
	}

	public static bool ValidMerge(ElementProperties e1, ElementProperties e2)
	{
		if (EType.Merge(e1.Element, e2.Element).IsValid && e1.Level == e2.Level)
		{
			if (e1 == e2 && e1.Level == 3)
				return false;
			else
				return true;
		}
		else
			return false;
	}

	public void OnMergeBegin(ElementProperties e1, ElementProperties e2) { }
	public void OnMergeComplete(ElementProperties e1, ElementProperties e2, ElementProperties merged, bool valid) { }

	//
	//	Operators, Comparisons & Overrides
	//
	#region operators, comparisons & overrides
	public static int Compare(ElementProperties e1, ElementProperties e2)
	{
		if (e1.Element == e2.Element && e1.Level == e2.Level)
			return 0;
		else if (e1.Level < e2.Level)
			return -1;
		else // e1Tier > e2Tier
			return 1;
	}

	public override string ToString() 
	{ 
		return _element.ToString() + " (" + _level.ToString() + ")"; 
	}

	public bool Equals(ElementProperties e) 
	{
		return (_element == e.Element && _level == e.Level) ? true : false;
	}

	public override bool Equals(object o)
	{
		ElementProperties e;
		if (typeof(ElementProperties) == o.GetType())
			e = o as ElementProperties;
		else
			return false;

		return (_element == e.Element && _level == e.Level) ? true : false;
	}
	
	public static bool operator ==(ElementProperties e1, ElementProperties e2)
	{
		return (Compare(e1, e2) == 0) ? true : false;
	}
	
	public static bool operator !=(ElementProperties e1, ElementProperties e2)
	{
		return (Compare(e1, e2) != 0) ? true : false;
	}

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

	/// <summary>
	///		EType : System.Object
	///			private Elements _element
	///			public Elements Element
	///			public int Tier
	///			public int IsValid
	///			----
	///			public EType()
	///			public EType(Elements)
	///			----
	///			public static EType Merge(Elements, Elements)
	///			public static int ElementTier(Elements)
	///			public static bool IsVaildElement(Elements)
	///			public override string ToString()
	///			public static int Compare(EType, EType)
	///			public bool Equals(EType)
	///			public bool Equals(Object)
	///			public static bool operator ==(EType, EType)
	///			public static bool operator !=(EType, EType)
	///			public override int GetHashCode()
	/// </summary>
	private class EType : System.Object
	{
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
		public int Tier
		{
			get
			{
				return ElementTier(_element);
			}
		}
		public bool IsValid 
		{
			get
			{
				return IsValidElement(_element);
			}
		}

		//	Constuctors
		public EType() 
		{
			_element = Elements.Air;
		}
		
		public EType(Elements e)
		{
			_element = e;
		}

		//	Merge
		//		Tier 1 elements can be merged into Tier 2 elements
		//		but only if they'll form a valid Tier 2 _element
		//		neither Air + Earth nor Water + Fire can be merged
		public static EType Merge(Elements e1, Elements e2)
		{
			EType e = new EType();

			if (ElementTier(e1) == 1 && ElementTier(e2) == 1)
			{
				if (e1 == e2)
					e._element = e1;
				else if (IsValidElement((Elements)((int)e1 + (int)e2)))
					e._element = (Elements)((int)e1 + (int)e2);
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

		//
		//	Operators, Comparisons & Overrides
		//
		#region operators, comparisons & overrides

		public override string ToString()
		{
			return _element.ToString();
		}

		public static int Compare(EType e1, EType e2)
		{
			if (e1.Element == e2.Element)
				return 0;
			else if (e1.Tier < e2.Tier)
				return -1;
			else // e1Tier > e2Tier
				return 1;
		}
		
		public bool Equals(EType e)
		{
			return (_element == e._element) ? true : false;
		}
		
		public override bool Equals(object o)
		{
			EType e;
			if (typeof(EType) == o.GetType())
				e = o as EType;
			else
				return false;

			return (_element == e._element) ? true : false;
		}
		
		public static bool operator ==(EType e1, EType e2)
		{
			return (Compare(e1, e2) == 0) ? true : false;
		}
		
		public static bool operator !=(EType e1, EType e2)
		{
			return (Compare(e1, e2) != 0) ? true : false;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (int)2166136261 * (16777619 ^ _element.GetHashCode());
			}
		}
		#endregion
	}
}