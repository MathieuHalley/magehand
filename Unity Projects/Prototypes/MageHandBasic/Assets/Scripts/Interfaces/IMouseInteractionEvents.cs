using UnityEngine;
using System.Collections;

public interface IMouseInteractionEvents
{
	event System.Action<GameObject> MouseDownEvent;
	event System.Action<GameObject> MouseDragEvent;
	event System.Action<GameObject> MouseEnterEvent;
	event System.Action<GameObject> MouseExitEvent;
	event System.Action<GameObject> MouseOverEvent;
	event System.Action<GameObject> MouseUpEvent;
	event System.Action<GameObject> MouseUpAsAButtonEvent;
}
