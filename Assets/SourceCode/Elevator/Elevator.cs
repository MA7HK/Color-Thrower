using UnityEngine;

public class Elevator : MonoBehaviour
{

	[SerializeField] private ElevatorMiner miner;
	[SerializeField] private Collector elevatorCollector;
	[SerializeField] private Transform collectLocation;

	public Collector ElevatorCollector => elevatorCollector;
	public Transform collectorLocation => collectLocation;
	public ElevatorMiner Miner => miner;
}