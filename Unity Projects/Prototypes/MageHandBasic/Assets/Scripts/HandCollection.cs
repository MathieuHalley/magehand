using UnityEngine;
using System.Collections;

public class HandCollection : CardCollection, IInputInteractionEvents, IObjectInteractionEvents
{
	#region Events
	//	Input Interaction Events
	public event System.Action<GameObject> MouseDownEvent;
	public event System.Action<GameObject> MouseDragEvent;
	public event System.Action<GameObject> MouseEnterEvent;
	public event System.Action<GameObject> MouseExitEvent;
	public event System.Action<GameObject> MouseOverEvent;
	public event System.Action<GameObject> MouseUpAsAButtonEvent;
	public event System.Action<GameObject> MouseUpEvent;
	//	Object Interaction Events
	public event System.Action<GameObject, Collider2D> TriggerEnter2DEvent;
	public event System.Action<GameObject, Collider2D> TriggerExit2DEvent;
	public event System.Action<GameObject, Collider2D> TriggerStay2DEvent;

	#region Input Interaction Events
	protected virtual void OnMouseDownEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mDownEvent = MouseDownEvent;

		if (mDownEvent != null)
			mDownEvent(cardGameObject);
	}

	protected virtual void OnMouseDragEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mDragEvent = MouseDragEvent;

		if (mDragEvent != null)
			mDragEvent(cardGameObject);
	}

	protected virtual void OnMouseEnterEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mEnterEvent = MouseEnterEvent;

		if (mEnterEvent != null)
			mEnterEvent(cardGameObject);
	}

	protected virtual void OnMouseExitEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mExitEvent = MouseExitEvent;

		if (mExitEvent != null)
			mExitEvent(cardGameObject);
	}

	protected virtual void OnMouseOverEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mOverEvent = MouseOverEvent;

		if (mOverEvent != null)
			mOverEvent(cardGameObject);
	}

	protected virtual void OnMouseUpAsAButtonEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mUpAsButtonEvent = MouseUpAsAButtonEvent;

		if (mUpAsButtonEvent != null)
			mUpAsButtonEvent(cardGameObject);
	}

	protected virtual void OnMouseUpEvent(GameObject cardGameObject)
	{
		System.Action<GameObject> mUpEvent = MouseUpEvent;

		if (mUpEvent != null)
			mUpEvent(cardGameObject);
	}
	#endregion
	#region Object Interaction Events
	protected void OnTriggerEnter2DEvent(GameObject cardGameObject, Collider2D other)
	{
		System.Action<GameObject, Collider2D> tEnter2DEvent = TriggerEnter2DEvent;

		if (tEnter2DEvent != null)
			tEnter2DEvent(cardGameObject, other);
	}

	protected void OnTriggerExit2DEvent(GameObject cardGameObject, Collider2D other)
	{
		System.Action<GameObject, Collider2D> tExit2DEvent = TriggerExit2DEvent;

		if (tExit2DEvent != null)
			tExit2DEvent(cardGameObject, other);
	}

	protected void OnTriggerStay2DEvent(GameObject cardGameObject, Collider2D other)
	{
		System.Action<GameObject, Collider2D> tStay2DEvent = TriggerStay2DEvent;

		if (tStay2DEvent != null)
			tStay2DEvent(cardGameObject, other);
	}
	#endregion
	#endregion

	public override void PositionCards()
	{
		for (int i = 0; i < Cards.Count; ++i)
		{
			Cards[i].transform.position = this.transform.position;

			Cards[i].transform.position +=
					new Vector3((i + 0.5f - Cards.Count * 0.5f) * 1.25f, 0f);

			Cards[i].transform.position +=
					new Vector3(Random.Range(-0.05f, 0.05f),
								Random.Range(-0.05f, 0.05f));
		}
	}

	/// <summary>
	///		AddCard - Add a Card to the CardCollection
	/// </summary>
	/// <param name="cardGameObject">The GameObject of the Card Component to be added</param>
	/// <param name="faceUp">Whether the Card is face-up or face-down</param>
	/// <returns>The added Card</returns>
	public override Card AddCard(GameObject cardGameObject, bool faceUp = true)
	{
		Card card = base.AddCard(cardGameObject,faceUp);
		SubscribeToInteractionEvents(card);
		PositionCards();

		return card;
	}

	/// <summary>
	///		AddCard - Add a Card to the CardCollection
	/// </summary>
	/// <param name="cardGameObject">The Card to be added</param>
	/// <param name="faceUp">Whether the Card is face-up or face-down</param>
	/// <returns>The added Card</returns>
	public override Card AddCard(Card card, bool faceUp = true)
	{
		base.AddCard(card,faceUp);
		PositionCards();
		SubscribeToInteractionEvents(card);

		return card;
	}

	/// <summary>
	///		AddNewCard - Add a new Card to the CardCollection of a specific element
	/// </summary>
	/// <param name="element">The element of the new Card</param>
	/// <param name="faceUp">Whether the Card is face-up or face-down</param>
	/// <returns>The new added Card</returns>
	public override Card AddNewCard(Elements element, bool faceUp = true)
	{
		Card card = base.AddNewCard(element,faceUp);
		PositionCards();
		SubscribeToInteractionEvents(card);

		return card;
	}

	/// <summary>
	///		AddNewRandomCard - Add a new card of a random element to the CardCollection
	/// </summary>
	/// <param name="faceUp">Whether the Card is face-up or face-down</param>
	/// <returns>The new added Card</returns>
	public override Card AddNewRandomCard(bool faceUp = true)
	{
		Card card = base.AddNewRandomCard(faceUp);
		PositionCards();
		SubscribeToInteractionEvents(card);

		return card;
	}

	/// <summary>
	///		RemoveCard - Remove a specific Card from the CardCollection
	/// </summary>
	/// <param name="cardGameObject">The GameObject of the Card Component to be removed</param>
	/// <returns>The removed Card</returns>
	public override Card RemoveCard(GameObject cardGameObject)
	{
		Card removedCard = base.RemoveCard(cardGameObject);
		PositionCards();
		UnsubscribeFromInteractionEvents(removedCard);

		return removedCard;
	}

	/// <summary>
	///		RemoveCard - Remove a specific Card from the CardCollection
	/// </summary>
	/// <param name="cardGameObject">The Card to be removed</param>
	/// <returns>The removed Card</returns>
	public override Card RemoveCard(Card card)
	{
		Card removedCard = base.RemoveCard(card);
		PositionCards();
		UnsubscribeFromInteractionEvents(removedCard);

		return removedCard;
	}

	public void SubscribeToInteractionEvents(Card card)
	{
		//Debug.Log("SubscribeToInteractionEvents " + card.name);
		//Input Interaction Events
		card.MouseDownEvent += OnMouseDownEvent;
		card.MouseDragEvent += OnMouseDragEvent;
		card.MouseEnterEvent += OnMouseEnterEvent;
		card.MouseExitEvent += OnMouseExitEvent;
		card.MouseOverEvent += OnMouseOverEvent;
		card.MouseUpAsAButtonEvent += OnMouseUpAsAButtonEvent;
		card.MouseUpEvent += OnMouseUpEvent;
		//Object Interaction Events
		card.TriggerEnter2DEvent += OnTriggerEnter2DEvent;
		card.TriggerExit2DEvent += OnTriggerExit2DEvent;
		card.TriggerStay2DEvent += OnTriggerStay2DEvent;
	}

	public void UnsubscribeFromInteractionEvents(Card card)
	{
		//Debug.Log("UnsubscribeFromInteractionEvents " + card.gameObject.name);
		//Input Interaction Events
		card.MouseDownEvent -= OnMouseDownEvent;
		card.MouseDragEvent -= OnMouseDragEvent;
		card.MouseEnterEvent -= OnMouseEnterEvent;
		card.MouseExitEvent -= OnMouseExitEvent;
		card.MouseOverEvent -= OnMouseOverEvent;
		card.MouseUpAsAButtonEvent -= OnMouseUpAsAButtonEvent;
		card.MouseUpEvent -= OnMouseUpEvent;
		//Object Interaction Events
		card.TriggerEnter2DEvent -= OnTriggerEnter2DEvent;
		card.TriggerExit2DEvent -= OnTriggerExit2DEvent;
		card.TriggerStay2DEvent -= OnTriggerStay2DEvent;
	}
}
