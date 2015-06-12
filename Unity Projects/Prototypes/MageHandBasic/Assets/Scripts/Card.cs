using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour, ICardData, ICardDataEvents, IMouseInteractionEvents
{
	[SerializeField]
	protected Elements _element;
	public Elements Element { get { return _element; } }
	[SerializeField]
	protected bool _isFaceUp;
	public bool IsFaceUp
	{
		get { return _isFaceUp; }
		set 
		{
			if (_isFaceUp != value)
				FlipCard();
		}
	}

	public GameObject cardMesh;

	#region Events
	public event System.Action<bool> FlipCardEvent;
	public event System.Action<GameObject> MouseDownEvent;
	public event System.Action<GameObject> MouseDragEvent;
	public event System.Action<GameObject> MouseEnterEvent;
	public event System.Action<GameObject> MouseExitEvent;
	public event System.Action<GameObject> MouseOverEvent;
	public event System.Action<GameObject> MouseUpAsAButtonEvent;
	public event System.Action<GameObject> MouseUpEvent;

	protected virtual void OnFlipCardEvent(bool faceUp) 
	{
		if (FlipCardEvent != null)
			FlipCardEvent(faceUp);
	}

	protected virtual void OnMouseDown()
	{
		if (MouseDownEvent != null)
			MouseDownEvent(this.gameObject);
	}

	protected virtual void OnMouseDrag()
	{
		if (MouseDragEvent != null)
			MouseDragEvent(this.gameObject);
	}

	protected virtual void OnMouseEnter()
	{
		if (MouseEnterEvent != null)
			MouseEnterEvent(this.gameObject);
	}

	protected virtual void OnMouseExit()
	{
		if (MouseExitEvent != null)
			MouseExitEvent(this.gameObject);
	}

	protected virtual void OnMouseOver()
	{
		if (MouseOverEvent != null)
			MouseOverEvent(this.gameObject);
	}

	protected virtual void OnMouseUpAsAButton()
	{
		if (MouseUpAsAButtonEvent != null)
			MouseUpAsAButtonEvent(this.gameObject);
	}

	protected virtual void OnMouseUp()
	{
		if (MouseUpEvent != null)
			MouseUpEvent(this.gameObject);
	}
	#endregion

	public void OnEnable()
	{
		FlipCardEvent += OnFlipCardEvent;
	}

	public void OnDisable()
	{
		FlipCardEvent -= OnFlipCardEvent;
	}

	public void FlipCard()
	{
		_isFaceUp = !_isFaceUp;
		cardMesh.transform.Rotate(0, 180, 0);
		OnFlipCardEvent(_isFaceUp);
	}

	public static GameObject NewCard(Elements element, bool faceUp = true)
	{
		GameObject newCardGameObject;
		newCardGameObject = Instantiate<GameObject>(PrefabHandler.Instance.GetCardPrefab(element));
		if (faceUp == false)
			newCardGameObject.GetComponent<Card>().FlipCard();

		return newCardGameObject;
	}
}
