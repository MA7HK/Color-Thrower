using InfinityCode.UltimateEditorEnhancer;
using Unity.Mathematics;
using UnityEngine;

public class Shaft : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField] private ShaftMiner minerPrefab;
	[SerializeField] private Collector CollectorPrefab;

	[Header("Locations")]
	[SerializeField] private Transform miningLocation;
	[SerializeField] private Transform collectorLocation;
	[SerializeField] private Transform collectorInstantiationPosition;

	public Transform MiningLocation => miningLocation;
	public Transform CollectorLocation => collectorLocation;
	public Collector currentCollector { get; set; }

	private void Start() {
		CreateMiner();
		CreateCollector();
	}

	private void CreateMiner() {
		var newMiner =  Instantiate(minerPrefab, collectorLocation.position, quaternion.identity);
		newMiner.currentShaft = this;
		newMiner.MoveMiner(miningLocation.position);
	}

	private void CreateCollector() {
		currentCollector = Instantiate(CollectorPrefab, collectorInstantiationPosition.position, Quaternion.identity);
		currentCollector.transform.SetParent(this.transform);
	}
}