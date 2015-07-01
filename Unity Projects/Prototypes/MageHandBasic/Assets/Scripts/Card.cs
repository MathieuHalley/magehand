using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour, ICardData, IInputInteractionEvents, IObjectInteractionEvents
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
	#endregion
	#region Input Interaction Messages

	public void OnMouseDown()
	{
		System.Action<GameObject> mDownEvent = MouseDownEvent;

		if (mDownEvent != null)
			mDownEvent(this.gameObject);
	}

	public void OnMouseDrag()
	{
		System.Action<GameObject> mDragEvent = MouseDragEvent;

		if (mDragEvent != null)
			mDragEvent(this.gameObject);
	}

	public void OnMouseEnter()
	{
		System.Action<GameObject> mEnterEvent = MouseEnterEvent;

		if (mEnterEvent != null)
			mEnterEvent(this.gameObject);
	}

	public void OnMouseExit()
	{
		System.Action<GameObject> mExitEvent = MouseExitEvent;

		if (mExitEvent != null)
			mExitEvent(this.gameObject);
	}

	public void OnMouseOver()
	{
		System.Action<GameObject> mOverEvent = MouseOverEvent;

		if (mOverEvent != null)
			mOverEvent(this.gameObject);
	}

	public void OnMouseUpAsAButton()
	{
		System.Action<GameObject> mUpAsAButtonEvent = MouseUpAsAButtonEvent;

		if (mUpAsAButtonEvent != null)
			mUpAsAButtonEvent(this.gameObject);
	}

	public void OnMouseUp()
	{
		System.Action<GameObject> mUpEvent = MouseUpEvent;

		if (mUpEvent != null)
			mUpEvent(this.gameObject);
	}
	#endregion
	#region Object Interaction Messages
	public void OnTriggerEnter2D(Collider2D other)
	{
		System.Action<GameObject, Collider2D> tEnter2DEvent = TriggerEnter2DEvent;

		if (tEnter2DEvent != null)
			tEnter2DEvent(this.gameObject, other);
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		System.Action<GameObject, Collider2D> tExit2DEvent = TriggerExit2DEvent;

		if (tExit2DEvent != null)
			tExit2DEvent(this.gameObject, other);
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		System.Action<GameObject, Collider2D> tStay2DEvent = TriggerStay2DEvent;

		if (tStay2DEvent != null)
			tStay2DEvent(this.gameObject, other);
	}
	#endregion

	public void FlipCard()
	{
		_isFaceUp = !_isFaceUp;
		cardMesh.transform.Rotate(0, 180, 0);
	}

	public static GameObject NewCard(Elements element, bool faceUp = true)
	{
		GameObject newCardGameObject;
		newCardGameObject = 
			(GameObject)Instantiate(PrefabHandler.Instance.GetCardPrefab(element),
			                        Vector3.zero,
									Quaternion.Euler(0, 0, Random.Range(-1f, 1f)));

		if (faceUp == false)
			newCardGameObject.GetComponent<Card>().FlipCard();

		return newCardGameObject;
	}

	public static bool operator ==(Card c1, Card c2)
	{
		if ((object)c1 == null || (object)c2 == null)
			return false;

		if (c1.Element == c2.Element && c1.IsFaceUp == c2.IsFaceUp)
			return true;
		else
			return false;
	}

	public static bool operator !=(Card c1, Card c2)
	{
		if ((object)c1 == null || (object)c2 == null)
			return true;

		if (c1.Element != c2.Element || c1.IsFaceUp != c2.IsFaceUp)
			return true;
		else
			return false;
	}

	public bool Equals(Card c)
	{
		if ((object)c == null)
			return false;

		if (_element == c.Element && _isFaceUp == c.IsFaceUp)
			return true;
		else
			return false;
	}

	public override bool Equals(object o)
	{
		Card c;

		if (o == null)
			return false;

		c = o as Card;
		if ((object)c == null)
			return false;

		if (_element == c.Element && _isFaceUp == c.IsFaceUp)
			return true;
		else
			return false;
	}

	public override int GetHashCode()
	{
		int hash = 0;
		unchecked
		{
			hash += 31 * _element.GetHashCode();
			hash += 31 * _isFaceUp.GetHashCode();
		}
		return hash;
	}
}
