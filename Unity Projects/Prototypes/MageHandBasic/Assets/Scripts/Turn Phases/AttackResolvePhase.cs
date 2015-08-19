using UnityEngine;
using System.Collections;

public class AttackResolvePhase : TurnPhase
{
	/*
	 * 4.	Attack Resolve Phase
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
	public bool wasSuccessfulAttack;
	public bool canDoubleDown;

	public override void OnEnable()
	{
		base.OnEnable();
		for ( int i = 0; i < player1.playSpace.cards.Count; ++i )
		{
			player1.playSpace.cards[i].faceUp = true;
		}

		for ( int i = 0; i < player2.playSpace.cards.Count; ++i )
		{
			player2.playSpace.cards[i].faceUp = true;
		}

		Debug.Log("Attack Resolve Phase.");

		wasSuccessfulAttack = IsSuccessfulAttack();
		GameManager.Instance.curTurnIsSuccessfulAttack = wasSuccessfulAttack;
		canDoubleDown = GameManager.Instance.curTurnCanDoubleDown;

		if ( !wasSuccessfulAttack && canDoubleDown )
		{
			Debug.Log("Double down attack?\nYes: " 
				+ doubleDownConfirmKeyCode.ToString() 
				+ " No: " + doubleDownCancelKeyCode.ToString());		
		}
	}

	public void Update()
	{
		if ( !wasSuccessfulAttack && canDoubleDown )
		{
			if ( Input.GetKeyDown(doubleDownConfirmKeyCode) )
			{
				TurnPhaseFSM.Instance.SwapPhase(GetComponent<DoubleDownPhase>());
				GameManager.Instance.curTurnCanDoubleDown = false;
			}
			else if ( Input.GetKeyDown(doubleDownCancelKeyCode) )
			{
				GameManager.Instance.curTurnCanDoubleDown = false;
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

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	protected bool IsSuccessfulAttack()
	{
		CardCollection initPSpace;
		CardCollection otherPSpace;

		initPSpace =
			(initPlayerNum == 1) ? player1.playSpace : player2.playSpace;
		otherPSpace =
			(initPlayerNum == 1) ? player2.playSpace : player1.playSpace;

		return ! otherPSpace.ContainsAllOpposedCards(initPSpace);
	}

}
