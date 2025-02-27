
using UnityEngine;

public class ObstacleTriple : Obstacle
{
    public override void SetLanePostion(int lane, float zpos, TrackManager tm)
    {
        Vector3 lanepos1 = tm.laneList[1].position;
               
        // 위치와 회전 설정
        Vector3 pos = new Vector3(lanepos1.x, lanepos1.y, zpos);
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

}
