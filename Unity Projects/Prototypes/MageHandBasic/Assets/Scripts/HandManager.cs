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
		handCollection.MouseDownEvent += OnCardMouseDown;
		handCollection.MouseDragEvent += OnCardMouseDrag;
		handCollection.MouseUpEvent += OnCardMouseUp;

		handCollection.TriggerEnter2DEvent += OnCardOverlapEnter;
	}

	public void OnDisable()
	{
		handCollection.MouseDownEvent -= OnCardMouseDown;
		handCollection.MouseDragEvent -= OnCardMouseDrag;
		handCollection.MouseUpEvent -= OnCardMouseUp;

		handCollection.TriggerEnter2DEvent -= OnCardOverlapEnter;
	}

	public void Start()
	{
		if (handCollection == null)
			handCollection = GetComponent<HandCollection>();

		for (int i = handCollection.Cards.Count; i < initialCardCount; ++i)
		{
			handCollection.AddNewRandomCard(initialFaceUp);
		}

		handCollection.PositionCards();
	}

	public void OnCardMouseDown(GameObject cardGameObject)
	{
		Debug.Log("OnMouseDownEvent " + cardGameObject.name);
		initPos = cardGameObject.transform.position;
	}

	public void OnCardMouseDrag(GameObject cardGameObject)
	{
		dragged = true;
		cardGameObject.transform.position =
			Camera.main
			.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
											Input.mousePosition.y,
											Mathf.Abs(Camera.main.transform.position.z)));
	}

	public void OnCardMouseUp(GameObject cardGameObject)
	{
		if (dragged == true && cardTransferAttempt == false)
		{
			dragged = false;
			cardGameObject.transform.position = initPos;
		}
		else if (dragged == true && cardTransferAttempt == true)
		{
			//	Occasionally unsubscribes cards without removing them !!!!
			dragged = false;
			cardTransferAttempt = false;
			handCollection.RemoveCard(cardGameObject);
			transferCollection.AddCard(cardGameObject, true);
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
