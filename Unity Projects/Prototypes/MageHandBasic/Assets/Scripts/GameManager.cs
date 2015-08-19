using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public Player player1;
	public Player player2;
	public Player activePlayer
	{
		get
		{
			return ( player1.active ) ? player1 : player2;
		}
	}

	public int curTurn;
	public bool curTurnSurrendered;
	public bool curTurnCanDoubleDown;
	public bool curTurnIsSuccessfulAttack;
	public TerrainCards terrain;
	public int initPlayerHandSize;
	public int initTerrainCardCount;
	public KeyCode surrenderTheTurnKeyCode;

	public static GameManager Instance
	{
		get;
		private set;
	}

	public void Awake()
	{
		if ( Instance != null && Instance != this )
			Destroy(gameObject);
		else
			Instance = this;
	}

	//	Set the active player, ensuring only 1 player can be active at a time
	public Player SetActivePlayer( int newActivePlayer )
	{
		newActivePlayer = Mathf.Clamp(newActivePlayer, 1, 2);

		player1.active = ( newActivePlayer == 1 ) ? true : false;
		player2.active = !player1.active;

		return ( player1.active ) ? player1 : player2;
	}

	//	Switch the active player, 
	//	ensuring only 1 player can be active at a time
	public Player SwitchActivePlayer()
	{
		player1.active = ( player1.active ) ? false : true;
		player2.active = !player1.active;

		return ( player1.active ) ? player1 : player2;
	}

	//	Shuffle the list of cards
	public static List<T> Shuffle<T>( List<T> cards )
	{
		T tempCard;
		int i, r;

		i = cards.Count;

		while ( i != 0 )
		{
			r = Random.Range(0, i);
			--i;
			tempCard = cards[i];
			cards[i] = cards[r];
			cards[r] = tempCard;
		}
		return cards;
	}
}
