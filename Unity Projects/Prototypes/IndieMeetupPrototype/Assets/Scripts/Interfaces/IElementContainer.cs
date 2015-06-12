using UnityEngine;
using System.Collections;

public interface IElementContainer
{
	event System.Action<IElementContainer, ElementProperties> ElementUpdate;
	event System.Action<IElementContainer, ElementProperties> LevelUpdate;
	event System.Action<IElementContainer, ElementProperties> TierUpdate;

	ElementProperties Properties { get; set; }
	void OnEnable();
	void OnValidate();
	string UpdateName();
}
