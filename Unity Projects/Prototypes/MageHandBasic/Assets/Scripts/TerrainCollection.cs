using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainCollection : CardCollection, IInteractable
{
	public List<CardCollection> terrainCards;

	public override List<Card> Cards 
	{ 
		get { return GetTopTerrainCards(); }
	}

	public List<Card> FaceUpCards
	{
		get { return GetFaceUpTerrainCards(); }
	}

	public List<Card> FaceDownCards
	{
		get { return GetFaceDownTerrainCards(); }
	}

	#region Events
	public event System.Action<GameObject> MouseDownEvent;
	public event System.Action<GameObject> MouseDragEvent;
	public event System.Action<GameObject> MouseEnterEvent;
	public event System.Action<GameObject> MouseExitEvent;
	public event System.Action<GameObject> MouseOverEvent;
	public event System.Action<GameObject> MouseUpAsAButtonEvent;
	public event System.Action<GameObject> MouseUpEvent;

	protected virtual void OnMouseDownEvent(GameObject cardGameObject)
	{
		if (MouseDownEvent != null)
			MouseDownEvent(cardGameObject);
	}

	protected virtual void OnMouseDragEvent(GameObject cardGameObject)
	{
		if (MouseDragEvent != null)
			MouseDragEvent(cardGameObject);
	}

	protected virtual void OnMouseEnterEvent(GameObject cardGameObject)
	{
		if (MouseEnterEvent != null)
			MouseEnterEvent(cardGameObject);
	}

	protected virtual void OnMouseExitEvent(GameObject cardGameObject)
	{
		if (MouseExitEvent != null)
			MouseExitEvent(cardGameObject);
	}

	protected virtual void OnMouseOverEvent(GameObject cardGameObject)
	{
		if (MouseOverEvent != null)
			MouseOverEvent(cardGameObject);
	}

	protected virtual void OnMouseUpAsAButtonEvent(GameObject cardGameObject)
	{
		if (MouseUpAsAButtonEvent != null)
			MouseUpAsAButtonEvent(cardGameObject);
	}

	protected virtual void OnMouseUpEvent(GameObject cardGameObject)
	{
		if (MouseUpEvent != null)
			MouseUpEvent(cardGameObject);
	}

	protected override void OnAddCardEvent(Card card)
	{
		base.OnAddCardEvent(card);
	}

	protected override void OnGetCardEvent(Card card)
	{
		base.OnGetCardEvent(card);
	}

	protected override void OnRemoveCardEvent(Card card)
	{
		base.OnRemoveCardEvent(card);
	}
	#endregion

	private List<Card> GetTopTerrainCards()
	{
		List<Card> topTerrainCards = new List<Card>(terrainCards.Count);

		for (int i = 0; i < terrainCards.Count; ++i)
		{
			if (terrainCards[i].Cards.Count == 0)
				continue;
			topTerrainCards.Add(terrainCards[i].Cards[0]);
		}
		return topTerrainCards;
	}

	private List<Card> GetFaceUpTerrainCards()
	{
		List<Card> topTerrainCards = new List<Card>(terrainCards.Count);

		for (int i = 0; i < terrainCards.Count; ++i)
		{
			if (terrainCards[i].Cards.Count == 0)
				continue;
			if (terrainCards[i].Cards[0].IsFaceUp == true)
				topTerrainCards.Add(terrainCards[i].Cards[0]);
		}
		return topTerrainCards;
	}
	private List<Card> GetFaceDownTerrainCards()
	{
		List<Card> topTerrainCards = new List<Card>(terrainCards.Count);

		for (int i = 0; i < terrainCards.Count; ++i)
		{
			if (terrainCards[i].Cards.Count == 0)
				continue;
			if (terrainCards[i].Cards[0].IsFaceUp == false)
				topTerrainCards.Add(terrainCards[i].Cards[0]);
		}
		return topTerrainCards;
	}

	/*
	 * AddCard, RemoveCard & GetCard all act upon terrainCards, not Cards
	 */

	public override Card AddCard(GameObject cardGameObject, bool faceUp = true)
	{
		Card card;

		terrainCards.Sort(delegate(CardCollection x, CardCollection y)
		{
			if (x.Cards.Count == y.Cards.Count)
				return 0;
			else if (x.Cards.Count > y.Cards.Count)
				return 1;
			else
				return -1;
		});

		card = terrainCards[0].AddCard(cardGameObject, faceUp);
		OnAddCardEvent(card);

		SubscribeToInteractionEvents(card.gameObject);

		return card;
	}

	public override Card AddCard(Card card, bool faceUp = true)
	{
		terrainCards.Sort(delegate(CardCollection x, CardCollection y)
		{
			if (x.Cards.Count == y.Cards.Count)
				return 0;
			else if (x.Cards.Count > y.Cards.Count)
				return 1;
			else
				return -1;
		});

		card = terrainCards[0].AddCard(card, faceUp);
		SubscribeToInteractionEvents(card.gameObject);

		return card;
	}

	public override Card AddNewCard(Elements element, bool faceUp = true)
	{
		Card card;

		terrainCards.Sort(delegate(CardCollection x, CardCollection y)
		{
			if (x.Cards.Count == y.Cards.Count)
				return 0;
			else if (x.Cards.Count > y.Cards.Count)
				return 1;
			else
				return -1;
		});

		card = terrainCards[0].AddNewCard(element, faceUp);
		SubscribeToInteractionEvents(card.gameObject);

		return card;
	}

	public override Card AddNewRandomCard(bool faceUp = true)
	{
		Card card;

		terrainCards.Sort(delegate(CardCollection x, CardCollection y)
		{
			if (x.Cards.Count == y.Cards.Count)
				return 0;
			else if (x.Cards.Count > y.Cards.Count)
				return 1;
			else
				return -1;
		});

		card = terrainCards[0].AddNewRandomCard(faceUp);
		SubscribeToInteractionEvents(card.gameObject);

		return card;
	}

	public override Card RemoveCard(GameObject cardGameObject)
	{
		Card card;
		bool contains;

		contains = false;
		card = cardGameObject.GetComponent<Card>();

		for (int i = 0; i < terrainCards.Count; ++i)
		{
			if (terrainCards[i].Cards.Contains(card))
			{
				contains = true;
				terrainCards[i].Cards.Remove(card);
				break;
			}
		}

		if (contains == true)
		{
			UnsubscribeFromInteractionEvents(card.gameObject);

			return card;
		}
		return null;
	}

	public override Card RemoveCard(Card card)
	{
		Card removedCard;
		bool contains;

		contains = false;

		for (int i = 0; i < terrainCards.Count; ++i)
		{
			if (terrainCards[i].Cards.Contains(card))
			{
				contains = true;
				terrainCards[i].Cards.Remove(card);
				break;
			}
		}

		if (contains == true)
		{
			UnsubscribeFromInteractionEvents(card.gameObject);

			return card;
		}
		return null;
	}

	public void SubscribeToInteractionEvents(GameObject cardGameObject)
	{
		Card card = cardGameObject.GetComponent<Card>();

		Debug.Log("SubscribeToInteractionEvents " + cardGameObject.name);
		card.MouseDownEvent += OnMouseDownEvent;
		card.MouseDragEvent += OnMouseDragEvent;
		card.MouseEnterEvent += OnMouseEnterEvent;
		card.MouseExitEvent += OnMouseExitEvent;
		card.MouseOverEvent += OnMouseOverEvent;
		card.MouseUpAsAButtonEvent += OnMouseUpAsAButtonEvent;
		card.MouseUpEvent += OnMouseUpEvent;
	}

	public void UnsubscribeFromInteractionEvents(GameObject cardGameObject)
	{
		Card card = cardGameObject.GetComponent<Card>();

		card.MouseDownEvent -= OnMouseDownEvent;
		card.MouseDragEvent -= OnMouseDragEvent;
		card.MouseEnterEvent -= OnMouseEnterEvent;
		card.MouseExitEvent -= OnMouseExitEvent;
		card.MouseOverEvent -= OnMouseOverEvent;
		card.MouseUpAsAButtonEvent -= OnMouseUpAsAButtonEvent;
		card.MouseUpEvent -= OnMouseUpEvent;
	}
}
