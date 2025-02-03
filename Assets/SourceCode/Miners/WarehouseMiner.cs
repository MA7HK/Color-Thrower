using System.Collections;
using UnityEngine;

public class WarehouseMiner : BaseMiner
{
	public Collector ElevatorCollector { get; set; }
	public Transform ElevatorCollectorLocation { get; set; }
	public Transform WarehouseLocation { get; set; }

	private readonly int _walkingNoGold = Animator.StringToHash("WalkingNoGold");
	private readonly int _walkingWithGold = Animator.StringToHash("WalkingWithGold");

	private void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			RotateMiner(-1);
			Vector2 fixedPos = new Vector2(ElevatorCollectorLocation.position.x, transform.position.y);
			MoveMiner(fixedPos);
		}
	}

    public override void MoveMiner(Vector3 newPosition) {
        base.MoveMiner(newPosition);
		_animator.SetBool(_walkingNoGold, true);
    }

    protected override void CollectGold() {
        if (ElevatorCollector.currentGold <= 0) {
			RotateMiner(1);
			ChangeGoal();
			MoveMiner(new Vector3(WarehouseLocation.position.x, transform.position.y));
			return;
		}

		_animator.SetBool(_walkingNoGold, false);
		_animator.SetBool(_walkingWithGold, true);

		int currentGold = ElevatorCollector.CollectorGold(this);
		float collectTime = CollectCapacity / CollectPerSecond;
		StartCoroutine(IECollect(currentGold, collectTime));
    }

    protected override IEnumerator IECollect(int collectGold, float collectTime) {
		yield return new WaitForSeconds(collectTime);

		CurrentGold = collectGold;
		ElevatorCollector.RemoveGold(collectGold);

		RotateMiner(1);
		ChangeGoal();
		MoveMiner(new Vector3(WarehouseLocation.position.x, transform.position.y));
    }



}