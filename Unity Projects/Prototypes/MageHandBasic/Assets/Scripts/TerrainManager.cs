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
		tCardCollections.MouseDownEvent += OnCardMouseDown;
		tCardCollections.MouseDragEvent += OnCardMouseDrag;
		tCardCollections.MouseUpEvent += OnCardMouseUp;

		tCardCollections.AddCardEvent += OnAddTerrainCard;
		tCardCollections.RemoveCardEvent += OnRemoveTerrainCard;

		tCardCollections.TriggerEnter2DEvent += OnCardOverlapEnter;
	}

	public void OnDisable()
	{
		tCardCollections.MouseDownEvent -= OnCardMouseDown;
		tCardCollections.MouseDragEvent -= OnCardMouseDrag;
		tCardCollections.MouseUpEvent -= OnCardMouseUp;

		tCardCollections.AddCardEvent -= OnAddTerrainCard;
		tCardCollections.RemoveCardEvent -= OnRemoveTerrainCard;

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

	public void OnCardMouseDown(GameObject cardGameObject) 
	{
		activeCardCollection =
			tCardCollections.FindCardCollection(cardGameObject);

		initPos = cardGameObject.transform.position;
	}

	public void OnCardMouseDrag(GameObject cardGameObject)
	{
		dragged = true;
		cardGameObject.transform.position =
			Camera.main
			.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
											Input.mousePosition.y,
											Mathf.Abs(Camera.main.transform.position.z)));
	}

	public void OnCardMouseUp(GameObject cardGameObject) 
	{
		if (dragged == true && cardTransferAttempt == false)
		{
			dragged = false;
			cardGameObject.transform.position = initPos;
		}
		else if (dragged == true && cardTransferAttempt == true)
		{
			dragged = false;
			cardTransferAttempt = false;
			tCardCollections.RemoveCard(cardGameObject);
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
