using UnityEngine;
using System.Collections;

public interface IMergable<T>
{
	/// <summary>
	///		MergeBegin
	///			T1 is this IMergable
	///			T2 is the IMergable to be merged with
	///		MergeComplete
	///			T1 & T2 are the IMergables from MergeBegin
	///			T3 is the new IMergable
	///			bool is whether the merge is valid or not
	///		MergeFailure
	///			T1 & T2 are the IMergables from MergeBegin
	/// </summary>
	event System.Action<T, T> MergeBegin;
	event System.Action<T, T, T, bool> MergeComplete;

	void OnMergeBegin(T m1, T m2);
	void OnMergeComplete(T m1, T m2, T merged, bool valid);

	T Merge(T m);
	bool ValidMerge(T m);
}
