using UnityEngine;
using System.Collections;

public interface ICardDataEvents
{
	event System.Action<bool> FlipCardEvent;
}
