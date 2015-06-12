using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ICardCollection
{
	List<Card> Cards { get; }

	Card AddCard(GameObject cardGameObject, bool faceUp = true);
	Card AddCard(Card card, bool faceUp = true);
	Card AddNewCard(Elements element, bool faceUp = true);
	Card AddNewRandomCard(bool faceUp = true);
	Card RemoveCard(GameObject cardGameObject);
	Card RemoveCard(Card card);
	Card GetCard(Elements element);
	Card GetRandomCard();
	Card GetRandomCard(Elements element);
}
