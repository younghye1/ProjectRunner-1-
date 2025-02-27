
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using CustomInspector;



[System.Serializable]
public class ObstaclePool : RandomItem
{
    public List<Obstacle> obstacleList;

    public override object GetItem()
    {
        if (obstacleList == null || obstacleList.Count <= 0)
            return null;
            
        return obstacleList[Random.Range(0, obstacleList.Count)];
    }
}

public class ObstacleManager : MonoBehaviour
{

    [Space(20)]
    public List<ObstaclePool> obstaclePools;


    [Space(20)]
    [SerializeField] float spawnZpos = 60f;
    [SerializeField, AsRange(0, 100)] Vector2 spawnInterval;


    private TrackManager trackMgr;
    private RandomGenerator randomGenerator = new RandomGenerator();


    // Coroutine 방식 : Function, Method, Subroutine
    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙관리자 없음");
            yield break; // return 과 동일 : 함수 완전 탈출
        }

        // Obstacle Pools 에 있는 모든 값을 랜덤생성기에 등록.
        foreach( var pool in obstaclePools )
            randomGenerator.AddItem(pool);


        //yield return new WaitForEndOfFrame();  // 지연 : 1프레임만 지연
        //yield return new WaitForSeconds(2f);   // 지연 : 2초 지연
        yield return new WaitUntil( ()=> GameManager.IsPlaying == true );

        StartCoroutine(InfiniteSpawn());        
    }


    // 장애물 생성 ( lane = 0,1,2 )
    public void SpawnObstacle()
    {
        (int lane, Obstacle prefab) = RandomLanePrefab();
        
        // Z 위치
        // 현재 해당 트랙의 자식으로 넣는다        
        Track t = trackMgr.GetTrackByZ(spawnZpos);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }
        
        if (prefab != null)
        {
            Obstacle o = Instantiate(prefab, t.ObstacleRoot);
            o.SetLanePostion(lane, spawnZpos, trackMgr);
        }
    }


    IEnumerator InfiniteSpawn()
    {
        double lastMileage = 0f;
        while( true )
        {
            yield return new WaitUntil( ()=> GameManager.IsPlaying );

            // 1m 거리간격 이상일때만 장애물을 생성한다
            // 5m - 0m = 5 > 1m 성립 => lastMileage = 5m
            // 5.5m - 5m = 0.5m > 1m 패스
            // 6.2m - 5m = 1.2m > 1m 성립 => last = 6.2m
            if (GameManager.mileage - lastMileage > Random.Range(spawnInterval.x,spawnInterval.y))
            {
                SpawnObstacle();        

                lastMileage = GameManager.mileage;
            }
        }
    }


    (int, Obstacle) RandomLanePrefab()
    {
        // 랜덤1 : Lane 을 랜덤 생성
        int rndLane = Random.Range(0,trackMgr.laneList.Count);
        
        // GetRandom() => ObstaclePool 가져온다.
        // GetItem() => Pool 안에서 RandomItem 가져온다.
        // as Obstacle => RandomItem 을 Obstacle 로 형변환
        Obstacle prefab = randomGenerator.GetRandom().GetItem() as Obstacle;

        if (prefab == null)
            return (-1, null);

        return (rndLane, prefab);
    }
}
