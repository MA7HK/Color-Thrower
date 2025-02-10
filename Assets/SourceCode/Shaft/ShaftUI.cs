using System;
using TMPro;
using UnityEngine;

public class ShaftUI : MonoBehaviour
{
	public static Action<Shaft, ShaftUpgrade> OnUpgradeRequest;

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
		if (_shaft.currentCollector.currentGold > 0) {
			currentGoldTMP.text = Currency.DisplayCurrency(_shaft.currentCollector.currentGold);
		} else { currentGoldTMP.text = $"0"; }
	}

	public void BuyNewShaft() {
		if (GoldManager.Instance.CurrentGold > ShaftManager.Instance.NewShaftCost) {
			GoldManager.Instance.RemoveGold(ShaftManager.Instance.NewShaftCost);
			ShaftManager.Instance.AddShaft();
			buyNewShaftButton.SetActive(false);
		}
	}

	public void UpgradeRequest() {
		OnUpgradeRequest.Invoke(_shaft, _shaftUpgrade);
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