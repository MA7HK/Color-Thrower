using System.Collections.Generic;
using UnityEngine;

public class ManagersController : MonoBehaviour
{
	[SerializeField] private ManagerCard managerCardPrefab;
	[SerializeField] private int initialManagerCost = 100;
	[SerializeField] private int managerCostMultiplier = 3;

	[SerializeField] private Transform managersContainer;
	[SerializeField] private List<Manager> managerList;
	public int newManagerCost { get; set; }

	void Start() {
		newManagerCost = initialManagerCost;
	}

	public void HireManager() {
		if (GoldManager.Instance.CurrentGold >= newManagerCost) {
			//	create card
			ManagerCard card = Instantiate(managerCardPrefab, managersContainer);
			//	get random manager
			int randomIndex = Random.Range(0, managerList.Count);
			Manager randomManager = managerList[randomIndex];
			card.SetupManagerCard(randomManager);

			managerList.RemoveAt(randomIndex);

			GoldManager.Instance.RemoveGold(newManagerCost);
			newManagerCost *= managerCostMultiplier;
		}
	}
}