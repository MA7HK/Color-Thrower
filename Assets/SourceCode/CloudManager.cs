using UnityEngine;

public class CloudManager : MonoBehaviour
{
	[SerializeField] private GameObject cloudPrefeb;
	[SerializeField] private Transform spawnPosition;

	void Start() {
		SpawnCloud();
	}

	private void SpawnCloud() {
		GameObject newCloud =Instantiate(cloudPrefeb, spawnPosition.position, Quaternion.identity);
		var cloud = newCloud.GetComponent<Cloud>();
		cloud.SpawnPosition = spawnPosition.position;

	}
}