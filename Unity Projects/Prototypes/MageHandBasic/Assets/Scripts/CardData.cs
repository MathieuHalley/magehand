using UnityEngine;
using System.Collections;


[System.Serializable]
public class CardData : ICardData
{
	[SerializeField]
	private Elements _element;
	public Elements Element
	{ get { return _element; } }
	[SerializeField]
	private bool _isFaceUp;
	public bool IsFaceUp
	{
		get { return _isFaceUp; }
		set { _isFaceUp = value; }
	}

	public CardData()
	{
		_element = Elements.Air;
		_isFaceUp = true;
	}

	public CardData(Elements element, bool faceUp = true)
	{
		_element = element;
		_isFaceUp = faceUp;
	}
}
