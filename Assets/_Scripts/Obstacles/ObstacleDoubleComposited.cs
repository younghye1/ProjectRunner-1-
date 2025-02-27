using System.Collections.Generic;
using UnityEngine;


public class ObstacleDoubleComposited : ObstacleDouble
{
    public List<Obstacle> compositedPrefabs;

    //public : 전체 공개
    //private : 비공개 (본인만 공개)
    //protected : 본인과 자식에게만 공개
    private List<Vector3> spawnedPos = new List<Vector3>();

    private void Start()
    {
        SpawnComposited();
    }

    private void SpawnComposited()
    {        
        //2개의 싱글 장애물 스폰
        // spawnedPos (1번,2번) 위치에 생성
        
        foreach( var p in spawnedPos)
        {
            int rnd = Random.Range(0, compositedPrefabs.Count);
            Obstacle prefab = compositedPrefabs[rnd];

            var o = Instantiate(prefab, p, Quaternion.identity, transform);
            Vector3 localpos = o.transform.localPosition;
            o.transform.localPosition = new Vector3(localpos.x, 0f, 0f);
        };
    }

    public override void SetLanePostion(int lane, float zpos, TrackManager tm)
    {
        spawnedPos.Clear();

        // lane 0 => 0 , 1 의 중심
        // lane 1 => 1 , 2 의 중심        

        lane = Mathf.Clamp(lane, 0, tm.laneList.Count-1);        
        Vector3 lanepos0 = tm.laneList[0].position;
        Vector3 lanepos1 = tm.laneList[1].position;
        Vector3 lanepos2 = tm.laneList[2].position;
        
        if (lane == 0) // 좌,센터
        {
            spawnedPos.Add(lanepos0);
            spawnedPos.Add(lanepos1);
        }
        else if (lane == 1) // 좌,우
        { 
            spawnedPos.Add(lanepos0);
            spawnedPos.Add(lanepos2);
        }
        else if (lane == 2) // 센터,우
        {
            spawnedPos.Add(lanepos1);
            spawnedPos.Add(lanepos2);            
        }
        
        // 위치와 회전 설정
        Vector3 pos = new Vector3(lanepos1.x, lanepos1.y, zpos);
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

}
