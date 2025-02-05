using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
	[SerializeField] private GameObject upgradePanel;
	[SerializeField] private TextMeshProUGUI panelTitle;

	[Header("Button Colors")]
	[SerializeField] private Sprite buttonDisableColor;
	[SerializeField] private Sprite buttonEnableColor;
	[SerializeField] private Color normalShade;
	[SerializeField] private Color buttonShade;

	[Header("Buttons")]
	[SerializeField] private GameObject[] upgradeButtons;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI upgradeCost;
	[SerializeField] private TextMeshProUGUI minerCount;
	[SerializeField] private TextMeshProUGUI walkingSpeed;
	[SerializeField] private TextMeshProUGUI loadSpeed;
	[SerializeField] private TextMeshProUGUI workerCapacity;

	[Header("Upgraded")]
	[SerializeField] private TextMeshProUGUI minerCountUpgraded;
	[SerializeField] private TextMeshProUGUI walkingSpeedUpgraded;
	[SerializeField] private TextMeshProUGUI loadSpeedUpgraded;
	[SerializeField] private TextMeshProUGUI workerCapacityUpgraded;

	public int TimesToUpgrade;

	private Shaft _selectedShaft;
	private ShaftUpgrade _selectedShaftUpgrade;

	public void OpenUpgradePanel(bool status) {
		upgradePanel.SetActive(status);
	}

	public void Upgrade_x1() {
		ActivateButton(0);
		TimesToUpgrade = 1;
	}
	public void Upgrade_x10() {
		ActivateButton(1);
		TimesToUpgrade = CanUpgradeManyTimes(10, _selectedShaftUpgrade) ? 10 : 0;
	}
	public void Upgrade_x50() {
		ActivateButton(2);
		TimesToUpgrade = CanUpgradeManyTimes(50, _selectedShaftUpgrade) ? 50 : 0;
	}
	public void Upgrade_Max() {
		ActivateButton(3);
	}

	public void ActivateButton(int buttonIndex) {
		for (int i = 0; i < upgradeButtons.Length; i++)	{
			upgradeButtons[i].GetComponent<Image>().sprite = buttonDisableColor;
			upgradeButtons[i].GetComponent<Image>().color = buttonShade;
		}
		upgradeButtons[buttonIndex].GetComponent<Image>().sprite = buttonEnableColor;
		upgradeButtons[buttonIndex].GetComponent<Image>().color = normalShade;
		upgradeButtons[buttonIndex].transform.DOPunchPosition(
			transform.position + new Vector3(0.0f, -4f, 0.0f),0.4f
		).Play();

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
		int currentGold =GoldManager.Instance.CurrentGold;
		int upgradeCost = (int) upgrade.UpgradeCost;
		for (int i = currentGold; i >= 0; i -= upgradeCost) {
			count++;
			upgradeCost *= (int)upgrade.UpgradeCostMultiplier;
		}
		return count;
	}

	private void UpdateUpgradePanel(ShaftUpgrade upgrade) {
		panelTitle.text = $"Mine Shaft {_selectedShaft.ShaftId + 1} Level {upgrade.CurrentLevel}"; 
		upgradeCost.text = $"{upgrade.UpgradeCost}";
		minerCount.text = $"{_selectedShaft.Miners.Count}";
		walkingSpeed.text = $"{_selectedShaft.Miners[0].MoveSpeed}";
		loadSpeed.text = $"{_selectedShaft.Miners[0].CollectPerSecond}";
		workerCapacity.text = $"{_selectedShaft.Miners[0].CollectCapacity}";

		//	upgrade miner count
		if ((upgrade.CurrentLevel + 1) % 10 == 0) {
			minerCountUpgraded.text = $"+1";
		}
		else {
			minerCountUpgraded.text = $"+0";
		}

		//	upgrade walking speed
		float walkSpeed = _selectedShaft.Miners[0].MoveSpeed;
		float walkSpeedMult = upgrade.MoveSpeedMultiplier;
		int walkSpeedAdded = (int)Mathf.Abs(walkSpeed - (walkSpeed  * walkSpeedMult));
		if ((upgrade.CurrentLevel + 1 % 10 == 0)) {
			walkingSpeedUpgraded.text = $"+{walkSpeedAdded}/s";
		}
		else {
			walkingSpeedUpgraded.text = $"+0/s";
		}

		//	upgrade load speed
		float currentLoadSpeed = _selectedShaft.Miners[0].CollectPerSecond;
		float currentLoadSpeedMult = upgrade.CollectPerSecondMultiplier;
		int LoadSpeedAdded = (int)Mathf.Abs(currentLoadSpeed - (currentLoadSpeed  * currentLoadSpeedMult));
		loadSpeedUpgraded.text = $"+{LoadSpeedAdded}";

		//	upgrade worker capacity
		int collectCapacity = _selectedShaft.Miners[0].CollectCapacity;
		float collectCapacityMult = upgrade.CollectCapacityMultiplier;
		int collectCapacityAdded = Mathf.Abs(collectCapacity - (collectCapacity  * (int)collectCapacityMult));
		workerCapacityUpgraded.text = $"+{collectCapacityAdded}";

	}

    private void ShaftUpgradeRequest(Shaft shaft, ShaftUpgrade upgrade) {
		List<Shaft> shaftList = ShaftManager.Instance.Shafts;
		for (int i = 0; i < shaftList.Count; i++) {
			if (shaft.ShaftId == shaftList[i].ShaftId) {
				_selectedShaft = shaftList[i];
				_selectedShaftUpgrade = shaftList[i].GetComponent<ShaftUpgrade>();
			}
		}
		OpenUpgradePanel(true);
		UpdateUpgradePanel(_selectedShaftUpgrade);
    }

	private void OnEnable() {
		ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
	}

    private void OnDisable() {
		ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
	}
}