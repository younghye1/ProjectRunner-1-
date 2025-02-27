using UnityEngine;
using MoreMountains.Feedbacks;


public class CollectableCoin : Collectable
{
    [SerializeField] Transform pivot;
    [SerializeField] MMF_Player feedbackDisappear;

    
    // 해당 코인 증가량
    [SerializeField] uint Add = 1;

    public override void SetLanePostion(int lane, float ypos, float zpos, TrackManager tm)
    {
        lane = Mathf.Clamp(lane, 0, tm.laneList.Count-1);                
        Transform laneTransform = tm.laneList[lane];
        Vector3 pos = new Vector3(laneTransform.position.x, ypos, zpos );

        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    public override void Collect()
    {
        GameManager.coins += Add;

        transform.SetParent(null);
        feedbackDisappear?.PlayFeedbacks();
    }


}
