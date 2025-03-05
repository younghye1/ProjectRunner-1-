
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using DG.Tweening;


// Serialize : 인스펙터 노출을 위한 내부 작업들은 한다
[System.Serializable]
public class CollectablePool : RandomItem
{
    public Collectable collectable;

    public override object GetItem()
    {
        return collectable;
    }
}


[System.Serializable]
public class LanepatternPool : RandomItem
{
    public LaneType patternType;
    public override object GetItem()
    {
        return patternType;
    }
}


public class CollectableManager : MonoBehaviour
{

    [Space(20)]
    [SerializeField] float spawnZpos = 60f;
    
    [SerializeField, ReadOnly,AsRange(0, 100)] Vector2 spawnInterval; // 개별 스폰 간격


    private TrackManager trackMgr;
    private CollectableSO data;
    private RandomGenerator collectableGenerator = new RandomGenerator();
    private LaneGenerator laneGenerator;
    

    

    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙관리자 없음");
            yield break;
        }

        yield return new WaitUntil( ()=> GameManager.IsPlaying == true );

        StartCoroutine(InfiniteSpawn());        
    }


    // 아이템 생성 ( lane = 0,1,2 )
    public void SpawnCollectable()
    {
        if (data == null)
            return;

        (LaneData lanedata, Collectable prefab) = RandomLanePrefab();
        
        // Z 위치
        // 현재 해당 트랙의 자식으로 넣는다        
        Track t = trackMgr.GetTrackByZ(spawnZpos);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }
        
        if (prefab != null && lanedata.currentLane != -1)
        {
            Collectable o = Instantiate(prefab, t.CollectableRoot);
            o.SetLanePostion(lanedata.currentLane, lanedata.currentY, spawnZpos, trackMgr);
        }
    }


    IEnumerator InfiniteSpawn()
    {
        double lastMileage = 0f;
        while( true )
        {
            yield return new WaitUntil( ()=> GameManager.IsPlaying && data != null);

            if (GameManager.mileage - lastMileage > Random.Range(spawnInterval.x,spawnInterval.y))
            {
                SpawnCollectable();        

                lastMileage = GameManager.mileage;
            }
        }
    }


    (LaneData, Collectable) RandomLanePrefab()
    {
        LaneData lane = laneGenerator.GetNextLane();
        Collectable prefab = collectableGenerator.GetRandom().GetItem() as Collectable;

        if (prefab == null)
            return (lane, null);

        return (lane, prefab);
    }



    public void SetPhase(PhaseSO phase, float duration = 1f)
    {
        if (phase.collectableData == null)
        {
            ClearCollectables();
            return;
        }

        data = phase.collectableData;

        collectableGenerator.Clear();

        // 아이템들 프리팹 과 랜덤비중 등록
        foreach( var pool in data.collectablepools )
            collectableGenerator.AddItem(pool);

        // 레인의 패턴과 랜덤비중 등록
        laneGenerator = new LaneGenerator(trackMgr.laneList.Count, data.quota, data.lanepatternPools);

        // 장애물 interval 적용
        DOVirtual.Vector2(spawnInterval, data.interval, duration, i => spawnInterval = i ).SetEase(Ease.InOutSine);
    }

    public void ClearCollectables()
    {        
        data = null;
        collectableGenerator.Clear();
    }
}
