using UnityEngine;
using CustomInspector;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Data/Collectable")]

public class CollectableSO : ScriptableObject
{
    public List<CollectablePool> collectablepools;
   public List<LanepatternPool> lanepatternPools;

    [AsRange(0,100)] public Vector2 interval;
    [AsRange(1,100)] public Vector2 quota; 
    

}
