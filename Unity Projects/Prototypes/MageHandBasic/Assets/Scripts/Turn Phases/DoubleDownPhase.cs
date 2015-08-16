using UnityEngine;
using System.Collections;

public class DoubleDownPhase : TurnPhase
{
	/*
	 * 5.	Double Down (optional; only after an Unsuccessful Attack)
	 * Attacker: Play 2 new cards face-up that don’t oppose each other and 
	 * don’t oppose the 2 cards already played by the Attacker in the 
	 * First Card phase and the Attack Phase.
	 * Defender: Play 2 cards face-up that oppose the 2 new cards played by the 
	 * Attacker in this phase.
	 * or Surrender the Double Down phase and don’t play any new cards.
	 * If only one or neither of the cards played by the Attacker in the 
	 * Double Down phase are opposed by the Defender’s cards, the turn becomes 
	 * a Successful Attack. If both of the cards played by the Attacker in the 
	 * Double Down phase are opposed by the Defender’s cards, the turn becomes 
	 * an Unsuccessful Attack.
	 * 	 
	 */

	public int numCardsDrawn;

	public override void OnEnable()
	{
		base.OnEnable();
		Debug.Log("Double Down Phase.");

		numCardsDrawn = 0;
		ActivePlayerTurnStart();
	}

	public void Update()
	{
		//	If it's the other player's turn and they don't have any of the 
		//	initial player's playSpace cards in their hand, then they have to 
		//	surrender the turn
		if ( !initPlayerTurn && !activePlayer.hand.ContainsAnyOpposedCards(initPlayer.playSpace) )
		{
			GameManager.Instance.curTurnSurrendered = true;
			endPhase = true;
		}

		//	Draw a card
		if ( !endPhase && PlayCard() )
		{
			++numCardsDrawn;

			if ( initPlayerTurn && numCardsDrawn >= 2 )
			{
				Debug.Log("Double Down Phase\nPlayer " + otherPlayerNum + "'s turn.");

				initPlayerTurn = false;

				activePlayer = GameManager.Instance.SwitchActivePlayer();

				ActivePlayerTurnStart();
			}
		}

		//	If it's the other player's turn explore the phase end conditions
		if ( !initPlayerTurn )
		{
			//	the other player has surrendered the phase
			if ( Input.GetKeyDown(GameManager.Instance.surrenderTheTurnKeyCode) )
			{
				Debug.Log("The active player has surrendered the turn");
				GameManager.Instance.curTurnSurrendered = true;
				endPhase = true;
			}
			//	or a total of 4 cards have been drawn by the players
			else if ( numCardsDrawn >= 4 )
			{
				Debug.Log("Both players have played cards");
				endPhase = true;
			}

			if ( endPhase == true )
			{
				Debug.Log("Double Down Phase Ended");
				GameManager.Instance.curTurnIsSuccessfulAttack = IsSuccessfulAttack();
				EndPhase();
			}
		}
	}

	//	Give the active player the permission to remove cards from their hand 
	//	and add them to their playspace
	protected override void ActivePlayerTurnStart()
	{
		base.ActivePlayerTurnStart();
		activePlayer.hand.SetAddRemovePermission(false, true);
		activePlayer.playSpace.SetAddRemovePermission(true, false);
	}
}
