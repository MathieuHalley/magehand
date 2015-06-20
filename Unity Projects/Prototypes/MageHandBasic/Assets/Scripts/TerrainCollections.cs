using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TerrainCardsSubset { Default, TopAll, TopFaceUp, TopFaceDown }

public class TerrainCollections : MonoBehaviour, IInputInteractionEvents, ICardCollectionEvents, IObjectInteractionEvents
{
	public List<CardCollection> terrainCardPiles;
	public List<Card> Cards { get { return GetTerrainCards(TerrainCardsSubset.Default); } }
	public List<Card> TopCards { get { return GetTerrainCards(TerrainCardsSubset.TopAll); } }
	public List<Card> FaceUpCards { get { return GetTerrainCards(TerrainCardsSubset.TopFaceUp); } }
	public List<Card> FaceDownCards { get { return GetTerrainCards(TerrainCardsSubset.TopFaceDown); } }

	#region Events
	//	ICardCollection Events
	public event System.Action<Card> AddCardEvent;
	public event System.Action<Card> RemoveCardEvent;
	public event System.Action<Card> GetCardEvent;

	//	IInputInteraction Events
	public event System.Action<GameObject> MouseDownEvent;
	public event System.Action<GameObject> MouseDragEvent;
	public event System.Action<GameObject> MouseEnterEvent;
	public event System.Action<GameObject> MouseExitEvent;
	public event System.Action<GameObject> MouseOverEvent;
	public event System.Action<GameObject> MouseUpAsAButtonEvent;
	public event System.Action<GameObject> MouseUpEvent;

	//	Object Interaction Events
	public event System.Action<GameObject, Collider2D> TriggerEnter2DEvent;
	public event System.Action<GameObject, Collider2D> TriggerExit2DEvent;
	public event System.Action<GameObject, Collider2D> TriggerStay2DEvent;
	
	#region CardCollection Events
	protected virtual void OnAddCardEvent(Card card)
	{
		System.Action<Card> aCardEvent = AddCardEvent;

		if (aCardEvent != null)
			aCardEvent(card);
	}

	protected virtual void OnRemoveCardEvent(Card card)
	{
		System.Action<Card> rCardEvent = RemoveCardEvent;

		if (rCardEvent != null)
			rCardEvent(card);
	}

	protected virtual void OnGetCardEvent(Card card)
	{
		System.Action<Card> gCardEvent = GetCardEvent;

		if (gCardEvent != null)
			gCardEvent(card);
	}
	#endregion
	#region Input Interaction Events
	protected virtual void OnMouseDownEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mDownEvent = MouseDownEvent;

