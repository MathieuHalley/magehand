using UnityEngine;
using System.Collections;

public enum Elements
{
	Air = 1,
	Earth = 2,
	Fire = 4,
	Water = 8
}

public interface ICardData
{
	bool IsFaceUp { get; set; }
	Elements Element { get; }
}
