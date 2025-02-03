using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ElevatorMiner : BaseMiner
{
	[SerializeField] private Elevator elevator;

	private int _currentShaftIndex = -1;
	private Collector _currentCollector;

	private void Update() {
		if (Input.GetKeyDown(KeyCode.N)) {
			MoveToNextLocation();
		}
	}

	public void MoveToNextLocation() {
		_currentShaftIndex++; // 0

		var currentShaft = ShaftManager.Instance.Shafts[_currentShaftIndex];
		Vector2 nextPos = currentShaft.CollectorLocation.position;
		Vector2 fixedPos = new Vector2(transform.position.x, nextPos.y);

		_currentCollector = currentShaft.currentCollector;
		MoveMiner(fixedPos);
	}

    protected override void CollectGold() {
        if (!_currentCollector.CanCollectGold() && _currentCollector != null) {
			_currentShaftIndex = -1;
			Vector3 elevatorCollectorPos = new Vector3(transform.position.x, 
				elevator.collectorLocation.position.y);
			MoveMiner(elevatorCollectorPos);
			return;
		}

		int amountToCollect = _currentCollector.CollectorGold(this);
		float collectTime = amountToCollect / CollectPerSecond;
		StartCoroutine(IECollect(amountToCollect, collectTime));
    }

    protected override IEnumerator IECollect(int collectGold, float colllectTime) {
		yield return new WaitForSeconds(colllectTime);
		CurrentGold = collectGold;
		_currentCollector.RemoveGold(collectGold);
		yield return new WaitForSeconds(0.5f);

		_currentShaftIndex = -1;
		ChangeGoal();
		Vector3 elevatorCollectorPos = new Vector3(transform.position.x, 
				elevator.collectorLocation.position.y);
		MoveMiner(elevatorCollectorPos);
    }

    protected override void CollectorGold() {
		if (CurrentGold <= 0) {
			_currentShaftIndex = -1;
			ChangeGoal();
			MoveToNextLocation();
			return;
		}

		float collectorTime = CurrentGold /CollectPerSecond;
		StartCoroutine(IECollector(CurrentGold, collectorTime));
    }

    protected override IEnumerator IECollector(int collectedGold, float colllectorTime) {
		yield return new WaitForSeconds(colllectorTime);

		elevator.ElevatorCollector.CollectorGold(CurrentGold);
		CurrentGold = 0;
		_currentShaftIndex = -1;

		//	update goal and move to next location
		ChangeGoal();
		MoveToNextLocation();
    }

}