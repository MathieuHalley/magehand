using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnPhase : MonoBehaviour
{
	protected Player player1;
	protected Player player2;
	protected Player initPlayer;
	protected Player activePlayer;
	protected TerrainCards terrain;

	protected int p1InitCardCount;
	protected int p2InitCardCount;
	protected int initPlayerNum;
	protected int otherPlayerNum;
	protected bool endPhase;
	protected bool initPlayerTurn;


	public virtual void OnEnable()
	{
		//	Initialize local references tof the 2 players & terrain
		player1 = GameManager.Instance.player1;
		player2 = GameManager.Instance.player2;
		terrain = GameManager.Instance.terrain;

		//	Record the initial card counts for the 2 players
		p1InitCardCount = player1.hand.cards.Count;
		p2InitCardCount = player2.hand.cards.Count;

		//	Set the initial active player.
		//	On even turns, player 1 is the initial active player
		//	On odd turns, player 2 is the initial active player
		initPlayerNum = ( GameManager.Instance.curTurn % 2 == 0 ) ? 1 : 2;
		otherPlayerNum = ( GameManager.Instance.curTurn % 2 == 0 ) ? 2 : 1;

		initPlayer = ( initPlayerNum == 1 ) ? player1 : player2;
		activePlayer = GameManager.Instance.SetActivePlayer(initPlayerNum);

		initPlayerTurn = true;
		endPhase = false;
		ResetAddRemovePermissions();
	}

	public virtual void OnDisable()
	{
		ResetAddRemovePermissions();
	}

	protected void EndPhase()
	{
		this.enabled = false;
	}

	protected bool DrawCard( 
		CardSuit cs, 
		CardCollection from, 
		CardCollection to, 
		bool faceUp = true )
	{
		bool success = false;
		Card card = new Card(cs);

		Debug.Log("DrawCard(suit: " + cs.ToString() + ", from: " + from.ToString() + ", to: " + to.ToString() + ", faceUp: " + faceUp);

		//	If the terrain contains the requested card suit, add one to the
		//	to collection and remove one from the from collection
		if ( from.cards.Contains(card) )
		{
			card.faceUp = faceUp;
			if ( to.AddCardAllowed )
			{
				to.cards.Add(card);
				success = true;
			}
			else
			{
				success = false;
				Debug.Log("You don't have permission to add to this card collection");
				Debug.Break();
			}

			if ( from.RemoveCardAllowed )
			{
				from.cards.Remove(card);
				success = true;
			}
			else
			{
				success = false;
				Debug.Log("You don't have permission to remove from this card collection");
				Debug.Break();
			}
		}
		else
		{
			success = false;
		}

		if ( success )
		{
			Debug.Log("Draw Card " + card.ToString() + ": Success");
			return true;
		}
		else
		{
			Debug.Log("Draw Card " + card.ToString() + ": Failed");
			return false;
		}
	}

	protected bool DrawCardInput( out CardSuit suit )
	{
		bool drawCard = false;

		switch ( Input.inputString )
		{
			case "1":
			drawCard = true;
			suit = CardSuit.Air;
			break;
			case "2":
			drawCard = true;
			suit = CardSuit.Earth;
			break;
			case "3":
			drawCard = true;
			suit = CardSuit.Fire;
			break;
			case "4":
			drawCard = true;
			suit = CardSuit.Water;
			break;
			default:
			drawCard = false;
			suit = CardSuit.Air;
			break;
		}
		return drawCard;
	}

	protected void ResetAddRemovePermissions()
	{
		player1.ResetAllAddRemovePermissions();
		player2.ResetAllAddRemovePermissions();

		terrain.visibleTerrain.SetAddRemovePermission(false, false);
	}

	protected bool PlayCard( bool playFaceUp = true )
	{
		CardSuit suitToDraw;
		bool cardWasDrawn = false;

		if ( DrawCardInput(out suitToDraw) )
		{
			Debug.Log("Suit to draw: " + suitToDraw.ToString());
			cardWasDrawn =
				DrawCard(suitToDraw,
						 activePlayer.hand,
						 activePlayer.playSpace,
						 playFaceUp);
		}

		return cardWasDrawn;
	}

	protected bool IsSuccessfulAttack()
	{
		bool success = true;

		CardCollection initPSpace;
		CardCollection otherPSpace;

		initPSpace = 
			( initPlayerNum == 1 ) ? player1.playSpace : player2.playSpace;
		otherPSpace = 
			( initPlayerNum == 1 ) ? player2.playSpace : player1.playSpace;


		for( int i = 0; i < initPSpace.cards.Count; ++i )
		{
			if ( !otherPSpace.ContainsOpposedCard(initPSpace.cards[i]) )
			{
				success = false;
				break;
			}
		}

		return success;
	}

	protected virtual void ActivePlayerTurnStart()
	{
		Debug.Log("Turn Start");
		ResetAddRemovePermissions();
	}
}
