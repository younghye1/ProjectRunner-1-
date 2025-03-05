

// abstract 와 유사하지만 , 가벼운 버젼
// abstract 변수 선언 (가능)
// interface 변수 선언 (불가능)
// interface 는 생성자(Construct) (불가능)


public enum LaneType{ EMPTY, STRAIGHT, WAVE, ZIGZAG}

public interface Lane
{
    public LaneType laneType { get; }

    // 초기화 함수
    public void Initialize(int maxlane);

    // Lane 정보를 가져오는 함수
    public LaneData GetNextLane();
}
