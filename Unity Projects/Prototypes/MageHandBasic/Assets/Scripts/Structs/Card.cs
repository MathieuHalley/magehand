using UnityEngine;
using System.Collections;

public enum CardSuit
{
	Air = 0,
	Earth = 1,
	Fire = 2,
	Water = 3
}

public class Card : System.Object, System.IEquatable<Card>
{
	public bool faceUp;
	public CardSuit suit;
	public CardSuit opposedSuit;

	public Card( CardSuit cs, bool fUp = true )
	{
		faceUp = fUp;
		suit = cs;
		opposedSuit = GetOpposedSuit(cs);
	}

	public static CardSuit GetOpposedSuit( CardSuit cs )
	{
		CardSuit opposed;

		if ( (int)cs == 0 )
			opposed = (CardSuit)1;
		else if ( (int)cs == 1 )
			opposed = (CardSuit)0;
		else if ( (int)cs == 2 )
			opposed = (CardSuit)3;
		else
			opposed = (CardSuit)2;

		return opposed;
	}

	//	CardSuit enum to Card conversion
	public static Card CardSuitToCardConversion(CardSuit cs)
	{
		return new Card(cs);
	}
	//	Card to CardSuit enum conversion
	public static CardSuit CardToCardSuitConversion(Card c)
	{
		return c.suit;
	}

	//	CardSuit to Card explicit conversion operator
	public static explicit operator Card(CardSuit cs)
	{
		return CardSuitToCardConversion(cs);
	}

	//	Card to CardSuit explicit conversion operator
	public static explicit operator CardSuit(Card c)
	{
		return CardToCardSuitConversion(c);
	}

	//	Card Card equality operator
	public static bool operator ==(Card c1, Card c2)
	{
		if ( object.ReferenceEquals(c1, c2) )
			return true;
		if ( object.ReferenceEquals(c1, null) )
			return false;
		if ( object.ReferenceEquals(c2, null) )
			return false;
		return c1.Equals(c2);
	}

	//	Card Card inequality operator
	public static bool operator !=(Card c1, Card c2)
	{
		if ( object.ReferenceEquals(c1, c2) )
			return false;
		if ( object.ReferenceEquals(c1, null) )
			return true;
		if ( object.ReferenceEquals(c2, null) )
			return true;
		return c1.Equals(c2);
	}

	public bool Equals(Card c)
	{
		return (suit == c.suit) ? true : false;
	}

	public override bool Equals(object obj)
	{
		Card c = obj as Card;
		if ( obj != null )
			return Equals(c);
		else
			return false;
	}

	public override int GetHashCode()
	{
		int hash = 31;
		hash += suit.GetHashCode() * 31;
		hash += base.GetHashCode() * 31;
		return hash;
	}

	public override string ToString()
	{
		return suit.ToString();
	}

}
