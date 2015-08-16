using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainDrawPhase : TurnPhase 
{
	/*
	 *	1.	Terrain Draw
	 *	Each player, in turn: Draw 2 face-up cards from the Terrain.
	 */

	private bool terrainIsEmpty;
	private bool cardWasDrawn;
	private int numCardsDrawn;
	private CardSuit suitToDraw;

	public override void OnEnable()
	{
		base.OnEnable();
		Debug.Log("Terrain Draw Phase");
		
		terrain.visibleTerrain.SetAddRemovePermission(false, true);

		ActivePlayerTurnStart();

		terrainIsEmpty = false;
		initPlayerTurn = true;
	}
	
	public void Update()
	{
		//	Use player input to attempt to draw a specific card suit
		//	from the terrain. 
		//	Increment the number of cards drawn if it was successful

		cardWasDrawn = false;

		if ( terrain.visibleTerrain.cards.Count == 0 )
		{
			terrainIsEmpty = true;
		}

		if ( !terrainIsEmpty && DrawCardInput(out suitToDraw) )
		{
			cardWasDrawn = DrawCard(suitToDraw, terrain.visibleTerrain, activePlayer.hand);
		}

		if ( cardWasDrawn )
		{
			++numCardsDrawn;
			//	If 2 or more cards have been drawn by the active player
			//	switch to the next player
			if ( initPlayerTurn && numCardsDrawn >= 2 )
			{
				Debug.Log("Active player has drawn 2 cards from the terrain");

				initPlayerTurn = false;

				activePlayer = GameManager.Instance.SwitchActivePlayer();

				ActivePlayerTurnStart();
			}
		}

		//	If it's the other player's turn explore the phase end conditions
		if ( !initPlayerTurn || terrainIsEmpty )
		{
			//	A total of 4 cards have been drawn by the players
			if (numCardsDrawn >= 4)
			{
				Debug.Log("Both players have played cards");
				endPhase = true;
			}
			//	There aren't any cards left in the terrain
			else if (terrainIsEmpty)
			{
				Debug.Log("The terrain is empty");
				endPhase = true;
			}

			if (endPhase == true)
			{
				Debug.Log("Terrain Draw Phase Ended");
				EndPhase();
			}
		}
	}

	//	Give the active player the permission to remove cards from their hand 
	//	and add them to their playspace
	protected override void ActivePlayerTurnStart()
	{
		base.ActivePlayerTurnStart();
		activePlayer.hand.SetAddRemovePermission(true, false);
	}
}
