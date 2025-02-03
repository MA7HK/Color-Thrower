using TMPro;
using UnityEngine;

public class ElevatorUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI elevatorCollectorGold;
	private Elevator _elevator;

	private void Start() {
		_elevator = GetComponent<Elevator>();
	}

	private void Update() {
		elevatorCollectorGold.text = _elevator.ElevatorCollector.currentGold.ToString();
	}
}