using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardCollection : System.Object
{
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

	public CardCollection()
	{
		AddCardAllowed = true;
		RemoveCardAllowed = true;
		cards = new List<Card>();
	}

	public CardCollection( 
		List<CardSuit> cardList, 
		bool isAddCardAllowed = true, 
		bool isRemoveCardAllowed = true )
	{
		System.Converter<CardSuit, Card> convert;
		convert = new System.Converter<CardSuit, Card>(Card.CardSuitToCardConversion);
		cards = new List<Card>(cardList.Count);
		cards = cardList.ConvertAll<Card>(convert);

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

	public void SetAddRemovePermission( bool isAddCardAllowed, bool isRemoveCardAllowed )
	{
		AddCardAllowed = ( isAddCardAllowed ) ? true : false;
		RemoveCardAllowed = ( isRemoveCardAllowed ) ? true : false;
	}

	//	Does this CardCollection contain this card's opposed suit
	public bool ContainsOpposedCard( Card c )
	{
		return ( cards.Contains((Card)c.opposedSuit) ) ? true : false;
	}

	//	Does this CardCollection contain the opposed suits for any of this 
	//	CardCollection's cards
	public bool ContainsAnyOpposedCards( CardCollection cc )
	{
		int opposedCards = 0;

		for ( int i = 0; i < cards.Count; ++i )
		{
			if ( cc.ContainsOpposedCard(cards[i]) )
				++opposedCards;
		}

		return ( opposedCards > 0 ) ? true : false;
	}
}
