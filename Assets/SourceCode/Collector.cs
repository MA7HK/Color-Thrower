using UnityEngine;

public class Collector : MonoBehaviour
{
	public int currentGold { get; set; }


	public void CollectorGold(int amount) {
		currentGold += amount;
	}

	public void RemoveGold(int amount) {
		currentGold -= amount;
	}
}