
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ManagerCard : MonoBehaviour
{
	[SerializeField] private Image managerIcon;
	[SerializeField] private Image boostIcon;
	[SerializeField] private TextMeshProUGUI managerName;
	[SerializeField] private TextMeshProUGUI managerLevel;
	[SerializeField] private TextMeshProUGUI boostEffect;
	[SerializeField] private TextMeshProUGUI boostDescription;

	public Manager Manager { get; set; }
	public BaseManagerLocation ManagerLocation;

	public void SetupManagerCard(Manager manager) {
		Manager = manager;
		managerIcon.sprite = manager.managerIcon;
		boostIcon.sprite = manager.boostIcon;
		managerName.text = manager.managerName;
		managerLevel.text = manager.managerLevel.ToString();
		managerLevel.color = manager.levelColor;
		boostEffect.text = manager.boostDuration.ToString();
		boostDescription.text = manager.boostDescription;
	}

	public void AssignManager() {
		ManagerLocation = ManagersController.Instance.currentManagerLocation;
		ManagersController.Instance.AddAssignManagerCard(this);
		SetManagerInfoToManagerLocation();
	}

	private void SetManagerInfoToManagerLocation() {
		if (ManagerLocation.Manager == null) {
			ManagerLocation.Manager = Manager;
			ManagersController.Instance.UpdateAssignedManagerInfo(ManagerLocation);
			gameObject.SetActive(false);
		}
	}
}