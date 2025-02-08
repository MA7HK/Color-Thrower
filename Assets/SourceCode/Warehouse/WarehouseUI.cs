using System;
using TMPro;
using UnityEngine;

public class WarehouseUI : MonoBehaviour
{
	public static Action<Warehouse, WarehouseUpgrade> OnUpgradeRequest;
	[SerializeField] private TextMeshProUGUI globalGoldTMP;
	private Warehouse _warehouse;
	private WarehouseUpgrade _warehouseUpgrade;

	private void Start() {
		_warehouse = GetComponent<Warehouse>();
		_warehouseUpgrade = GetComponent<WarehouseUpgrade>();
	}

	private void Update() {
		globalGoldTMP.text = GoldManager.Instance.CurrentGold.ToString();
	}

	public void UpgradeRequest() {
		OnUpgradeRequest?.Invoke(_warehouse, _warehouseUpgrade);
	}
}