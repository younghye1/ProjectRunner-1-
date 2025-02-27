

using UnityEngine;

public class LaneWave : Lane
{
    public string Name => "WavePattern";

    private LaneData data;
    private float amplitude = 1.5f; // 진폭( Amplitude )
    private float frequency = 4f; // 주기( Frequency )

    private float elapsed = 0f;
    
    public void Initialize(int maxlane)
    {
        data.maxLane = maxlane;
        
        System.Random random = new System.Random();
        data.currentLane = random.Next(0, maxlane);

        elapsed = 0f;
    }

    public LaneData GetNextLane()
    {        
        data.currentY = Mathf.Abs(Mathf.Sin(elapsed * Mathf.PI * frequency)) * amplitude;
        elapsed += 0.1f;

        return data;
    }



    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;

    //     for (int i=0; i<count; i++)
    //     {
    //         float t = (float)i/(count-1);
    //         Vector3 v = Vector3.Lerp(transform.position, transform.position + transform.forward * offsetZ, t );
    //         // PI => 3.14 => 180도 , 2PI => 360도
    //         // Sin => -1f ~ 1f => 음수 를 양수로 전환
    //         float s = Mathf.Abs(Mathf.Sin(t * Mathf.PI * frequency)) * amplitude;
    //         v = new Vector3(v.x, v.y + s ,v.z);
    //         Gizmos.DrawCube(v, Vector3.one*0.5f);
    //     }
    // }
}
