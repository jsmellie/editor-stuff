using UnityEngine;

public abstract class BaseState 
{
	#region FIELDS & PROPERTIES

	public abstract bool isPassive { get; }

	#endregion

	#region METHODS

	#region ABSTRACT METHODS

	protected virtual void Pushed(){}

	protected virtual void Popped(){}

	protected virtual void Update(){}

	#endregion

	#endregion
}
