﻿using UnityEngine;
using System;
using System.Collections;


public interface IInputInteractionEvents
{
	event System.Action<GameObject> MouseDownEvent;
	event System.Action<GameObject> MouseDragEvent;
	event System.Action<GameObject> MouseEnterEvent;
	event System.Action<GameObject> MouseExitEvent;
	event System.Action<GameObject> MouseOverEvent;
	event System.Action<GameObject> MouseUpAsAButtonEvent;
	event System.Action<GameObject> MouseUpEvent;
}