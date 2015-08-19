using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardCollection : System.Object
{
	public static System.Converter<CardSuit, Card> ConvertCardSuitToCard;
	public static System.Converter<Card, CardSuit> ConvertCardToCardSuit;

	public List<Card> cards;
	public bool AddCardAllowed
	{
		get;
		private set;
	}
	public bool RemoveCardAllowed
	{
		get;
		private set;
	}

	static CardCollection()
	{
		ConvertCardSuitToCard = 
			new System.Converter<CardSuit, Card>(Card.ConvertCardSuitToCard);
		ConvertCardToCardSuit = 
			new System.Converter<Card, CardSuit>(Card.ConvertCardToCardSuit);
	}

	public CardCollection()
	{
		AddCardAllowed = true;
		RemoveCardAllowed = true;
		cards = new List<Card>();
	}

	public CardCollection( 
		List<CardSuit> cardSuitList, 
		bool isAddCardAllowed = true, 
		bool isRemoveCardAllowed = true )
	{
		cards = new List<Card>(cardSuitList.Count);
		cards = cardSuitList.ConvertAll<Card>(ConvertCardSuitToCard);

		AddCardAllowed = true;
		RemoveCardAllowed = true;
	}

	public CardCollection( bool isAddCardAllowed, bool isRemoveCardAllowed )
	{
		cards = new List<Card>();
		AddCardAllowed = isAddCardAllowed;
		RemoveCardAllowed = isRemoveCardAllowed;
	}

	public bool AddCard( Card card )
	{
		if ( AddCardAllowed )
		{
			cards.Add(card);
			return true;
		}
		else
			return false;
	}

	public bool RemoveCard( Card card )
	{
		if ( RemoveCardAllowed )
		{
			cards.Remove(card);
			return true;
		}
		else
			return false;
	}

	public void SetAddRemovePermission(
		bool isAddCardAllowed, 
		bool isRemoveCardAllowed )
	{
		AddCardAllowed = ( isAddCardAllowed ) ? true : false;
		RemoveCardAllowed = ( isRemoveCardAllowed ) ? true : false;
	}

	//	Does this CardCollection contain this card's opposed suit
	public bool ContainsOpposedCard( Card c )
	{
		return ( cards.Contains((Card)c.opposedSuit) ) ? true : false;
	}

	//	Does this CardCollection contain all of this CardCollection's opposed suits
	public bool ContainsAllOpposedCards( CardCollection cc )
	{
		if ( cc.cards.Count < cards.Count )
			return false;

		for ( int i = 0; i < cards.Count; ++i )
		{
			if ( !cc.ContainsOpposedCard(cards[i]) )
				return false;
		}
		return true;
	}

	//	Does this CardCollection contain all of this CardCollection's opposed suits
	public bool ContainsAnyOpposedCards(CardCollection cc)
	{
		for ( int i = 0; i < cards.Count; ++i )
		{
			if ( cc.ContainsOpposedCard(cards[i]) )
				return true;
		}

		return false;
	}
}
