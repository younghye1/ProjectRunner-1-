using UnityEngine;
using CustomInspector;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Data/Obstacle")]

public class ObstacleSO : ScriptableObject
{
    public List<ObstaclePool> pools;
    [AsRange(0,100)] public Vector2 interval;
    

}
