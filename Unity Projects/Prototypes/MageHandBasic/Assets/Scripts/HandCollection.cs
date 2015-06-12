using UnityEngine;
using System.Collections;

public class HandCollection : CardCollection, IInteractable
{
	public int initialCardCount = 6;
	public bool initialFaceUp = true;

	private Vector3 initPos;
	private bool dragged;

	#region Events
	public event System.Action<GameObject> MouseDownEvent;
	public event System.Action<GameObject> MouseDragEvent;
	public event System.Action<GameObject> MouseEnterEvent;
	public event System.Action<GameObject> MouseExitEvent;
	public event System.Action<GameObject> MouseOverEvent;
	public event System.Action<GameObject> MouseUpAsAButtonEvent;
	public event System.Action<GameObject> MouseUpEvent;

	protected virtual void OnMouseDownEvent(GameObject cardGameObject)
	{
		Debug.Log("OnMouseDownEvent " + cardGameObject.name);
		initPos = cardGameObject.transform.position;

		if (MouseDownEvent != null)
			MouseDownEvent(cardGameObject);
	}

	protected virtual void OnMouseDragEvent(GameObject cardGameObject)
	{
		dragged = true;
		cardGameObject.transform.position =
			Camera.main
			.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
											Input.mousePosition.y,
											Mathf.Abs(Camera.main.transform.position.z)));

		if (MouseDragEvent != null)
			MouseDragEvent(cardGameObject);
	}

	protected virtual void OnMouseEnterEvent(GameObject cardGameObject)
	{
		if (MouseEnterEvent != null)
			MouseEnterEvent(cardGameObject);
	}

	protected virtual void OnMouseExitEvent(GameObject cardGameObject)
	{
		if (MouseExitEvent != null)
			MouseExitEvent(cardGameObject);
	}

	protected virtual void OnMouseOverEvent(GameObject cardGameObject)
	{
		if (MouseOverEvent != null)
			MouseOverEvent(cardGameObject);
	}

	protected virtual void OnMouseUpAsAButtonEvent(GameObject cardGameObject)
	{
		if (MouseUpAsAButtonEvent != null)
			MouseUpAsAButtonEvent(cardGameObject);
	}

	protected virtual void OnMouseUpEvent(GameObject cardGameObject)
	{
		if (dragged == true)
		{
			cardGameObject.transform.position = initPos;
			dragged = false;
		}
		initPos = this.transform.position;

		if (MouseUpEvent != null)
			MouseUpEvent(cardGameObject);
	}
	#endregion

	public void Start()
	{
		for (int i = _cards.Count; i < initialCardCount; ++i)
		{
			AddNewRandomCard(initialFaceUp);
		}

		for (int i = 0; i < _cards.Count; ++i)
		{
			_cards[i].transform.position
				= this.transform.position
				+ new Vector3((i + 0.5f - _cards.Count * 0.5f) * 1.5f, 0f);
		}
	}

	public override Card AddCard(GameObject cardGameObject, bool faceUp = true)
	{
		Card card;

		card = base.AddCard(cardGameObject,faceUp);
		SubscribeToInteractionEvents(card.gameObject);

		return card;
	}

	public override Card AddCard(Card card, bool faceUp = true)
	{
		base.AddCard(card,faceUp);
		SubscribeToInteractionEvents(card.gameObject);

		return card;
	}

	public override Card AddNewCard(Elements element, bool faceUp = true)
	{
		Card card;

		card = base.AddNewCard(element,faceUp);
		SubscribeToInteractionEvents(card.gameObject);

		return card;
	}

	public override Card AddNewRandomCard(bool faceUp = true)
	{
		Card card;

		card = base.AddNewRandomCard(faceUp);
		SubscribeToInteractionEvents(card.gameObject);

		return card;
	}

	public override Card RemoveCard(GameObject cardGameObject)
	{
		Card removedCard;
		
		removedCard = base.RemoveCard(cardGameObject);
		UnsubscribeFromInteractionEvents(removedCard.gameObject);

		return removedCard;
	}

	public override Card RemoveCard(Card card)
	{
		Card removedCard;

		removedCard = base.RemoveCard(card);
		UnsubscribeFromInteractionEvents(removedCard.gameObject);

		return removedCard;
	}

	public void SubscribeToInteractionEvents(GameObject cardGameObject)
	{
		Card card = cardGameObject.GetComponent<Card>();

		Debug.Log("SubscribeToInteractionEvents " + cardGameObject.name);
		card.MouseDownEvent += OnMouseDownEvent;
		card.MouseDragEvent += OnMouseDragEvent;
		card.MouseEnterEvent += OnMouseEnterEvent;
		card.MouseExitEvent += OnMouseExitEvent;
		card.MouseOverEvent += OnMouseOverEvent;
		card.MouseUpAsAButtonEvent += OnMouseUpAsAButtonEvent;
		card.MouseUpEvent += OnMouseUpEvent;
	}

	public void UnsubscribeFromInteractionEvents(GameObject cardGameObject)
	{
		Card card = cardGameObject.GetComponent<Card>();

		card.MouseDownEvent -= OnMouseDownEvent;
		card.MouseDragEvent -= OnMouseDragEvent;
		card.MouseEnterEvent -= OnMouseEnterEvent;
		card.MouseExitEvent -= OnMouseExitEvent;
		card.MouseOverEvent -= OnMouseOverEvent;
		card.MouseUpAsAButtonEvent -= OnMouseUpAsAButtonEvent;
		card.MouseUpEvent -= OnMouseUpEvent;
	}
}
