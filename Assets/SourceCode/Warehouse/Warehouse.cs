using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
	[Header("Prefab")]
	[SerializeField] private GameObject warehouseMinerPrefabs;

	[Header("Objects")]
	[SerializeField] private Collector elevatorCollector;
	[SerializeField] private Transform elevatorLocation;
	[SerializeField] private Transform warehouseCollectorLocation;
	[SerializeField] private List<WarehouseMiner> miners;

	public List<WarehouseMiner> Miners => miners;

	private void Start() {
		miners = new List<WarehouseMiner>();
		AddMiner();
	}

	public void AddMiner() {
		GameObject newMiner = Instantiate(warehouseMinerPrefabs, warehouseCollectorLocation.position, Quaternion.identity);
		var miner = newMiner.GetComponent<WarehouseMiner>();
		miner.ElevatorCollector = elevatorCollector;
		miner.ElevatorCollectorLocation = elevatorLocation;
		miner.WarehouseLocation = warehouseCollectorLocation;

		miners.Add(miner);
	}

    private void WarehouseMinerBoost(WarehouseManagerLocation warehouseManager) {
		switch (warehouseManager.Manager.boostType) {
			case BoostType.Movement:
				foreach (var miner in Miners) {
					ManagersController.Instance.RunMovementBoost(miner,
					warehouseManager.Manager.boostDuration, 
					warehouseManager.Manager.boostValue);
				}
				break;
			case BoostType.Loading:
			foreach (var miner in Miners) {
					ManagersController.Instance.RunLoadingBoost(miner,
					warehouseManager.Manager.boostDuration, 
					warehouseManager.Manager.boostValue);
				}
				break;
		}
    }

	private void OnEnable() {
		WarehouseManagerLocation.OnBoost += WarehouseMinerBoost;
	}

    private void OnDisable() {
		WarehouseManagerLocation.OnBoost -= WarehouseMinerBoost;
	}
}