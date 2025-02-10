using System;
using UnityEngine;

public class ElevatorManagerLocation : BaseManagerLocation
{
	public static Action<ElevatorManagerLocation> OnBoost;

    public override void RunBoost() {
		OnBoost?.Invoke(this);
    }

}