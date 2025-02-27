

// Class > Struct
// Class : 데이타 , 기능 , 구조 등 (종합적인)
// Struct : 데이타 특화 (특화된)
// Heap(작업자) vs Stack(컴파일러)

public struct LaneData
{
    // 최대 레인 (수)
    public int maxLane; 

    // 결정한 현재 레인 (인덱스) ; X 위치
    public int currentLane;
    // 결정한 현재 Y 위치
    public float currentY;    
    // Z 위치는 각 매니저들이 결정 ( ObstacleManager, CollectableManager 등)

    public LaneData(int current = -1)
    {
        maxLane = -1;
        currentLane = current;
        currentY = 0f;
    }

}
