using UnityEngine;

public class ElevatorUpgrade : BaseUpgrade
{
    protected override void RunUpgrade() {
        _elevator.Miner.CollectCapacity += (int) collectCapacityMultiplier;
		_elevator.Miner.CollectPerSecond += collectPerSecondMultiplier;

		if (CurrentLevel % 10 == 0) {
			_elevator.Miner.MoveSpeed *= moveSpeedMultiplier;
		}
    }

}