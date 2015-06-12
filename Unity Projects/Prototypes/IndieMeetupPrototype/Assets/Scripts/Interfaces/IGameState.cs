using UnityEngine;
using System.Collections;

public interface IGameState 
{
	void OnStartState();
	void OnEndState();
	void OnHideState();
	void OnShowState();
}
