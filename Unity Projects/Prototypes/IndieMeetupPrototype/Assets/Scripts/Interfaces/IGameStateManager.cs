using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGameStateManager 
{	
	Stack<IGameState> CurrentStates {get;}
	void SwitchState(IGameState state);
	IGameState PopState();
	void PushState(IGameState state);
	IGameState PeekState();
}
