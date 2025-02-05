using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class ShaftManager : Singleton<ShaftManager>
{
	[SerializeField] private Shaft shaftPrefab;
	[SerializeField] private float newShaftYPosition;
	[SerializeField] private int newShaftCost = 500;

	[Header("Shafts")]
	[SerializeField] private List<Shaft> shafts;

	private int _currentShaftIndex;

	public List<Shaft> Shafts => shafts;
	public int NewShaftCost => newShaftCost;

	private void Start() {
		shafts[0].ShaftId = 0;
	}

	public void AddShaft() {

		Transform lastShaft = shafts.Last().transform;
		var newShaft =  Instantiate(shaftPrefab, lastShaft.position, quaternion.identity);
		newShaft.transform.localPosition = new Vector3(lastShaft.position.x, lastShaft.position.y - newShaftYPosition,
			lastShaft.position.z);

		_currentShaftIndex++;
		newShaft.ShaftId = _currentShaftIndex;
		shafts.Add(newShaft);
	}
}