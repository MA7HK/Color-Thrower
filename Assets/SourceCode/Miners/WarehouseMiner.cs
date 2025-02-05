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
			_animator.SetBool(_walkingNoGold, true);
			MoveMiner(new Vector2(ElevatorCollectorLocation.position.x, transform.position.y));
		}
	}

    protected override void CollectGold() {
        if (ElevatorCollector.currentGold <= 0) {
			RotateMiner(1);
			ChangeGoal();
			MoveMiner(new Vector3(WarehouseLocation.position.x, transform.position.y));
			return;
		}

		_animator.SetBool(_walkingNoGold, false);

		int currentGold = ElevatorCollector.CollectorGold(this);
		float collectTime = CollectCapacity / CollectPerSecond;
		OnLoading?.Invoke(this, collectTime);
		StartCoroutine(IECollect(currentGold, collectTime));
    }

    protected override IEnumerator IECollect(int collectGold, float collectTime) {
		yield return new WaitForSeconds(collectTime);
		_animator.SetBool(_walkingWithGold, true);

		CurrentGold = collectGold;
		ElevatorCollector.RemoveGold(collectGold);

		RotateMiner(1);
		ChangeGoal();
		MoveMiner(new Vector3(WarehouseLocation.position.x, transform.position.y));
    }

    protected override void CollectorGold() {
        if (CurrentGold <= 0) {
			RotateMiner(-1);
			ChangeGoal();
			MoveMiner(new Vector2(ElevatorCollectorLocation.position.x, transform.position.y));
			return;
		}

		_animator.SetBool(_walkingWithGold, false);
		_animator.SetBool(_walkingNoGold, false);

		float collectorTime = CurrentGold / CollectPerSecond;
		OnLoading?.Invoke(this, collectorTime);
		StartCoroutine(IECollector(CurrentGold, collectorTime));
    }

    protected override IEnumerator IECollector(int collectedGold, float collectorTime) {
		yield return new WaitForSeconds(collectorTime);

		GoldManager.Instance.AddGold(CurrentGold);
		CurrentGold = 0;

		RotateMiner(-1);
		ChangeGoal();
		MoveMiner(new Vector2(ElevatorCollectorLocation.position.x, transform.position.y));
    }

}