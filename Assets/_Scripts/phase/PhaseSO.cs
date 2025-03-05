using UnityEngine;
using CustomInspector;

// MonoBehaviour -> Runtime(실행중) 작동 클래스
// ScriptableObject -> 에디터 어디든 존재 할 수 있는 클래스(DATA)

[CreateAssetMenu(menuName = "Data/Phase")]
public class PhaseSO : ScriptableObject
{

    public string DisplayName;
    [Preview(Size.small)] public Sprite icon;
    public uint mileage;

    public float scrollSpeed;

    //장애물 (Obstacle) 설정
    [Foldout] public ObstacleSO obstacleData;

    //아이템 설정- 코인 등...
    [Foldout] public CollectableSO collectableData;
}
