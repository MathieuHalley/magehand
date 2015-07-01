using UnityEngine;
using System.Collections;

public class HandManager : MonoBehaviour 
{
	private Vector3 initPos;
	private bool dragged;
	private bool cardTransferAttempt;
	private CardCollection transferCollection;

	public int initialCardCount = 6;
	public bool initialFaceUp = true;
	public HandCollection handCollection;

	public void OnEnable()
	{
		//	Subscribe to all of the MouseDown events
		handCollection.MouseDownEvent += OnCardMouseDown;
		handCollection.MouseDragEvent += OnCardMouseDrag;
		handCollection.MouseUpEvent += OnCardMouseUp;

		//	Subscribe to all of the Trigger#2D events
		handCollection.TriggerEnter2DEvent += OnCardOverlapEnter;
	}

	public void OnDisable()
	{
		//	Unsubscribe from all of the MouseDown events
		handCollection.MouseDownEvent -= OnCardMouseDown;
		handCollection.MouseDragEvent -= OnCardMouseDrag;
		handCollection.MouseUpEvent -= OnCardMouseUp;

		//	Unsubscribe from all of the Trigger#2D events
		handCollection.TriggerEnter2DEvent -= OnCardOverlapEnter;
	}

	public void Start()
	{
		if (handCollection == null)
			handCollection = GetComponent<HandCollection>();

		//	Populate the hand with a random collection of cards
		for (int i = handCollection.Cards.Count; i < initialCardCount; ++i)
		{
			handCollection.AddNewRandomCard(initialFaceUp);
		}

		handCollection.PositionCards();
	}

	//	Subscribed to every card's OnMouseDown event
	public void OnCardMouseDown(GameObject cardGameObject)
	{
		initPos = cardGameObject.transform.position;
	}

	//	Subscribed to every card's OnMouseDrag event
	//	Drags selected card under mouse position
	public void OnCardMouseDrag(GameObject cardGameObject)
	{
		dragged = true;
		cardGameObject.transform.position =
			Camera.main
			.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
											Input.mousePosition.y,
											Mathf.Abs(Camera.main.transform.position.z)));
	}

	//	Subscribed to every card's OnMouseUp event
	//	Transfers card to another collection if the card overlaps it
	public void OnCardMouseUp(GameObject cardGameObject)
	{
		Card removedCard;
		if (dragged == true && cardTransferAttempt == false)
		{
			dragged = false;
			cardGameObject.transform.position = initPos;
		}
		else if (dragged == true && cardTransferAttempt == true)
		{
			//	Calls CardCollection.AddCard not TerrainCollection.AddCard and bypasses all of the terrain event subscription code
			dragged = false;
			cardTransferAttempt = false;
			removedCard = handCollection.RemoveCard(cardGameObject);
			Debug.Log("removedCard = " + removedCard);
			if (removedCard != null)
			{
				transferCollection.AddCard(cardGameObject, true);
			}
		}
		initPos = this.transform.position;
	}

	public void OnCardOverlapEnter(GameObject cardGameObject, Collider2D other)
	{
		CardCollection otherCCollection;

		if (handCollection.CardIsInCollection(other.gameObject) == false)
		{
			otherCCollection = other.GetComponent<CardCollection>();

			if (otherCCollection == null)
			{
				otherCCollection =
					other.transform.parent
					.GetComponent<CardCollection>();
			}

			cardTransferAttempt = true;
			transferCollection = otherCCollection;
		}
	}

}
