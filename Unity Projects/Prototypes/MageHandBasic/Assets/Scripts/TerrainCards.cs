using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainCards : MonoBehaviour
{
	public bool active;
	public CardCollection visibleTerrain;
	public List<CardSuit> visibleTerrainCardSuits;

	private System.Converter<Card, CardSuit> cardToCardSuit;

	public void Start()
	{
		int suitCount;
		List<CardSuit> tempCards;

		//	The number of cards of each suit that will appear in the visibleTerrain
		suitCount = (int)(GameManager.Instance.initTerrainCardCount * 0.25f);

		tempCards = 
			new List<CardSuit>(GameManager.Instance.initTerrainCardCount);

		//	Add and then shuffle an evenly distributed list of cards 
		//	to the visibleTerrain
		for ( int i = 0; i < suitCount; ++i )
		{
			tempCards.Add(CardSuit.Air);
			tempCards.Add(CardSuit.Earth);
			tempCards.Add(CardSuit.Fire);
			tempCards.Add(CardSuit.Water);
		}

		tempCards = GameManager.Shuffle<CardSuit>(tempCards);
		visibleTerrain = new CardCollection(tempCards);
		visibleTerrainCardSuits =
			visibleTerrain.cards
			.ConvertAll<CardSuit>(CardCollection.ConvertCardToCardSuit);
	}

	public void Update()
	{
		visibleTerrainCardSuits = 
			visibleTerrain.cards
			.ConvertAll<CardSuit>(CardCollection.ConvertCardToCardSuit);
	}
}
