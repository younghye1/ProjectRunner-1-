using Unity.Mathematics;
using UnityEngine;

// 싱글 형태 장애물
// 베이스 클래스
public class ObstacleSingle : Obstacle
{
    public enum SingleType { NONE, BLOCK }

    public SingleType singletype = SingleType.NONE;

    public override void SetLanePostion(int lane, float zpos, TrackManager tm)
    {
        // Lane 위치
        lane = Mathf.Clamp(lane, 0, tm.laneList.Count-1);                
        Transform laneTransform = tm.laneList[lane];
        Vector3 pos = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos );

        //Instantiate(프리팹, 포지션, 회전, 부모);
        
        //Instantiate(프리팹, 부모);
        //SetPositionAndRotation(포지션 ,회전);

        // 위치와 회전 설정
        transform.SetPositionAndRotation(pos, quaternion.identity);
    }
}
