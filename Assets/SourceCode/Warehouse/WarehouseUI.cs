using TMPro;
using UnityEngine;

public class WarehouseUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI globalGoldTMP;

	private void Update() {
		globalGoldTMP.text = GoldManager.Instance.CurrentGold.ToString();
	}
}