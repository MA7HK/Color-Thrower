using System.Threading;
using UnityEngine;

public class Cloud : MonoBehaviour
{
	
	[SerializeField] private float moveSpeed = 5.0f;

	public Vector2 SpawnPosition { get; set; }


	void Update() {
		transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		transform.position = new Vector3(SpawnPosition.x, 
			SpawnPosition.y + Random.Range(-0.5f, 0.5f), 1.0f);
	}
}