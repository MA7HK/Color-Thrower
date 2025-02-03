using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShaftManager : Singleton<ShaftManager>
{
	[SerializeField] private Shaft shaftPrefab;
	[SerializeField] private float newShaftYPosition;
	[SerializeField] private int newShaftCost = 500;

	[Header("Shafts")]
	[SerializeField] private List<Shaft> shafts;

	public List<Shaft> Shafts => shafts;
	public int NewShaftCost => newShaftCost;

	public void AddShaft() {
		Transform lastShaft = shafts[0].transform;
		var newShaft =  Instantiate(shaftPrefab, lastShaft.position, quaternion.identity);
		newShaft.transform.localPosition = new Vector3(lastShaft.position.x, lastShaft.position.y - newShaftYPosition,
			lastShaft.position.z);
		shafts.Add(newShaft);
	}
}