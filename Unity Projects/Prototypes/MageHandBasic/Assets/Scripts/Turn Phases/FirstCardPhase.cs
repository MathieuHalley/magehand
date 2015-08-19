using UnityEngine;
using System.Collections;

public class FirstCardPhase : PlayPhase
{
	/*
	 * 2.	First Card
	 *	Attacker: Play 1 card face-up. 
	 *	Defender: Play 1 card face-up that opposes the card played 
	 *		by the Attacker. Or Surrender the Turn, and don’t play any cards 
	 *		for the rest of the turn. 
	 *		If the Defender doesn’t have any cards in their hand 
	 *		that can oppose the card played by the Attacker, 
	 *		they must Surrender the Turn.
	 */
	private bool handIsEmpty;

	public override void OnEnable()
	{
		phaseName = "First Card Phase";
		base.OnEnable();
	}

	public void Update()
	{
		//	If it's the other player's turn and they don't have any of the 
		//	initial player's playSpace cards in their hand, then they have to 
		//	surrender the turn
		if ( !initPlayerTurn && 
			 !activePlayer.hand.ContainsAnyOpposedCards(initPlayer.playSpace) )
		{
			GameManager.Instance.curTurnSurrendered = true;
			endPhase = true;
		}

		//	Draw a card
		if ( !endPhase && numCardsDrawn < 2 )
		{
			if ( PlayCard() )
			{
				++numCardsDrawn;
				if ( initPlayerTurn )
				{
					Debug.Log(phaseName + "\nPlayer " + 
						      otherPlayerNum + "'s turn.");

					initPlayerTurn = false;

					activePlayer = GameManager.Instance.SwitchActivePlayer();
					ActivePlayerTurnStart();
				}
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
			//	or a total of 2 cards have been drawn by the players
			else if ( numCardsDrawn >= 2 )
			{
				Debug.Log("Both players have played cards");
				endPhase = true;
			}

			if ( endPhase )
			{
				Debug.Log("First Card Phase Ended");
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
