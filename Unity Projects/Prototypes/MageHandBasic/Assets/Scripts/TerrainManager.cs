using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour 
{
	public CardCollection activeCardCollection;
	private Vector3 initPos;
	private bool dragged;
	private bool cardTransferAttempt;
	private CardCollection transferCollection;

	public int initialPerPileCardCount = 3;
	public bool initialFaceUp = true;
	public int terrainPileCount = 12;
	public TerrainCollections tCardCollections;

	public void OnEnable()
	{
		//	Subscribe to all of the MouseDown events
		tCardCollections.MouseDownEvent += OnCardMouseDown;
		tCardCollections.MouseDragEvent += OnCardMouseDrag;
		tCardCollections.MouseUpEvent += OnCardMouseUp;

		//	Subscribe to all of the Add/Remove events
		tCardCollections.AddCardEvent += OnAddTerrainCard;
		tCardCollections.RemoveCardEvent += OnRemoveTerrainCard;

		//	Subscribe to all of the Trigger#2D events
		tCardCollections.TriggerEnter2DEvent += OnCardOverlapEnter;
	}

	public void OnDisable()
	{
		//	Unsubscribe from all of the MouseDown events
		tCardCollections.MouseDownEvent -= OnCardMouseDown;
		tCardCollections.MouseDragEvent -= OnCardMouseDrag;
		tCardCollections.MouseUpEvent -= OnCardMouseUp;

		//	Unsubscribe from all of the Add/Remove events
		tCardCollections.AddCardEvent -= OnAddTerrainCard;
		tCardCollections.RemoveCardEvent -= OnRemoveTerrainCard;

		//	Unsubscribe from all of the Trigger#2D events
		tCardCollections.TriggerEnter2DEvent -= OnCardOverlapEnter;
	}

	public void Start()
	{
		List<CardCollection> cardPiles;
		CardCollection newCardPile;
		cardPiles = tCardCollections.terrainCardPiles;

		Debug.Log("Terrain collections: " + cardPiles.Count + " Terrain cards: " + tCardCollections.Cards.Count);

		if (cardPiles.Count < terrainPileCount)
		{
			for (int i = 0; cardPiles.Count < terrainPileCount; ++i)
			{
				newCardPile = tCardCollections.AddNewCardPile();
				newCardPile.name = "terrainCardPile " + (i + 1);
			}
		}

		for (int i = 0; i < cardPiles.Count; ++i)
		{
			for (int j = 0; j < initialPerPileCardCount; ++j)
			{
				tCardCollections.AddNewRandomCard(cardPiles[i], initialFaceUp);
			}
		}

		tCardCollections.PositionCards();
	}

	//	Subscribed to every card's OnMouseDown event
	public void OnCardMouseDown(GameObject cardGameObject) 
	{
		activeCardCollection =
			tCardCollections.FindCardCollection(cardGameObject);

		initPos = cardGameObject.transform.position;
	}

	//	Subscribed to every card's OnMouseDrag event
	//	Drags selected card under mouse position
	public void OnCardMouseDrag(GameObject cardGameObject)
	{
		dragged = true;
		cardGameObject.transform.position =
			Camera.main
			.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
											Input.mousePosition.y,
											Mathf.Abs(Camera.main.transform.position.z)));
	}

	//	Subscribed to every card's OnMouseUp event
	//	Transfers card to another collection if the card overlaps it
	public void OnCardMouseUp(GameObject cardGameObject) 
	{
		Card removedCard;

		if (dragged == true && cardTransferAttempt == false)
		{
			dragged = false;
			cardGameObject.transform.position = initPos;
		}
		else if (dragged == true && cardTransferAttempt == true)
		{
			dragged = false;
			cardTransferAttempt = false;
			removedCard = tCardCollections.RemoveCard(cardGameObject);
			if (removedCard != null)
				transferCollection.AddCard(cardGameObject, true);
		}

		initPos = this.transform.position;
		activeCardCollection = null;
	}

	public void OnAddTerrainCard(Card card) { }
	public void OnRemoveTerrainCard(Card card) 
	{
		Debug.Log(card.name + " removed");
	}

	//	Subscribed to every card's OverlapEnter event
	//	On card overlap the selected card is added to the cardcollection that its overlapping
	public void OnCardOverlapEnter(GameObject cardGameObject, Collider2D other) 
	{
		CardCollection cCollection;
 		CardCollection otherCCollection;

		cCollection = tCardCollections
			          .FindCardCollection(cardGameObject);
		otherCCollection = other.GetComponent<CardCollection>();

		if (cCollection == activeCardCollection && otherCCollection != null)
		{
			cardTransferAttempt = true;
			transferCollection = otherCCollection;
		}
		else if (cCollection == activeCardCollection && 
			tCardCollections.CardIsInTerrain(other.gameObject) == false)
		{
			otherCCollection = 
				other.transform.parent
				.GetComponent<CardCollection>();

			cardTransferAttempt = true;
			transferCollection = otherCCollection;
		}
	}
}
