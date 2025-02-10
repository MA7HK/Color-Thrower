using System;
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

	[Header("Manager")]
	[SerializeField] private Transform managerPos;
	[SerializeField] private GameObject managerPrefab;

	public Transform MiningLocation => miningLocation;
	public Transform CollectorLocation => collectorLocation;
	public List<ShaftMiner> Miners => _miners;
	public Collector currentCollector { get; set; }

	public int ShaftId { get; set; }

	private GameObject _minerContainer;
	private List<ShaftMiner> _miners;
	private ShaftManagerLocation _shaftManagerLocation;

	private void Start() {
		_minerContainer = new GameObject("Miners");
		_miners = new List<ShaftMiner>();
		_shaftManagerLocation = GetComponent<ShaftManagerLocation>();
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

	public void CreateManager() {
		GameObject shaftManager = Instantiate(managerPrefab, managerPos.position, quaternion.identity);
		var mineManager = shaftManager.GetComponent<MineManager>();
		mineManager.Setupmanager(_shaftManagerLocation);
		_shaftManagerLocation.MineManager = mineManager;
	}

	private void CreateCollector() {
		currentCollector = Instantiate(CollectorPrefab, collectorInstantiationPosition.position, Quaternion.identity);
		currentCollector.transform.SetParent(collectorInstantiationPosition);
	}

    private void ShaftBoost(Shaft shaft, ShaftManagerLocation shaftManager) {
		if (shaft == this) {
			switch (shaftManager.Manager.boostType) {
				case BoostType.Movement:
					foreach (var miner in _miners) {
						ManagersController.Instance.RunMovementBoost(miner,
						shaftManager.Manager.boostDuration, shaftManager.Manager.boostValue);
					}
					break;
				case BoostType.Loading:
					foreach (var miner in _miners) {
						ManagersController.Instance.RunLoadingBoost(miner,
						shaftManager.Manager.boostDuration, shaftManager.Manager.boostValue);
					}
					break;
			}
		}
    }

	private void OnEnable() {
		ShaftManagerLocation.OnBoost += ShaftBoost;
	}

    private void OnDisable() {
		ShaftManagerLocation.OnBoost += ShaftBoost;
	}
}