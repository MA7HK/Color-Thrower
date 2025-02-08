
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

	public void SetupManagerCard(Manager manager) {
		managerIcon.sprite = manager.managerIcon;
		boostIcon.sprite = manager.boostIcon;
		managerName.text = manager.managerName;
		managerLevel.text = manager.managerLevel.ToString();
		managerLevel.color = manager.levelColor;
		boostEffect.text = manager.boostDuration.ToString();
		boostDescription.text = manager.boostDescription;
	}
}