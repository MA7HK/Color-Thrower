using UnityEngine;

public class Elevator : MonoBehaviour
{
	[SerializeField] private Collector elevatorCollector;
	[SerializeField] private Transform collectLocation;

	public Collector ElevatorCollector => elevatorCollector;
	public Transform collectorLocation => collectLocation;
}