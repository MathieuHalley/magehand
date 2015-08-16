using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnPhaseFSM : MonoBehaviour
{
	public Stack<TurnPhase> turnPhaseStack;
	public List<TurnPhase> turnPhaseOrder;
	public TurnPhase curPhase
	{
		get
		{
			return (turnPhaseStack.Count > 0) ? turnPhaseStack.Peek() : null;
		}
	}

	public static TurnPhaseFSM Instance
	{
		get;
		private set;
	}
	public int curTurn;

	public void Awake()
	{
		if ( Instance != null && Instance != this )
			Destroy(gameObject);
		else
			Instance = this;

		turnPhaseStack = new Stack<TurnPhase>();

	}

	public void OnEnable()
	{
		curTurn = -1;
	}

	public void Update()
	{
		if ( curPhase != null && curPhase.enabled == false )
		{
			Debug.Log("New Phase");
			PopPhase();
		}

		if ( turnPhaseStack.Count == 0 )
		{
			Debug.Log("Turn " + curTurn + " completed");
			NewTurn();
		}
	}

	//	Begin a new turn by reseting the turnPhaseStack
	public void NewTurn()
	{
		List<TurnPhase> reverseTurnPhaseOrder;
		reverseTurnPhaseOrder = new List<TurnPhase>(turnPhaseOrder);
		reverseTurnPhaseOrder.Reverse();

		Debug.Log("New Turn");
		turnPhaseStack = new Stack<TurnPhase>(reverseTurnPhaseOrder);

		++curTurn;
		GameManager.Instance.curTurnSurrendered = false;
		turnPhaseStack.Peek().enabled = true;
	}

	//	Pop the current phase from the stack and disable it, 
	//	and Push the new phase onto the stack and enable it
	public TurnPhase SwapPhase( TurnPhase newPhase )
	{
		if ( newPhase != curPhase )
		{
			curPhase.enabled = false;
			turnPhaseStack.Pop();
			newPhase.enabled = true;
			turnPhaseStack.Push(newPhase);
		}

		return newPhase;
	}

	//	Pop the current phase from the top of the stack and disable it
	public TurnPhase PopPhase()
	{
		TurnPhase oldPhase = curPhase;
		curPhase.enabled = false;
		turnPhaseStack.Pop();

		if ( turnPhaseStack.Count > 0 )
			curPhase.enabled = true;
		return oldPhase;
	}

	//	Push the new phase onto the stack and enable it
	public TurnPhase PushPhase( TurnPhase newPhase )
	{
		if ( newPhase != curPhase )
		{
			if ( curPhase != null )
				curPhase.enabled = false;
			newPhase.enabled = true;
			turnPhaseStack.Push(newPhase);
		}
		return newPhase;
	}
}
