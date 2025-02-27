

using UnityEngine;


// 싱글의 파생형
// Concrete 클래스
// Abstract -> Concrete
public class ObstacleDouble : Obstacle
{  

    // override 기각,무시
    // 부모에 있는 SetLanePostion 은 무시 ,
    // 현재 나의 SetLanePostion 을 사용
    public override void SetLanePostion(int lane, float zpos, TrackManager tm)
    {
        // lane 0 => 0 , 1 의 중심
        // lane 1 => 1 , 2 의 중심        

        lane = Mathf.Clamp(lane, 0, tm.laneList.Count-1);        
        Vector3 lanepos0 = tm.laneList[0].position;
        Vector3 lanepos1 = tm.laneList[1].position;
        Vector3 lanepos2 = tm.laneList[2].position;

        float posX = 0f;
        
        // 내부에서 자체 랜덤으로 Lane 결정
        int rndLane = Random.Range(0,tm.laneList.Count-1);
        if (rndLane == 0)
        {
            posX = (lanepos0.x + lanepos1.x) / 2;
        }
        else if (rndLane == 1)
        {
            posX = (lanepos1.x + lanepos2.x) / 2;
        }
        
        // 위치와 회전 설정
        Vector3 pos = new Vector3(posX, lanepos1.y, zpos);
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

}
