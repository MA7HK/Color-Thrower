using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
	[SerializeField] private GameObject upgradePanel;
	[SerializeField] private TextMeshProUGUI panelTitle;
	[SerializeField] private GameObject[] stats;
	[SerializeField] private Image panelIcon;

	[Header("Button Colors")]
	[SerializeField] private Sprite buttonDisableColor;
	[SerializeField] private Sprite buttonEnableColor;
	[SerializeField] private Color normalShade;
	[SerializeField] private Color buttonShade;

	[Header("Buttons")]
	[SerializeField] private GameObject[] upgradeButtons;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI upgradeCost;
	[SerializeField] private TextMeshProUGUI currentStat1;
	[SerializeField] private TextMeshProUGUI currentStat2;
	[SerializeField] private TextMeshProUGUI currentStat3;
	[SerializeField] private TextMeshProUGUI currentStat4;
	[SerializeField] private TextMeshProUGUI stat1Title;
	[SerializeField] private TextMeshProUGUI stat2Title;
	[SerializeField] private TextMeshProUGUI stat3Title;
	[SerializeField] private TextMeshProUGUI stat4Title;

	[Header("Upgraded")]
	[SerializeField] private TextMeshProUGUI upgradeStat1;
	[SerializeField] private TextMeshProUGUI upgradeStat2;
	[SerializeField] private TextMeshProUGUI upgradeStat3;
	[SerializeField] private TextMeshProUGUI upgradeStat4;

	[Header("Images")]
	[SerializeField] private Image stat1Icon;
	[SerializeField] private Image stat2Icon;
	[SerializeField] private Image stat3Icon;
	[SerializeField] private Image stat4Icon;


	[Header("Shaft Icon")]
	[SerializeField] private Sprite shaftMinerIcon;
	[SerializeField] private Sprite  minerIcon;
	[SerializeField] private Sprite walkingIcon;
	[SerializeField] private Sprite miningIcon;
	[SerializeField] private Sprite workingCapacityIcon;

	[Header("Elevator Icon")]
	[SerializeField] private Sprite elevatorMinerIcon;
	[SerializeField] private Sprite  loadIcon;
	[SerializeField] private Sprite movementIcon;
	[SerializeField] private Sprite loadingIcon;

	[Header("Warehouse Icon")]
	[SerializeField] private Sprite warehouseMinerIcon;
	[SerializeField] private Sprite transportationIcon;
	[SerializeField] private Sprite warehouseLoadingIcon;
	[SerializeField] private Sprite warehouseWalkingIcon;

	public int TimesToUpgrade { get; set; }

	private Shaft _selectedShaft;
	private Warehouse _currentWarehouse;
	private ShaftUpgrade _selectedShaftUpgrade;
	private BaseUpgrade _currentUpgrade;

	private void Start(){
		ActivateButton(0);
		TimesToUpgrade = 1;
	}

	public void Upgrade() {
		if (GoldManager.Instance.CurrentGold >= _currentUpgrade.UpgradeCost) {
			_currentUpgrade.Upgrade(TimesToUpgrade);

			if (_currentUpgrade is ShaftUpgrade) {
				UpdateUpgradePanel(_currentUpgrade);
			}

			if (_currentUpgrade is ElevatorUpgrade) {
				UpdateElevatorPanel(_currentUpgrade);
			}

			if (_currentUpgrade is WarehouseUpgrade) {
				UpdateWarehousePanel(_currentUpgrade);
			}
		}
	}

	public void OpenUpgradePanel(bool status) {
		upgradePanel.SetActive(status);
	}

	public void Upgrade_x1() {
		ActivateButton(0);
		TimesToUpgrade = 1;
		upgradeCost.text = $"{_currentUpgrade.UpgradeCost}";
	}
	public void Upgrade_x10() {
		ActivateButton(1);
		TimesToUpgrade = CanUpgradeManyTimes(10, _currentUpgrade) ? 10 : 0;
		upgradeCost.text = GetUpgradeCost(10, _currentUpgrade).ToString();
	}
	public void Upgrade_x50() {
		ActivateButton(2);
		TimesToUpgrade = CanUpgradeManyTimes(50, _currentUpgrade) ? 50 : 0;
		upgradeCost.text = GetUpgradeCost(50, _currentUpgrade).ToString();
	}
	public void Upgrade_Max() {
		ActivateButton(3);
		TimesToUpgrade = CalculateUpgradeCount(_currentUpgrade);
		upgradeCost.text = GetUpgradeCost(TimesToUpgrade, _currentUpgrade).ToString();
	}

	public void ActivateButton(int buttonIndex) {
		for (int i = 0; i < upgradeButtons.Length; i++)	{
			upgradeButtons[i].GetComponent<Image>().sprite = buttonDisableColor;
			upgradeButtons[i].GetComponent<Image>().color = buttonShade;
		}
		upgradeButtons[buttonIndex].GetComponent<Image>().sprite = buttonEnableColor;
		upgradeButtons[buttonIndex].GetComponent<Image>().color = normalShade;
		// upgradeButtons[buttonIndex].transform.DOPunchPosition(
		// 	transform.position + new Vector3(0.0f, -4f, 0.0f),0.4f
		// ).Play();

	}

	public int GetUpgradeCost(int amount, BaseUpgrade upgrade) {
		int cost = 0;
		int upgradeCost = (int) upgrade.UpgradeCost;
		for (int i = 0; i < amount; i++) {
			cost += upgradeCost;
			upgradeCost *= (int) upgrade.UpgradeCostMultiplier;
		}
		return cost;
	}

	public bool CanUpgradeManyTimes(int upgradeAmount, BaseUpgrade upgrade) {
		int count = CalculateUpgradeCount(upgrade);
		if (count > upgradeAmount) {
			return true;
		}
		return false;
	}


	public int CalculateUpgradeCount(BaseUpgrade upgrade) {
		int count = 0;
		int currentGold = GoldManager.Instance.CurrentGold;
		int upgradeCost = (int) upgrade.UpgradeCost;
		for (int i = currentGold; i >= 0; i -= upgradeCost) {
			count++;
			upgradeCost *= (int)upgrade.UpgradeCostMultiplier;
		}
		return count;
	}

	private void UpdateUpgradePanel(BaseUpgrade upgrade) {
		panelTitle.text = $"Mine Shaft {_selectedShaft.ShaftId + 1} Level {upgrade.CurrentLevel}"; 
		upgradeCost.text = $"{upgrade.UpgradeCost}";
		currentStat1.text = $"{_selectedShaft.Miners.Count}";
		currentStat2.text = $"{_selectedShaft.Miners[0].MoveSpeed}";
		currentStat3.text = $"{_selectedShaft.Miners[0].CollectPerSecond}";
		currentStat4.text = $"{_selectedShaft.Miners[0].CollectCapacity}";

		//	update stats Icon
		stats[3].SetActive(true);
		panelIcon.sprite = shaftMinerIcon;

		stat1Icon.sprite = minerIcon;
		stat2Icon.sprite = walkingIcon;
		stat3Icon.sprite = miningIcon;
		stat4Icon.sprite = workingCapacityIcon;

		//	Update Stats Titles
		stat1Title.text = "Miners";
		stat2Title.text = "Walking Speed";
		stat3Title.text = "Mining Speed";
		stat4Title.text = "Working Capacity";

		//	upgrade miner count
		if ((upgrade.CurrentLevel + 1) % 10 == 0) {
			upgradeStat1.text = $"+1";
		}
		else {
			upgradeStat1.text = $"+0";
		}

		//	upgrade walking speed
		float walkSpeed = _selectedShaft.Miners[0].MoveSpeed;
		float walkSpeedMult = upgrade.MoveSpeedMultiplier;
		int walkSpeedAdded = (int)Mathf.Abs(walkSpeed - (walkSpeed  * walkSpeedMult));
		if ((upgrade.CurrentLevel + 1) % 10 == 0) {
			upgradeStat2.text = $"+{walkSpeedAdded}/s";
		}
		else {
			upgradeStat2.text = $"+0/s";
		}

		//	upgrade load speed
		float currentLoadSpeed = _selectedShaft.Miners[0].CollectPerSecond;
		float currentLoadSpeedMult = upgrade.CollectPerSecondMultiplier;
		int LoadSpeedAdded = (int) Mathf.Abs(currentLoadSpeed - (currentLoadSpeed  * currentLoadSpeedMult));
		upgradeStat3.text = $"+{LoadSpeedAdded}";

		//	upgrade worker capacity
		int collectCapacity = _selectedShaft.Miners[0].CollectCapacity;
		float collectCapacityMult = upgrade.CollectCapacityMultiplier;
		int collectCapacityAdded = Mathf.Abs(collectCapacity - (collectCapacity  * (int)collectCapacityMult));
		upgradeStat4.text = $"+{collectCapacityAdded}";

	}

	private void UpdateElevatorPanel(BaseUpgrade upgrade) {
		ElevatorMiner miner = upgrade.Elevator.Miner;
		panelTitle.text = $"Elevator Level {upgrade.CurrentLevel}";

		stats[3].SetActive(false);
		panelIcon.sprite = elevatorMinerIcon;
		
		stat1Icon.sprite = loadIcon;
		stat2Icon.sprite = movementIcon;
		stat3Icon.sprite = loadingIcon;

		//	Update Stats Titles
		stat1Title.text = "Load";
		stat2Title.text = "Movement Speed";
		stat3Title.text = "Loading Speed";

		//	update current stats
		currentStat1.text = $"{miner.CollectCapacity}";
		currentStat2.text = $"{miner.MoveSpeed}";
		currentStat3.text = $"{miner.CollectPerSecond}";

		//	upgrade load values upgraded
		int currentCollect = miner.CollectCapacity;
		float collectMult = upgrade.CollectCapacityMultiplier;
		int load = Mathf.Abs(currentCollect - (currentCollect * (int) collectMult));
		upgradeStat1.text = $"{load}";

		//	update move speed upgrade
		float currentMoveSpeed = miner.MoveSpeed;
		float MoveSpeedMult = upgrade.MoveSpeedMultiplier;
		float moveSpededAdded = Mathf.Abs(currentMoveSpeed - (currentMoveSpeed * MoveSpeedMult));
		if ((upgrade.CurrentLevel + 1) % 10 == 0){
			upgradeStat2.text = $"+{moveSpededAdded}/s";
		}
		else {
			upgradeStat2.text = $"+0/s";
		} 


		//	update move loading speed 
		float loadingSpeed = miner.CollectPerSecond;
		float loadingSpeedMult = upgrade.CollectPerSecondMultiplier;
		float loadingAdded = Mathf.Abs(loadingSpeed - (loadingSpeed * loadingSpeedMult));
		upgradeStat3.text = $"+{loadingAdded}";
	}

	private void UpdateWarehousePanel(BaseUpgrade upgrade) {
		panelTitle.text = $"Warehouse Level {upgrade.CurrentLevel}";
		upgradeCost.text = $"{upgrade.UpgradeCost}";

		//	update Icon
		stat1Icon.sprite = warehouseMinerIcon;
		stat2Icon.sprite = transportationIcon;
		stat3Icon.sprite = warehouseLoadingIcon;
		stat4Icon.sprite = warehouseWalkingIcon;

		// update stats Title
		stat1Title.text = "Transporters";
		stat2Title.text = "Transportation";
		stat3Title.text = "Loading Speed";
		stat4Title.text = "Walking Speed";

		//	update current value
		currentStat1.text = $"{_currentWarehouse.Miners.Count}";
		currentStat2.text = $"{_currentWarehouse.Miners[0].CollectCapacity}";
		currentStat3.text = $"{_currentWarehouse.Miners[0].CollectPerSecond}";
		currentStat4.text = $"{_currentWarehouse.Miners[0].MoveSpeed}";

		//	update miner count upgrade
		if ((upgrade.CurrentLevel + 1) % 10 == 0) {
			upgradeStat1.text = $"+1";
		}
		else {
			upgradeStat1.text = $"+0";
		}

		//	update collect capcity
		int collectCapacity = _currentWarehouse.Miners[0].CollectCapacity;
		float collectCapacitymult = upgrade.CollectCapacityMultiplier;
		int collectCapacityAdded = Mathf.Abs(collectCapacity - (collectCapacity * (int)collectCapacitymult));
		upgradeStat2.text = $"+{collectCapacityAdded}";

		// update load speed
		float currentLoadSpeed = _currentWarehouse.Miners[0].CollectPerSecond;
		float currentLoadSpeedMult = upgrade.CollectPerSecondMultiplier;
		int LoadSpeedAdded = (int) Mathf.Abs(currentLoadSpeed - (currentLoadSpeed * currentLoadSpeedMult));
		upgradeStat3.text = $"{LoadSpeedAdded}";

		//	update move speed upgrade
		float currentMoveSpeed = _currentWarehouse.Miners[0].MoveSpeed;
		float MoveSpeedMult = upgrade.MoveSpeedMultiplier;
		float moveSpededAdded = Mathf.Abs(currentMoveSpeed - (currentMoveSpeed * MoveSpeedMult));
		if ((upgrade.CurrentLevel + 1) % 10 == 0){
			upgradeStat4.text = $"+{moveSpededAdded}/s";
		}
		else {
			upgradeStat4.text = $"+0/s";
		} 

	}

    private void ShaftUpgradeRequest(Shaft shaft, ShaftUpgrade shaftUpgrade) {
		List<Shaft> shaftList = ShaftManager.Instance.Shafts;
		for (int i = 0; i < shaftList.Count; i++) {
			if (shaft.ShaftId == shaftList[i].ShaftId) {
				_selectedShaft = shaftList[i];
				_selectedShaftUpgrade = shaftList[i].GetComponent<ShaftUpgrade>();
			}
		}
		_currentUpgrade = shaftUpgrade;
		OpenUpgradePanel(true);
		UpdateUpgradePanel(_selectedShaftUpgrade);
    }

    private void ElevatorUpgradeRequest(ElevatorUpgrade elevatorUpgrade) {
		_currentUpgrade = elevatorUpgrade;
		OpenUpgradePanel(true);
		UpdateElevatorPanel(elevatorUpgrade);
    }

    private void WarehouseUpgradeRequest(Warehouse warehouse, WarehouseUpgrade upgrade) {
		_currentUpgrade = upgrade;
		_currentWarehouse = warehouse;
		OpenUpgradePanel(true);
		UpdateWarehousePanel(upgrade);

    }

	private void OnEnable() {
		ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
		ElevatorUI.OnUpgradeRequest += ElevatorUpgradeRequest;
		WarehouseUI.OnUpgradeRequest += WarehouseUpgradeRequest;
	}

    private void OnDisable() {
		ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
		ElevatorUI.OnUpgradeRequest -= ElevatorUpgradeRequest;
		WarehouseUI.OnUpgradeRequest -= WarehouseUpgradeRequest;
	}
}