		if (mDownEvent != null)
			mDownEvent(cardGameObject);
	}

	protected virtual void OnMouseDragEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mDragEvent = MouseDragEvent;

		if (mDragEvent != null)
			mDragEvent(cardGameObject);
	}

	protected virtual void OnMouseEnterEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mEnterEvent = MouseEnterEvent;

		if (mEnterEvent != null)
			mEnterEvent(cardGameObject);
	}

	protected virtual void OnMouseExitEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mExitEvent = MouseExitEvent;

		if (mExitEvent != null)
			mExitEvent(cardGameObject);
	}

	protected virtual void OnMouseOverEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mOverEvent = MouseOverEvent;

		if (mOverEvent != null)
			mOverEvent(cardGameObject);
	}

	protected virtual void OnMouseUpAsAButtonEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mUpAsButtonEvent = MouseUpAsAButtonEvent;

		if (mUpAsButtonEvent != null)
			mUpAsButtonEvent(cardGameObject);
	}

	protected virtual void OnMouseUpEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mUpEvent = MouseUpEvent;

		if (mUpEvent != null)
			mUpEvent(cardGameObject);
	}
	#endregion
	#region Object Interaction Events
	protected void OnTriggerEnter2DEvent(GameObject cardGameObject, Collider2D other)
	{
		System.Action<GameObject, Collider2D> tEnter2DEvent = TriggerEnter2DEvent;

		if (tEnter2DEvent != null)
			tEnter2DEvent(cardGameObject, other);
	}

	protected void OnTriggerExit2DEvent(GameObject cardGameObject, Collider2D other)
	{
		System.Action<GameObject, Collider2D> tExit2DEvent = TriggerExit2DEvent;

		if (tExit2DEvent != null)
			tExit2DEvent(cardGameObject, other);
	}

	protected void OnTriggerStay2DEvent(GameObject cardGameObject, Collider2D other)
	{
		System.Action<GameObject, Collider2D> tStay2DEvent = TriggerStay2DEvent;

		if (tStay2DEvent != null)
			tStay2DEvent(cardGameObject, other);
	}
	#endregion
	#endregion

	/// <summary>
	///		Returns the cards from the terrainCardPiles in a single List of Cards
	/// </summary>
	/// <returns>
	/// 	GetTerrainCardsSelector.Default: Returns all of the cards in terrainCardPiles, sorted by layer
	///		GetTerrainCardsSelector.Top: Returns the top layer of cards in terrainCardPiles
	///		GetTerrainCardsSelector.FaceUp: Returns all of the face-up cards in the top layer of cards in terrainCardPiles
	///		GetTerrainCardsSelector.FaceDown: Returns all of the face-down cards in the top layer of cards in terrainCardPiles
	/// </returns>
	private List<Card> GetTerrainCards(TerrainCardsSubset terrainCardsSubset)
	{
		List<Card> terrainCards = new List<Card>();
		int j = 0;
		int totalCards = 0;

		//	Return all of the terrain cards
		if (terrainCardsSubset == TerrainCardsSubset.Default)
		{
			for (int i = 0; i < terrainCardPiles.Count; ++i)
			{
				totalCards += terrainCardPiles[i].Cards.Count;
			}
			while (terrainCards.Count < totalCards)
			{
				for (int i = 0; i < terrainCardPiles.Count; ++i)
				{
					if (terrainCardPiles[i].Cards.Count > j)
						terrainCards.Add(terrainCardPiles[i].Cards[j]);
				}
				++j;
			}
		}
		else
		{
			for (int i = 0; i < terrainCardPiles.Count; ++i)
			{
				if (terrainCardPiles[i].Cards.Count == 0)
					continue;
				switch (terrainCardsSubset)
				{
					case TerrainCardsSubset.TopAll:
						terrainCards.Add(terrainCardPiles[i].Cards[0]);
						break;
					case TerrainCardsSubset.TopFaceUp:
						if (terrainCardPiles[i].Cards[0].IsFaceUp == true)
							terrainCards.Add(terrainCardPiles[i].Cards[0]);
						break;
					case TerrainCardsSubset.TopFaceDown:
						if (terrainCardPiles[i].Cards[0].IsFaceUp == false)
							terrainCards.Add(terrainCardPiles[i].Cards[0]);
						break;
				}
			}
		}
		return terrainCards;
	}

	/// <summary>
	///		FindCardCollection - Returns the CardCollection in terrainCardPiles that contains a Card
	/// </summary>
	public CardCollection FindCardCollection(Card card)
	{
		for (int i = 0; i < terrainCardPiles.Count; ++i)
		{
			for (int j = 0; j < terrainCardPiles[i].Cards.Count; ++j)
			{
				if (card.gameObject == terrainCardPiles[i].Cards[j].gameObject)
					return terrainCardPiles[i];
			}
		}
		return null;
	}

	/// <summary>
	///		FindCardCollection - Returns the CardCollection in terrainCardPiles that contains a Card
	/// </summary>
	public CardCollection FindCardCollection(GameObject cardGameObject)
	{
		for (int i = 0; i < terrainCardPiles.Count; ++i)
		{
			for (int j = 0; j < terrainCardPiles[i].Cards.Count; ++j)
			{
				if (cardGameObject == terrainCardPiles[i].Cards[j].gameObject)
					return terrainCardPiles[i];
			}
		}
		return null;
	}

	public bool CardIsInTerrain(Card card)
	{
		for (int i = 0; i < terrainCardPiles.Count; ++i)
		{
			for (int j = 0; j < terrainCardPiles[i].Cards.Count; ++j)
			{
				if (card.gameObject == terrainCardPiles[i].Cards[j].gameObject)
					return true;
			}
		}
		return false;
	}

	public bool CardIsInTerrain(GameObject cardGameObject)
	{
		for (int i = 0; i < terrainCardPiles.Count; ++i)
		{
			for (int j = 0; j < terrainCardPiles[i].Cards.Count; ++j)
			{
				if (cardGameObject == terrainCardPiles[i].Cards[j].gameObject)
					return true;
			}
		}
		return false;
	}

	/// <summary>
	///		FindCardOfElement - Finds a Card of a specific Elements
	/// </summary>
	private Card FindCardOfElement(Elements element, TerrainCardsSubset terrainCardSubset = TerrainCardsSubset.TopFaceUp)
	{
		Card foundCard;
		List<Card> cardList = GetTerrainCards(terrainCardSubset);

		foundCard = cardList.Find(delegate(Card card)
		{
			return (card.Element == element) ? true : false;
		});

		if (foundCard != null)
			return foundCard;
		else
			return null;
	}

	public void PositionCards()
	{
		int cardPileCount = terrainCardPiles.Count;

		for (int i = 0; i < cardPileCount; ++i)
		{
			terrainCardPiles[i].transform.position = this.transform.position;

			terrainCardPiles[i].transform.position +=
				(i < (int)(cardPileCount * 0.5f))
				? new Vector3((i + 0.5f - cardPileCount * 0.25f) * 1.5f, 0.75f)
				: new Vector3((i - cardPileCount * 0.5f + 0.5f - cardPileCount * 0.25f) * 1.5f, -0.75f);

			terrainCardPiles[i].transform.position +=
				new Vector3(Random.Range(-0.05f, 0.05f),
							Random.Range(-0.05f, 0.05f));
			
			for (int j = 0; j < terrainCardPiles[i].Cards.Count; ++j)
			{
				terrainCardPiles[i]
					.Cards[j].transform.localPosition =
					new Vector3(0, 0, (terrainCardPiles[i].Cards.Count - j) * 0.1f);
			}
		}
	}

	/*
	 * AddCard, RemoveCard & GetCard all act upon terrainCardPiles, not any of the Card lists
	 */
	#region AddCard
	/// <summary>
	///		AddCard - Add a Card to the smallest CardCollection
	///	</summary>
	/// <param name="card">The Card to be added</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddCard(Card card, bool faceUp = true)
	{
		List<CardCollection> sortedterrainCardPiles;

		sortedterrainCardPiles = terrainCardPiles;
		sortedterrainCardPiles.Sort(
			delegate(CardCollection x, CardCollection y)
			{
				if (x.Cards.Count == y.Cards.Count)
					return 0;
				else if (x.Cards.Count < y.Cards.Count)
					return -1;
				else
					return 1;
			});

		return AddCard(card, terrainCardPiles[0], faceUp);
	}

	/// <summary>
	///		AddCard - Add a Card to the smallest CardCollection
	///	</summary>
	/// <param name="cardGameObject">The GameObject with the Card Component to be added to the CardCollection</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddCard(GameObject cardGameObject, bool faceUp = true)
	{
		List<CardCollection> sortedterrainCardPiles;

		sortedterrainCardPiles = terrainCardPiles;
		sortedterrainCardPiles.Sort(
			delegate(CardCollection x, CardCollection y)
			{
				if (x.Cards.Count == y.Cards.Count)
					return 0;
				else if (x.Cards.Count < y.Cards.Count)
					return -1;
				else
					return 1;
			});

		return AddCard(cardGameObject, terrainCardPiles[0], faceUp);
	}

	/// <summary>
	///		AddCard - Add a Card to a CardCollection
	///	</summary>
	/// <param name="card">The Card that will be added to the CardCollection</param>
	/// <param name="cardCollectionGameObject">The GameObject with the CardCollection Component that the Card will be added to</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddCard(Card card, GameObject cardCollectionGameObject, bool faceUp = true)
	{
		return AddCard(card, cardCollectionGameObject.GetComponent<CardCollection>(), faceUp);
	}

	/// <summary>
	///		AddCard - Add a Card to a CardCollection
	///	</summary>
	/// <param name="cardGameObject">The GameObject with the Card Component to be added to the CardCollection</param>
	/// <param name="cardCollectionGameObject">The GameObject with the CardCollection Component that the Card will be added to</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddCard(GameObject cardGameObject, GameObject cardCollectionGameObject, bool faceUp = true)
	{
		CardCollection cardCollection;
		cardCollection = cardCollectionGameObject.GetComponent<CardCollection>();
		return AddCard(cardGameObject, cardCollection, faceUp);
	}

	/// <summary>
	///		AddCard - Add a Card to a CardCollection
	///	</summary>
	/// <param name="card">The Card that will be added to the CardCollection</param>
	/// <param name="cardCollection">The CardCollection that the Card will be added to</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddCard(Card card, CardCollection cardCollection, bool faceUp = true)
	{
		Card addedCard = cardCollection.AddCard(card, faceUp);

		OnAddCardEvent(card);

		SubscribeToInteractionEvents(addedCard);

		return addedCard;
	}

	/// <summary>
	///		AddCard - Add a Card to a CardCollection
	///	</summary>
	/// <param name="cardGameObject">The GameObject with the Card Component to be added to the CardCollection</param>
	/// <param name="cardCollection">The CardCollection that the Card will be added to</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns> The Card that has been added to the terrain </returns>
	public Card AddCard(GameObject cardGameObject, CardCollection cardCollection, bool faceUp = true)
	{
		Card addedCard = cardCollection.AddCard(cardGameObject, faceUp);

		OnAddCardEvent(addedCard);

		SubscribeToInteractionEvents(addedCard);

		return addedCard;
	}

	/// <summary>
	///		AddNewCard - Add a new Card of a specific element to a CardCollection
	///	</summary>
	/// <param name="element">The element of the new Card</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddNewCard(Elements element, bool faceUp = true)
	{
		List<CardCollection> sortedterrainCardPiles;

		sortedterrainCardPiles = terrainCardPiles;
		sortedterrainCardPiles.Sort(
			delegate(CardCollection x, CardCollection y)
			{
				if (x.Cards.Count == y.Cards.Count)
					return 0;
				else if (x.Cards.Count < y.Cards.Count)
					return -1;
				else
					return 1;
			});

		return AddNewCard(element, terrainCardPiles[0], faceUp);
	}

	/// <summary>
	///		AddNewCard - Add a new Card of a specific element to a CardCollection
	///	</summary>
	/// <param name="element">The element of the new Card</param>
	/// <param name="cardCollectionGameObject">The GameObject with the CardCollection Component that the Card will be added to</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddNewCard(Elements element, GameObject cardCollectionGameObject, bool faceUp = true)
	{
		CardCollection cardCollection;
		cardCollection = cardCollectionGameObject.GetComponent<CardCollection>();
		return AddNewCard(element, cardCollection, faceUp);
	}

	/// <summary>
	///		AddNewCard - Add a new Card of a specific element to a CardCollection
	///	</summary>
	/// <param name="element">The element of the new Card</param>
	/// <param name="cardCollection">The CardCollection that the Card will be added to</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddNewCard(Elements element, CardCollection cardCollection, bool faceUp = true)
	{
		Card newCard = cardCollection.AddNewCard(element, faceUp);

		OnAddCardEvent(newCard);

		SubscribeToInteractionEvents(newCard);

		return newCard;
	}

	/// <summary>
	///		AddNewRandomCard - Add a new random card to the smallest CardCollection
	///	</summary>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddNewRandomCard(bool faceUp = true)
	{
		List<CardCollection> sortedterrainCardPiles;

		sortedterrainCardPiles = terrainCardPiles;
		sortedterrainCardPiles.Sort(
			delegate(CardCollection x, CardCollection y)
			{
				if (x.Cards.Count == y.Cards.Count)
					return 0;
				else if (x.Cards.Count < y.Cards.Count)
					return -1;
				else
					return 1;
			});

		return AddNewRandomCard(terrainCardPiles[0], faceUp);
	}

	/// <summary>
	///		AddNewRandomCard - Add a new random Card to a CardCollection
	///	</summary>
	/// <param name="cardCollectionGameObject">The GameObject with the CardCollection Component that the Card will be added to</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddNewRandomCard(GameObject cardCollectionGameObject, bool faceUp = true)
	{
		CardCollection cardCollection;
		cardCollection = cardCollectionGameObject.GetComponent<CardCollection>();
		return AddNewRandomCard(cardCollection, faceUp);
	}

	/// <summary>
	///		AddNewRandomCard - Add a new random Card to a CardCollection
	///	</summary>
	/// <param name="cardCollection">The CardCollection that the Card will be added to</param>
	/// <param name="faceUp">Whether the added card will be face-up or face-down</param>
	/// <returns>The Card that has been added to the terrain</returns>
	public Card AddNewRandomCard(CardCollection cardCollection, bool faceUp = true)
	{
		Card newRandomCard = cardCollection.AddNewRandomCard(faceUp);

		OnAddCardEvent(newRandomCard);

		SubscribeToInteractionEvents(newRandomCard);

		return newRandomCard;
	}

	public CardCollection AddNewCardPile()
	{
		GameObject newCardPileGO;
		CardCollection newCardPile;

		newCardPileGO =
			(GameObject)Instantiate(
				PrefabHandler.Instance.GetCardPilePrefab(),
				this.transform.position,
				Quaternion.identity);

		newCardPile = 
			newCardPileGO
			.GetComponent<CardCollection>();

		terrainCardPiles.Add(newCardPile);

		newCardPile.transform.parent = this.transform;

		return newCardPile;
	}

	#endregion
	#region RemoveCard
	/// <summary>
	///		RemoveCard - Remove a Card from one of the CardCollections
	/// </summary>
	/// <param name="card">The Card to remove</param>
	/// <returns>The removed Card</returns>
	public Card RemoveCard(Card card, TerrainCardsSubset terrainCardsSubset = TerrainCardsSubset.TopFaceUp)
	{
		List<Card> cardList = GetTerrainCards(terrainCardsSubset);

		if (cardList.Contains(card))
		{
			FindCardCollection(card).RemoveCard(card);
			OnRemoveCardEvent(card);
			UnsubscribeFromInteractionEvents(card);

			return card;
		}
		else
			return null;
	}

	/// <summary>
	///		RemoveCard - Remove a Card from one of the CardCollections
	/// </summary>
	/// <param name="cardGameObject">The GameObject of the Card to remove</param>
	/// <returns>The removed Card</returns>
	public Card RemoveCard(GameObject cardGameObject, TerrainCardsSubset terrainCardsSubset = TerrainCardsSubset.TopFaceUp)
	{
		List<Card> cardList = GetTerrainCards(terrainCardsSubset);
		Card card = cardGameObject.GetComponent<Card>();

		if (cardList.Contains(card))
		{
			FindCardCollection(card).RemoveCard(card);
			OnRemoveCardEvent(card);
			UnsubscribeFromInteractionEvents(card);

			return card;
		}
		else
			return null;
	}

	/// <summary>
	///		RemoveCardOfElement - Remove a Card of a specific Element
	/// </summary>
	/// <param name="element">The element of the card to remove</param>
	/// <param name="terrainCardsSubset">The subset of the terrain cards to look for the Card</param>
	/// <returns>The removed Card</returns>
	public Card RemoveCardOfElement(Elements element, TerrainCardsSubset terrainCardsSubset = TerrainCardsSubset.TopFaceUp)
	{
		Card card = FindCardOfElement(element, terrainCardsSubset);

		if (card != null)
		{
			FindCardCollection(card).RemoveCard(card);
			OnRemoveCardEvent(card);
			UnsubscribeFromInteractionEvents(card);

			return card;
		}
		else
			return null;
	}
	#endregion
	#region GetCard
	/// <summary>
	///		GetCard - Get a specific card from the terrain cardCollections
	/// </summary>
	/// <param name="card">The Card to get</param>
	/// <param name="terrainCardsSubset">The subset of the terrain cards to look for the Card</param>
	/// <returns>The retrieved card</returns>
	public Card GetCard(Card card, TerrainCardsSubset terrainCardsSubset = TerrainCardsSubset.TopFaceUp)
	{
		List<Card> cardList = GetTerrainCards(terrainCardsSubset);

		if (cardList.Contains(card))
		{
			OnGetCardEvent(card);
			return card;
		}
		else
			return null;
	}

	/// <summary>
	///		GetCard - Get a specific card from the terrain cardCollections
	/// </summary>
	/// <param name="cardGameObjec">The GameObject of the Card to get</param>
	/// <param name="terrainCardsSubset">The subset of the terrain cards to look for the Card</param>
	/// <returns>The retrieved card</returns>
	public Card GetCard(GameObject cardGameObject, TerrainCardsSubset terrainCardsSubset = TerrainCardsSubset.TopFaceUp)
	{
		List<Card> cardList = GetTerrainCards(terrainCardsSubset);
		Card card = cardGameObject.GetComponent<Card>();

		if (cardList.Contains(card))
		{
			OnGetCardEvent(card);
			return card;
		}
		else
			return null;
	}

	/// <summary>
	///		GetCardOfElement - Get a Card of a specific element
	/// </summary>
	/// <param name="element">The element of the Card to get</param>
	/// <param name="terrainCardsSubset">The subset of the terrain cards to look for the Card</param>
	/// <returns>The retrieved Card</returns>
	public Card GetCardOfElement(Elements element, TerrainCardsSubset terrainCardsSubset = TerrainCardsSubset.TopFaceUp)
	{
		Card card = FindCardOfElement(element, terrainCardsSubset);

		if (card != null)
		{
			OnGetCardEvent(card);
			return card;
		}

		return null;	
	}

	/// <summary>
	///		GetRandomCard - Get a random Card from the terrain cardCollections
	/// </summary>
	/// <param name="terrainCardsSubset">The subset of the terrain cards to look for the Card</param>
	/// <returns>The retrieved Card</returns>
	public Card GetRandomCard(TerrainCardsSubset terrainCardsSubset = TerrainCardsSubset.TopFaceUp)
	{
		Card card;
		List<Card> cardList = GetTerrainCards(terrainCardsSubset);

		card = cardList[Random.Range(0, cardList.Count)];

		OnGetCardEvent(card);

		return card;
	}

	/// <summary>
	///		GetRandomCardOfElement - Get a random Card of a specific element from the terrain card Collections
	/// </summary>
	/// <param name="element">The element of the card to remove</param>
	/// <param name="terrainCardsSubset">The subset of the terrain cards to look for the Card</param>
	/// <returns>The retrieved Card</returns>
	public Card GetRandomCardOfElement(Elements element, TerrainCardsSubset terrainCardsSubset = TerrainCardsSubset.TopFaceUp)
	{
		Card card;
		List<Card> cardList, elementList;

		cardList = GetTerrainCards(terrainCardsSubset);

		elementList = 
			cardList.FindAll(delegate(Card c)
			{
				return (c.Element == element) ? true : false;
			});

		card = elementList[Random.Range(0, elementList.Count)];

		OnGetCardEvent(card);

		return card;		
	}
	#endregion
	/// <summary>
	///		SubscribeToInteractionEvents - Subscribe to all of a Card's input interaction events
	/// </summary>
	/// <param name="card">The Card being subscribed to</param>
	private void SubscribeToInteractionEvents(Card card)
	{
		//Debug.Log("SubscribeToInteractionEvents " + card.gameObject.name);
		//Input Interaction Events
		card.MouseDownEvent += OnMouseDownEvent;
		card.MouseDragEvent += OnMouseDragEvent;
		card.MouseEnterEvent += OnMouseEnterEvent;
		card.MouseExitEvent += OnMouseExitEvent;
		card.MouseOverEvent += OnMouseOverEvent;
		card.MouseUpAsAButtonEvent += OnMouseUpAsAButtonEvent;
		card.MouseUpEvent += OnMouseUpEvent;
		//Object Interaction Events
		card.TriggerEnter2DEvent += OnTriggerEnter2DEvent;
		card.TriggerExit2DEvent += OnTriggerExit2DEvent;
		card.TriggerStay2DEvent += OnTriggerStay2DEvent;
	}

	/// <summary>
	///		UnsubscribeFromInteractionEvents - Unsubscribe from all of a Card's input interaction events
	/// </summary>
	/// <param name="card">The Card being unsubscribed from</param>
	private void UnsubscribeFromInteractionEvents(Card card)
	{
		//Debug.Log("UnsubscribeFromInteractionEvents " + card.gameObject.name);
		//Input Interaction Events
		card.MouseDownEvent -= OnMouseDownEvent;
		card.MouseDragEvent -= OnMouseDragEvent;
		card.MouseEnterEvent -= OnMouseEnterEvent;
		card.MouseExitEvent -= OnMouseExitEvent;
		card.MouseOverEvent -= OnMouseOverEvent;
		card.MouseUpAsAButtonEvent -= OnMouseUpAsAButtonEvent;
		card.MouseUpEvent -= OnMouseUpEvent;
		//Object Interaction Events
		card.TriggerEnter2DEvent -= OnTriggerEnter2DEvent;
		card.TriggerExit2DEvent -= OnTriggerExit2DEvent;
		card.TriggerStay2DEvent -= OnTriggerStay2DEvent;
	}
}
