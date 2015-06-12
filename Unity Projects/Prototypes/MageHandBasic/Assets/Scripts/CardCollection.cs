using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CardCollection : MonoBehaviour, ICardCollection, ICardCollectionEvents
{
	[SerializeField]
	protected List<Card> _cards;
	public virtual List<Card> Cards
	{
		get { return _cards; }
		set { _cards = value; }
	}

	#region Events
	public event System.Action<Card> AddCardEvent;
	public event System.Action<Card> RemoveCardEvent;
	public event System.Action<Card> GetCardEvent;

	protected virtual void OnAddCardEvent(Card card) 
	{
		if (AddCardEvent != null)
			AddCardEvent(card);
	}

	protected virtual void OnRemoveCardEvent(Card card) 
	{
		if (RemoveCardEvent != null)
			RemoveCardEvent(card);
	}

	protected virtual void OnGetCardEvent(Card card) 
	{
		if (GetCardEvent != null)
			GetCardEvent(card);
	}
	#endregion

	public void OnEnable()
	{
		AddCardEvent += OnAddCardEvent;
		RemoveCardEvent += OnRemoveCardEvent;
		GetCardEvent += OnGetCardEvent;
	}

	public void OnDisable()
	{
		AddCardEvent -= OnAddCardEvent;
		RemoveCardEvent -= OnRemoveCardEvent;
		GetCardEvent -= OnGetCardEvent;
	}

	/// <summary>
	///		AddCard
	///			Adds a Card to the List _cards
	/// </summary>
	/// <param name="cardGameObject">The GameObject containing the Card Component to be added</param>
	/// <param name="faceUp">Whether the Card is face up or face down, defaults to true</param>
	public virtual Card AddCard(GameObject cardGameObject, bool faceUp = true)
	{
		Card card = cardGameObject.GetComponent<Card>();
		
		if (card != null)
		{
			card.IsFaceUp = faceUp;
			_cards.Add(card);
			card.transform.parent = this.transform;

			if (AddCardEvent != null)
				AddCardEvent(card);
		}

		return card;
	}

	/// <summary>
	///		AddCard
	///			Adds a Card to the List _cards
	/// </summary>
	/// <param name="card">The Card Component to be added</param>
	/// <param name="faceUp">Whether the Card is face up or face down, defaults to true</param>
	public virtual Card AddCard(Card card, bool faceUp = true) 
	{
		card.transform.parent = this.transform;
		card.IsFaceUp = faceUp;
		_cards.Add(card);

		if (AddCardEvent != null)
			AddCardEvent(card);

		return card;
	}

	/// <summary>
	///		AddNewCard
	///			Adds a new Card to the List _cards
	/// </summary>
	/// <param name="element">The element of the Card to be added</param>
	/// <param name="faceUp">Whether the Card is face up or face down, defaults to true</param>
	public virtual Card AddNewCard(Elements element, bool faceUp = true)
	{
		GameObject cardGameObject;
		Card card;

		cardGameObject = Card.NewCard(element, faceUp);
		card = cardGameObject.GetComponent<Card>();
		cardGameObject.transform.parent = this.transform;
		card.IsFaceUp = faceUp;
		_cards.Add(card);

		if (AddCardEvent != null)
			AddCardEvent(card);

		return card;
	}

	/// <summary>
	///		AddNewRandomCard
	///			Adds a new random Card to the List _cards
	/// </summary>
	/// <param name="faceUp">Whether the Card is face up or face down, defaults to true</param>
	public virtual Card AddNewRandomCard(bool faceUp = true)
	{
		System.Array elementArr;
		GameObject cardGameObject;
		Card card;

		elementArr = System.Enum.GetValues(typeof(Elements));
		cardGameObject = Card.NewCard((Elements)elementArr.GetValue(Random.Range(0, elementArr.Length)), faceUp);
		cardGameObject.transform.parent = this.transform;
		card = cardGameObject.GetComponent<Card>();
		card.IsFaceUp = faceUp;
		_cards.Add(card);

		if (AddCardEvent != null)
			AddCardEvent(card);

		return card;
	}
	/// <summary>
	///		RemoveCard
	///			Removes a Card from the List _cards
	/// </summary>
	/// <param name="cardGameObject">The GameObject containing the Card to be removed from _cards</param>
	/// <returns>RemoveCard returns the removed Card if it was in _cards, null otherwise</returns>
	public virtual Card RemoveCard(GameObject cardGameObject)
	{
		Card removeCard;
		Card card = cardGameObject.GetComponent<Card>();

		removeCard = _cards.Find(delegate(Card c)
		{
			return (c.Element == card.Element) ? true : false;
		});

		if (_cards.Remove(removeCard))
		{
			if (RemoveCardEvent != null)
				RemoveCardEvent(removeCard);
			return removeCard;
		}
		else
			return null;
	}

	/// <summary>
	///		RemoveCard
	///			Removes a Card from the List _cards
	/// </summary>
	/// <param name="card">The Card to be removed from _cards</param>
	/// <returns>RemoveCard returns the removed Card if it was in _cards, null otherwise</returns>
	public virtual Card RemoveCard(Card card) 
	{
		Card removeCard;
		
		removeCard = _cards.Find(delegate(Card c)
		{
			return (c.Element == card.Element) ? true : false;
		});

		if (_cards.Remove(removeCard))
		{
			if (RemoveCardEvent != null)
				RemoveCardEvent(removeCard);
			return removeCard;
		}
		else
			return null;
	}

	/// <summary>
	///		GetCard
	///			Get a Card of a specific element
	/// </summary>
	/// <param name="element">The element to find a card of in _cards</param>
	/// <returns>GetCard returns a card of that element if one was in _cards, null otherwise</returns>
	public virtual Card GetCard(Elements element) 
	{
		Card gotCard;
		
		gotCard = _cards.Find(delegate(Card c)
		{
			return (c.Element == element) ? true : false;
		});
		
		if (GetCardEvent != null)
			GetCardEvent(gotCard);
		
		return gotCard;
	}

	/// <summary>
	///		GetRandomCard
	///			returns a random Card from _cards
	/// </summary>
	public virtual Card GetRandomCard()
	{
		Card gotCard;
		
		gotCard = _cards[Random.Range(0, _cards.Count - 1)];
		if (GetCardEvent != null)
			GetCardEvent(gotCard);
		
		return gotCard;
	}

	/// <summary>
	///		GetRandomCard
	///			returns a random Card with an Element equal to element from _cards
	/// </summary>
	public virtual Card GetRandomCard(Elements element) 
	{
		List<Card> eList;
		Card gotCard;

		eList = _cards.FindAll(delegate(Card c)
		{
			return (c.Element == element) ? true : false;
		});

		gotCard = eList[Random.Range(0, eList.Count - 1)];

		if (GetCardEvent != null)
			GetCardEvent(gotCard);
		
		return gotCard;
	}

}
