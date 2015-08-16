using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public bool active;
	public CardCollection hand;
	public CardCollection playSpace;
	public CardCollection scorePile;
	public CardSuit player1Suit;

	public List<CardSuit> handCardSuits;
	public List<CardSuit> playSpaceCardSuits;
	public List<CardSuit> scorePileCardSuits;

	private System.Converter<Card, CardSuit> cardToCardSuit;

	public void Start()
	{
		int numCardSuits;
		List<CardSuit> tempCards;

		cardToCardSuit = new System.Converter<Card, CardSuit>(Card.CardToCardSuitConversion);

		hand = new CardCollection();
		playSpace = new CardCollection();
		scorePile = new CardCollection();

		UpdateCardSuitSummaries();

		numCardSuits = System.Enum.GetValues(typeof(CardSuit)).Length;

		tempCards = new List<CardSuit>(GameManager.Instance.initPlayerHandSize);

		//	Add random cards to the player's hand
		for ( int i = 0; i < tempCards.Capacity; ++i )
		{
			tempCards.Add((CardSuit)Random.Range(0, numCardSuits));
		}

		hand = new CardCollection(tempCards);
	}

	public void Update()
	{
		UpdateCardSuitSummaries();
	}

	private void UpdateCardSuitSummaries()
	{
		handCardSuits = hand.cards.ConvertAll<CardSuit>(cardToCardSuit);
		playSpaceCardSuits = playSpace.cards.ConvertAll<CardSuit>(cardToCardSuit);
		scorePileCardSuits = scorePile.cards.ConvertAll<CardSuit>(cardToCardSuit);
	}

	public void ResetAllAddRemovePermissions()
	{
		hand.SetAddRemovePermission(false, false);
		playSpace.SetAddRemovePermission(false, false);
		scorePile.SetAddRemovePermission(false, false);
	}
}
