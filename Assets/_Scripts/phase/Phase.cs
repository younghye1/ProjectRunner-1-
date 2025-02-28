
using CustomInspector;
using UnityEngine;

[System.Serializable]
public struct Phase
{
    public string Name;
    [Preview(Size.small)] public Sprite Icon;
    public uint Mileage;

public float scrollSpeed;
    [AsRange(0, 100)] public Vector2 obstacleInterval;
}