using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
	private static  T instance;
	public static T Instance {
		get {
			if (instance == null) {
				instance = FindAnyObjectByType<T>();
				if (instance == null) {
					GameObject newGO = new GameObject();
					instance = newGO.AddComponent<T>();
				}
			}
			return instance;
		}
	}

	protected virtual void Awake() {
		instance = this as T;
	}
}