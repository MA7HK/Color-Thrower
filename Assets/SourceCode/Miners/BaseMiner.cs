using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BaseMiner : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 5.0f;
	[SerializeField] private float goldCollectPerSecond = 50.0f;
	[SerializeField] private int InitialCollectCapacity = 200;

	public float MoveSpeed { get; set; }
	public int CurrentGold { get; set; }
	public int CollectCapacity{ set; get; }
	public float CollectPerSecond{ set; get; }
	public bool IsTimeToCollect { get; set; }

	protected Animator _animator;

	private void Awake() {
		_animator = GetComponent<Animator>();

		CurrentGold = 0;
		MoveSpeed = moveSpeed;
		IsTimeToCollect = true;
		CollectCapacity = InitialCollectCapacity;
		CollectPerSecond = goldCollectPerSecond;
	}

	public virtual void MoveMiner(Vector3 newPosition) {
		transform.DOMove(newPosition, 10.0f / MoveSpeed).OnComplete(
			() => {
				if(IsTimeToCollect) {
					CollectGold();
				}
				else {
					CollectorGold();
				}
			}
		).Play();
	}

	protected virtual void CollectGold() {}

	protected virtual IEnumerator IECollect(int collectGold, float colllectTime) {
		yield return null;
	} 
	protected virtual IEnumerator IECollector(int collectedGold, float colllectorTime) {
		yield return null;
	} 

	protected virtual void CollectorGold() {}

	public void ChangeGoal() => IsTimeToCollect = !IsTimeToCollect;

	public void RotateMiner(int direction) {
		if (direction == 1) {
			transform.localScale = new Vector3(1,1,1);
		}
		else {
			transform.localScale = new Vector3(-1,1,1);
		}
	}
}