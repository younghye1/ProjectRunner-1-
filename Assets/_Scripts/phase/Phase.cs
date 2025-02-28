
using CustomInspector;
using UnityEngine;

[System.Serializable]
public struct Phase
{
    public string Name;
    public uint Mileage;

public float scrollSpeed;
    [AsRange(0, 100)] public Vector2 obstacleInterval;
}