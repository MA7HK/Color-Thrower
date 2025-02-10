using System;
using UnityEngine;

public class WarehouseManagerLocation : BaseManagerLocation
{
	public static Action<WarehouseManagerLocation> OnBoost;

	public override void RunBoost() {
		OnBoost?.Invoke(this);
	}
}