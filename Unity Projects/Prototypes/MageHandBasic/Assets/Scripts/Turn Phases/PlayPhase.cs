using UnityEngine;
using System.Collections;

public class PlayPhase : TurnPhase
{
	protected int cardsForPhase;
	protected bool playCardFaceUp;
	protected int numCardsDrawn;

	public int cardsPerPlayer;
	public bool initPlaysCardsFaceUp;
	public bool otherPlaysCardsFaceUp;
	public bool otherPlayerCanSurrender;

	public override void OnEnable ()
	{
		base.OnEnable();
		numCardsDrawn = 0;
		cardsForPhase = cardsPerPlayer * 2;
		ActivePlayerTurnStart();
	}

	void Update()
	{
		if ( !initPlayerTurn && EndConditionsMet() )
		{
			EndPhase();
		}

		//	Draw a card
		if ( numCardsDrawn < cardsForPhase )
		{
			playCardFaceUp = (initPlayer) 
				             ? initPlaysCardsFaceUp : otherPlaysCardsFaceUp;

			if ( PlayCard(playCardFaceUp) )
			{
				++numCardsDrawn;
				if ( initPlayerTurn && numCardsDrawn >= cardsPerPlayer )
				{
					initPlayerTurn = false;

					activePlayer = GameManager.Instance.SwitchActivePlayer();
					ActivePlayerTurnStart();
				}
			}
		}
	}

	//	If it's the other player's turn explore the phase end conditions
	protected bool EndConditionsMet()
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
		//	If the other player has surrendered the phase
		if ( GameManager.Instance.curTurnSurrendered )
		{
			Debug.Log("Player " + otherPlayerNum +" has already surrendered the turn");
			endPhase = true;
		}
		//	or a total of 2 cards have been drawn by the players
		else if ( numCardsDrawn >= cardsForPhase )
		{
			Debug.Log("Both players have played cards");
			endPhase = true;
		}

		return endPhase;
	}

}
