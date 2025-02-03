using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaftMiner : BaseMiner
{
	public Shaft currentShaft { get; set; }

	private int miningAnimationPatameter = Animator.StringToHash("Mining");
	private int walkingAnimationParameter = Animator.StringToHash("Walking");
	
    public override void MoveMiner(Vector3 newPosition) {
        base.MoveMiner(newPosition);
		_animator.SetTrigger(walkingAnimationParameter);
    }

    protected override void CollectGold() {
        base.CollectGold();
		float collectTime = CollectCapacity / CollectPerSecond;
		_animator.SetTrigger(miningAnimationPatameter);
		StartCoroutine(IECollect(CollectCapacity, collectTime));
    }

    protected override IEnumerator IECollect(int collectGold, float colllectTime) {
        yield return new WaitForSeconds(colllectTime);

		CurrentGold = collectGold;
		ChangeGoal();
		RotateMiner(-1);
		MoveMiner(currentShaft.CollectorLocation.position);
    }

    protected override void CollectorGold() {
		//	Add Current glod to the Collector class
		currentShaft.currentCollector.CollectorGold(CurrentGold);
		//	update some values
		CurrentGold = 0;
		ChangeGoal();
		RotateMiner(1);
		MoveMiner(currentShaft.MiningLocation.position);
    }

}