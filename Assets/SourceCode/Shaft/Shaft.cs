using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Shaft : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField] private ShaftMiner minerPrefab;
	[SerializeField] private Collector CollectorPrefab;

	[Header("Locations")]
	[SerializeField] private Transform miningLocation;
	[SerializeField] private Transform collectorLocation;
	[SerializeField] private Transform collectorInstantiationPosition;

	public Transform MiningLocation => miningLocation;
	public Transform CollectorLocation => collectorLocation;
	public List<ShaftMiner> Miners => _miners;
	public Collector currentCollector { get; set; }

	private GameObject _minerContainer;
	private List<ShaftMiner> _miners;

	private void Start() {
		_miners = new List<ShaftMiner>();
		_minerContainer = new GameObject("Miners");
		CreateMiner();
		CreateCollector();
	}

	public void CreateMiner() {
		var newMiner =  Instantiate(minerPrefab, collectorLocation.position, quaternion.identity);
		newMiner.currentShaft = this;
		newMiner.transform.SetParent(_minerContainer.transform);
		newMiner.MoveMiner(miningLocation.position);

		if (_miners.Count > 0) {
			newMiner.CollectCapacity = _miners[0].CollectCapacity;
			newMiner.CollectPerSecond = _miners[0].CollectPerSecond;
			newMiner.MoveSpeed = _miners[0].MoveSpeed;
		}

		//	add new miner
		_miners.Add(newMiner);
	}

	private void CreateCollector() {
		currentCollector = Instantiate(CollectorPrefab, collectorInstantiationPosition.position, Quaternion.identity);
		currentCollector.transform.SetParent(collectorInstantiationPosition);
	}
}