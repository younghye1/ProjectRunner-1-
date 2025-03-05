

public class LaneStraight : Lane
{
    public LaneType laneType => LaneType.STRAIGHT;
    
    private LaneData data;
    
    public void Initialize(int maxlane)
    {
        data.maxLane = maxlane;
        
        System.Random random = new System.Random();
        data.currentLane = random.Next(0, maxlane);
    }

    public LaneData GetNextLane()
    {
        return data;
    }    
}
