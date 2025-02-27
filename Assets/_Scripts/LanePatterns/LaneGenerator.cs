using System.Collections.Generic;
using UnityEngine;


public class LaneGenerator
{
    private List<Lane> lanePatterns = new List<Lane>();

    private RandomGenerator randomGenerator = new RandomGenerator();


    // 할당량 채우면 교체해라
    private Vector2 limitQuota;
    private int _currentQuota;

    private int laneCount;

    [HideInInspector] public Lane currentPattern;

    // 생성자 ( Construct ) : 클래스 최초 호출
    public LaneGenerator(int lanecount, Vector2 quota, List<LanepatternPool> pools)
    {
        laneCount = lanecount;
        limitQuota = quota;

        lanePatterns.Add( new LaneStraight() );
        lanePatterns.Add( new LaneWave() );
        lanePatterns.Add( new LaneZigzag() );

        foreach( var p in pools )
            randomGenerator.AddItem(p);

        SwitchPattern();
    }

    public LaneData GetNextLane()
    {
        _currentQuota++;

        if (_currentQuota >= Random.Range((int)limitQuota.x, (int)limitQuota.y))
            SwitchPattern();

        if (currentPattern == null)
            return new LaneData(-1);

        return currentPattern.GetNextLane();
    }
    
    public void SwitchPattern()
    {
        string patternName = randomGenerator.GetRandom().GetItem() as string;

        Lane lanepattern = lanePatterns.Find( f => f.Name == patternName);                
        currentPattern = lanepattern;
        currentPattern?.Initialize(laneCount);

        _currentQuota = 0;
    }
}
