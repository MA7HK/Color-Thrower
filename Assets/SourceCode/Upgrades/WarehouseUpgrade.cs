using UnityEngine;

public class WarehouseUpgrade : BaseUpgrade
{
    protected override void RunUpgrade() {
		if (CurrentLevel % 10 == 0) {
			_warehouse.AddMiner();
		}

		foreach(WarehouseMiner miner in _warehouse.Miners) {
			miner.CollectCapacity *= (int) collectCapacityMultiplier;
			miner.CollectPerSecond *= collectPerSecondMultiplier;

			if (CurrentLevel % 10 == 0) {
				miner.MoveSpeed *= moveSpeedMultiplier;
			}
		}
    }

}