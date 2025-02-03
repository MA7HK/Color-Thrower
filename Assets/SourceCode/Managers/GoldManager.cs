using UnityEngine;

public class GoldManager : Singleton<GoldManager>
{
	[SerializeField] private int testgold = 0;
	public int CurrentGold { get; set; }
	private readonly string GOLD_KEY = "goldkey";

	private void Start() {
		LoadGold();
	}

	private void LoadGold() {
		CurrentGold = PlayerPrefs.GetInt(GOLD_KEY, testgold);
	}

	public void AddGold(int amount) {
		CurrentGold += amount;
		PlayerPrefs.SetInt(GOLD_KEY, CurrentGold);
		PlayerPrefs.Save();

	}

	public void RemoveGold(int amount) {
		CurrentGold -= amount;
		PlayerPrefs.SetInt(GOLD_KEY, CurrentGold);
		PlayerPrefs.Save();
	}

}