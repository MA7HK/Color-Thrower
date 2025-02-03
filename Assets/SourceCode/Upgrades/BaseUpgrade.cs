using System;
using UnityEngine;

public class BaseUpgrade : MonoBehaviour
{
	public static Action<BaseUpgrade, int> OnUpgrade;

	[Header("Upgrades")]
	[SerializeField] protected float collectCapacityMultiplier = 2.0f;
	[SerializeField] protected float collectPerSecondMultiplier = 2.0f;
	[SerializeField] protected float moveSpeedMultiplier = 1.25f;

	[Header("Cost")]
	[SerializeField] private float InitialUpgradeCost = 200.0f;
	[SerializeField] private float upgradesCostMultiplier = 2.0f;

	public int CurrentLevel { get; set; }
	public float UpgradeCost { get; set; }

	protected Shaft _shaft;

	private void Start() {
		_shaft = GetComponent<Shaft>();

		CurrentLevel = 1;
		UpgradeCost = InitialUpgradeCost;
	}

	public virtual void Upgrade(int upgradeAmount) {
		if (upgradeAmount > 0) {
			for (int i = 0; i < upgradeAmount; i++) {
				UpgradeSuccess();
				UpdateUpgradeValues();
				RunUpgrade();
			}
		}
	}

	protected virtual void UpgradeSuccess() {  
		GoldManager.Instance.RemoveGold((int)UpgradeCost);
		CurrentLevel++;
		OnUpgrade?.Invoke(this, CurrentLevel);
	}

	protected virtual void UpdateUpgradeValues() { 
		/* Update Values */
		UpgradeCost *= upgradesCostMultiplier;
	}
	protected virtual void RunUpgrade() { /* Update Logic */ }

	
}