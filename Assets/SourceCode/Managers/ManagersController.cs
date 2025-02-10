using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;

public class ManagersController : Singleton<ManagersController>
{
	[SerializeField] private ManagerCard managerCardPrefab;
	[SerializeField] private int initialManagerCost = 100;
	[SerializeField] private int managerCostMultiplier = 3;

	[SerializeField] private Transform managersContainer;
	[SerializeField] private List<Manager> managerList;
	[SerializeField] private GameObject managerPanel;
	[SerializeField] private GameObject assignedManagerPanel;
	[SerializeField] private TextMeshProUGUI managerPanelTitle;

	[Header("Assign Manager UI")]
	[SerializeField] private Image managerIcon;
	[SerializeField] private Image boostIcon;
	[SerializeField] private TextMeshProUGUI managerName;
	[SerializeField] private TextMeshProUGUI managerLevel;
	[SerializeField] private TextMeshProUGUI boostEffect;
	[SerializeField] private TextMeshProUGUI boostDescription;

	public BaseManagerLocation currentManagerLocation { get; set; }
	public int newManagerCost { get; set; }
	private List<ManagerCard> _assignedManagerCards;
	private Camera _camera;

	void Start() {
		_assignedManagerCards = new List<ManagerCard>();
		_camera = Camera.main;
		newManagerCost = initialManagerCost;
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo)) {
				if (hitInfo.transform.GetComponent<MineManager>() != null) {
					currentManagerLocation = hitInfo.transform.GetComponent<MineManager>().Location;
					OpenPanel(true);
				}
			}
		}
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

	public void UnassignManager() {
		RestoreManagerCard(currentManagerLocation.Manager);
		currentManagerLocation.Manager = null;
		UpdateAssignedManagerInfo(currentManagerLocation);
	}

	public void UpdateAssignedManagerInfo(BaseManagerLocation managerLocation) {
		if (managerLocation.Manager != null) {
			managerIcon.sprite = managerLocation.Manager.managerIcon;
			boostIcon.sprite = managerLocation.Manager.boostIcon;
			managerName.text = managerLocation.Manager.managerName;
			managerLevel.text = managerLocation.Manager.managerLevel.ToString();
			boostEffect.text = managerLocation.Manager.boostDuration.ToString();
			boostDescription.text = managerLocation.Manager.boostDescription;
			managerLocation.UpdateBoostIcon();
			assignedManagerPanel.SetActive(true);
		}
		else {
			managerIcon.sprite = null;
			boostIcon.sprite = null;
			managerName.text = null;
			managerLevel.text = null;
			boostEffect.text = null;
			boostDescription.text = null;
			assignedManagerPanel.SetActive(false);
		}
	}

	public void AddAssignManagerCard(ManagerCard card) {
		_assignedManagerCards.Add(card);
	}

	public void OpenPanel(bool value) {
		managerPanel.SetActive(value);
		if (value) {
			managerPanelTitle.text = currentManagerLocation.LocationTitle;
			UpdateAssignedManagerInfo(currentManagerLocation);
		}
	}

	private void RestoreManagerCard(Manager manager) {
		ManagerCard managerCard = null;
		for (int i = 0; i < _assignedManagerCards.Count; i++) {
			if (_assignedManagerCards[i].Manager.managerName == manager.managerName) {
				_assignedManagerCards[i].gameObject.SetActive(true);
				managerCard = _assignedManagerCards[i];
			}
		}

		if (managerCard != null) {
			_assignedManagerCards.Remove(managerCard);
		}
	}

	public void RunMovementBoost(BaseMiner miner, float duration, float value) {
		StartCoroutine(IEMovementBoost(miner, duration, value));
	}

	public void RunLoadingBoost(BaseMiner miner, float duration, float value) {
		StartCoroutine(IELoadingBoost(miner, duration, value));
	}

	private IEnumerator IEMovementBoost(BaseMiner miner, float duration, float value) {
		float startSpeed = miner.MoveSpeed;
		miner.MoveSpeed *= value;
		yield return new WaitForSeconds(duration);
		miner.MoveSpeed = startSpeed;
	}

	private IEnumerator IELoadingBoost(BaseMiner miner, float duration, float value) {
		float startValue = miner.CollectPerSecond;
		miner.CollectPerSecond *= value;
		yield return new WaitForSeconds(duration);
		miner.CollectPerSecond = startValue;
	}
}