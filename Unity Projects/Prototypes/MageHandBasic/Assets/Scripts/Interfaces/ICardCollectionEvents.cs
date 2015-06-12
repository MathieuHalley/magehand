using UnityEngine;
using System.Collections;

public interface ICardCollectionEvents
{
	event System.Action<Card> AddCardEvent;
	event System.Action<Card> RemoveCardEvent;
	event System.Action<Card> GetCardEvent;
}
