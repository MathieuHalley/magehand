using UnityEngine;
using System.Collections;

public class AttackPhase : TurnPhase
{
	/*
	 * 4.	Attack Phase
	 *		Attacker: Reveal your face-down card.
	 *		If both of the Attacker’s cards are opposed by the Defender’s cards 
	 *		the attack was an Unsuccessful Attack. If only one or neither of 
	 *		the Attacker’s cards are opposed by the Defender’s cards attack was 
	 *		a Successful Attack.
	 *		Attacker (optional; only after an Unsuccessful Attack): Challenge 
	 *		an Unsuccessful Attack result and initiate a Double Down phase.
	 */

	public KeyCode doubleDownConfirmKeyCode;
	public KeyCode doubleDownCancelKeyCode;
	private bool wasSuccessfulAttack;

	public override void OnEnable()
	{
		base.OnEnable();
		activePlayer.playSpace.cards[1].faceUp = true;
		Debug.Log("Attack Phase.");

		wasSuccessfulAttack = IsSuccessfulAttack();
		if ( !wasSuccessfulAttack )
			Debug.Log("Double down attack?\nYes: " + doubleDownConfirmKeyCode.ToString() + " No: " + doubleDownCancelKeyCode.ToString());
	}

	public void Update()
	{
		if ( !wasSuccessfulAttack )
		{
			if ( Input.GetKeyDown(doubleDownConfirmKeyCode) )
			{
				TurnPhaseFSM.Instance.SwapPhase(GetComponent<DoubleDownPhase>());
			}
			else if ( Input.GetKeyDown(doubleDownCancelKeyCode) )
			{
				endPhase = true;
			}
		}
		else
		{
			endPhase = true;
		}

		if ( endPhase )
		{
			GameManager.Instance.curTurnIsSuccessfulAttack = wasSuccessfulAttack;
			EndPhase();
		}
	}

	public override void OnDisable()
	{
		Debug.Break();
	}
}
