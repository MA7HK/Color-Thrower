using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ElevatorMiner : BaseMiner
{
	[SerializeField] private Elevator elevator;
	[SerializeField] private float collectTime;
	[SerializeField] private float collectorTime;

	private int _currentShaftIndex = -1;
	private Collector _currentCollector;

	private void Start() {
		MoveToNextLocation();
	}

	// private void Update() {
	// 	if (Input.GetKeyDown(KeyCode.N)) {
	// 		MoveToNextLocation();
	// 	}
	// }

	public void MoveToNextLocation() {
		_currentShaftIndex++; // 0

		var currentShaft = ShaftManager.Instance.Shafts[_currentShaftIndex];
		Vector2 nextPos = currentShaft.CollectorLocation.position;
		Vector2 fixedPos = new Vector2(transform.position.x, nextPos.y);

		_currentCollector = currentShaft.currentCollector;
		MoveMiner(fixedPos);
	}

    protected override void CollectGold() {
        if (!_currentCollector.CanCollectGold() && _currentCollector != null
				&& _currentShaftIndex == ShaftManager.Instance.Shafts.Count - 1) {
			_currentShaftIndex = -1;
			ChangeGoal();
			Vector3 elevatorCollectorPos = new Vector3(transform.position.x, elevator.collectorLocation.position.y);
			MoveMiner(elevatorCollectorPos);
			return;
		}

		int amountToCollect = _currentCollector.CollectorGold(this);
		collectTime = amountToCollect / CollectPerSecond;
		OnLoading?.Invoke(this, collectTime);
		StartCoroutine(IECollect(amountToCollect, collectTime));
    }

    protected override IEnumerator IECollect(int collectGold, float collectTime) {
		yield return new WaitForSeconds(collectTime);
		if (CurrentGold > 0 && CurrentGold < CollectCapacity) {
			CurrentGold += collectGold;
		}
		else {
			CurrentGold = collectGold;
		}

		_currentCollector.RemoveGold(collectGold);
		yield return new WaitForSeconds(0.5f);

		if (CurrentGold == CollectCapacity || _currentShaftIndex == ShaftManager.Instance.Shafts.Count - 1) {
			_currentShaftIndex = -1;
			ChangeGoal();
			Vector3 elevatorCollectorPos = new Vector3(transform.position.x, elevator.collectorLocation.position.y);
			MoveMiner(elevatorCollectorPos);
		}
		else {
			MoveToNextLocation();
		}
    }

    protected override void CollectorGold() {
		if (CurrentGold <= 0) {
			_currentShaftIndex = -1;
			ChangeGoal();
			MoveToNextLocation();
			return;
		}

		collectorTime = CurrentGold /CollectPerSecond;
		OnLoading?.Invoke(this, collectorTime);
		StartCoroutine(IECollector(CurrentGold, collectorTime));
    }

    protected override IEnumerator IECollector(int collectedGold, float collectorTime) {
		yield return new WaitForSeconds(collectorTime);

		elevator.ElevatorCollector.CollectorGold(CurrentGold);
		CurrentGold = 0;
		_currentShaftIndex = -1;

		//	update goal and move to next location
		ChangeGoal();
		MoveToNextLocation();
	}

    private void ElevatorBoost(ElevatorManagerLocation elevatorManager) {
		switch (elevatorManager.Manager.boostType)
		{
			case BoostType.Movement:
				ManagersController.Instance.RunMovementBoost(this,
				elevatorManager.Manager.boostDuration,
				elevatorManager.Manager.boostValue);
				break;
			case BoostType.Loading:
				ManagersController.Instance.RunLoadingBoost(this,
				elevatorManager.Manager.boostDuration,
				elevatorManager.Manager.boostValue);
				break;
		}
    }

	private void OnEnable() {
		ElevatorManagerLocation.OnBoost += ElevatorBoost;
	}

    private void OnDisable() {
		ElevatorManagerLocation.OnBoost -= ElevatorBoost;
	}

}