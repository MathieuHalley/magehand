using UnityEngine;
using System.Collections;

public interface IElementContainer
{
	ElementProperties Properties { get; set; }
	void OnEnable();
	void OnValidate();
	string UpdateName();
}
