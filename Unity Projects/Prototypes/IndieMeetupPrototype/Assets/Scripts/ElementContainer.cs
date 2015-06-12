using UnityEngine;
using System.Collections;

public class ElementContainer : MonoBehaviour, IElementContainer 
{
	public event System.Action<IElementContainer, ElementProperties> ElementUpdate;
	public event System.Action<IElementContainer, ElementProperties> LevelUpdate;
	public event System.Action<IElementContainer, ElementProperties> TierUpdate;

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
			CheckPropertyUpdate(_properties, value);
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

	private void CheckPropertyUpdate(ElementProperties e1, ElementProperties e2)
	{
		if (e1.Element != e2.Element && ElementUpdate != null)
			ElementUpdate(this, e2);
		if (e1.Level != e2.Level && LevelUpdate != null)
			LevelUpdate(this, e2);
		if (e1.Tier != e2.Tier && TierUpdate != null)
			TierUpdate(this, e2);
	}

}
