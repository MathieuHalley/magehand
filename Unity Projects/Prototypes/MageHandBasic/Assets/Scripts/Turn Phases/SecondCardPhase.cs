using UnityEngine;
using System.Collections;


public class SecondCardPhase : TurnPhase
{
	/*
	 * 3.	Second Card Phase
	 *	Attacker: Play a face-down card that doesn’t oppose the card you played 
	 *	in the First Card phase.
	 *	Defender: Play a face-up card that may oppose the Attacker’s 
	 *	just-played face-down card and that doesn’t oppose the card you played 
	 *	in the First Card phase.
	 */

	private bool isCardPlayedFaceUp;
	public int numCardsDrawn;

	public override void OnEnable()
	{
		base.OnEnable();
		if ( player1.playSpace.cards.Count == 0 || player2.playSpace.cards.Count == 0 )
		{
			TurnPhaseFSM.Instance.PushPhase(GetComponent<FirstCardPhase>());
		}

		Debug.Log("Second Card Play Phase\nPlayer " + initPlayerNum + "'s turn first.");

		numCardsDrawn = 0;
		ActivePlayerTurnStart();
	}

	public void Update()
	{

		isCardPlayedFaceUp = ( initPlayerTurn ) ? false : true;

		//	Draw a card. Face down if you're the initial player, 
		//	face up if you're the other player
		if ( !endPhase && PlayCard(isCardPlayedFaceUp) )
		{
			++numCardsDrawn;
			if ( initPlayerTurn )
			{
				Debug.Log("Second Card Play Phase\nPlayer " + otherPlayerNum + "'s turn.");

				initPlayerTurn = false;
				activePlayer = GameManager.Instance.SwitchActivePlayer();

				ActivePlayerTurnStart();
			}
		}

		//	If it's the other player's turn explore the phase end conditions
		if ( !initPlayerTurn )
		{
			//	If the other player has surrendered the phase
			if ( GameManager.Instance.curTurnSurrendered )
			{
				Debug.Log("The active player has already surrendered the turn");
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
				Debug.Log("Second Card Phase Ended");
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
