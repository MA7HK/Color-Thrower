using UnityEngine;
using UnityEngine.UI;


public class MineManager : MonoBehaviour
{
	[SerializeField] private Image boostImage;
	[SerializeField] private BaseManagerLocation location;
	public BaseManagerLocation Location => location;

	public ShaftManagerLocation ShaftManagerLocation { get; set; }
	public Image BoostImage { get; set; }

	public void Setupmanager(BaseManagerLocation managerLocation) {
		BoostImage = boostImage;
		location = managerLocation;
		ShaftManagerLocation = managerLocation as ShaftManagerLocation;
	}

	public void RunBoost() {
		ShaftManagerLocation.RunBoost();
	}
}