using UnityEngine;
using System.Collections;

public interface IObjectInteractionEvents
{
	event System.Action<GameObject, Collider2D> TriggerEnter2DEvent;
	event System.Action<GameObject, Collider2D> TriggerExit2DEvent;
	event System.Action<GameObject, Collider2D> TriggerStay2DEvent;
}
