using UnityEngine;

public class Collector : MonoBehaviour
{
	public int currentGold { get; set; }


	public void CollectorGold(int amount) {
		currentGold += amount;
	}

	public void RemoveGold(int amount) {
		currentGold -= amount;
	}

	public int CollectorGold(BaseMiner miner) {
		int minerCapacity = miner.CollectCapacity - miner.CurrentGold;
		return EvaluateAmountToCollect(minerCapacity);
	}

	private int EvaluateAmountToCollect(int minerCollectCapcity) {
		if ( minerCollectCapcity <= currentGold) {
			return minerCollectCapcity;
		}
		else {
			return currentGold;
		}
	}

	public bool CanCollectGold() {
		return currentGold > 0;
	}
}