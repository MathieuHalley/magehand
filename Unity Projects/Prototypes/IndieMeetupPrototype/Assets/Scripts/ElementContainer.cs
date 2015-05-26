using UnityEngine;
using System.Collections;

public class ElementContainer : MonoBehaviour, IElementContainer 
{
	[SerializeField]
	protected ElementProperties _properties;
	public ElementProperties Properties
	{
		get
		{
			return _properties;
		}
		set
		{
			_properties = value;
		}
	}

	public virtual void OnEnable()
	{
		UpdateName();
	}

	public virtual void OnValidate()
	{
		UpdateName();
	}

	public virtual string UpdateName()
	{
		name = _properties.ToString();
		return name;
	}

}
