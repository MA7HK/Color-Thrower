using System;
using TMPro;
using UnityEngine;

public class ShaftUI : MonoBehaviour
{
	[Header("Buttons")]
	[SerializeField] private GameObject buyNewShaftButton;

	[Header("Text")]
	[SerializeField] private TextMeshProUGUI currentGoldTMP;
	[SerializeField] private TextMeshProUGUI currentLevelTMP;

	private Shaft _shaft;
	private ShaftUpgrade _shaftUpgrade;

	private void Start() {
		_shaftUpgrade = GetComponent<ShaftUpgrade>();
		_shaft = GetComponent<Shaft>();
	}

	private void Update() {
		currentGoldTMP.text = _shaft.currentCollector.currentGold.ToString();
	}

	public void BuyNewShaft() {
		if (GoldManager.Instance.CurrentGold > ShaftManager.Instance.NewShaftCost) {
			GoldManager.Instance.RemoveGold(ShaftManager.Instance.NewShaftCost);
			ShaftManager.Instance.AddShaft();
			buyNewShaftButton.SetActive(false);
		}
	}

    private void UpgradeShaft(BaseUpgrade upgrade, int currentLvl) {
		if (upgrade == _shaftUpgrade) {
			currentLevelTMP.text = $"Level\n {currentLvl}";
		}
    }

	private void OnEnable() {
		ShaftUpgrade.OnUpgrade += UpgradeShaft;
	}

    private void OnDisable() {
		ShaftUpgrade.OnUpgrade -= UpgradeShaft;
	}
}