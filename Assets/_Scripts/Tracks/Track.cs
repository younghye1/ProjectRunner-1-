using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public Transform EntryPoint;
    public Transform ExitPoint;  
    public List<Transform> laneList;

    public Transform ObstacleRoot;
    public Transform CollectableRoot;


    [HideInInspector] public TrackManager trackmgr;


    void LateUpdate()
    {
        if (GameManager.IsPlaying == false || GameManager.IsGameover == true)
            return;
            
        Scroll();
    }  

    void Scroll()
    {
        if (trackmgr == null) return;

        transform.position += Vector3.back * trackmgr.scrollSpeed * Time.smoothDeltaTime;
        
        //Time.deltaTime            => 매프레임당 1번 호출 될때 간격(Interval Time )
        //Time.fixedDeltaTime       => 0.02 간격 
        //Time.smoothDeltaTime      => deltaTime 평균 => 값이 고르게 나온다

        // fixedDelta < delta < smoothDelta

        //Debug.Log( $"{name} : local = {EntryPoint.localPosition} , world = {EntryPoint.position}");
    }
}
