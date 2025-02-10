using UnityEngine;

public enum ManagerLevel {
	Junior,
	Senior,
	Executive,
}

public enum BoostType {
	Movement,
	Loading
}

[CreateAssetMenu]
public class Manager : ScriptableObject
{
	[Header("Manger info")]
	public ManagerLevel managerLevel;
	public Color levelColor;
	public Sprite managerIcon;
	public string managerName;

	[Header("Boost Info")]
	public BoostType boostType;
	public Sprite boostIcon;
	public float boostDuration;
	public float boostValue;
	public string boostDescription;
}