using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using Deform;
using MoreMountains.Feedbacks;

public enum PlayerMove { Idle = 0, Move, Jump, Slide }
// int 4byte = 32bit 0000 0000 ... 0000 0000 
public enum PlayerState 
{ 
    INVINCIBLE = 1 << 0, 
    MAGNETIC = 1 << 1, 
    MULTIPLE = 1 << 2
}

public class PlayerControl : MonoBehaviour
{
    
    [Space(20)]
    // 속성 : 인스펙터 노출
    [SerializeField] Transform pivot;
    [SerializeField] Collider colNormal, colSlide; // 0: 기본상태 , 1: 슬라이드
    
    [SerializeField] SquashAndStretchDeformer deformLeft, deformRight, deformJumpUp, deformJumpDown, deformSlide;


    [Space(20)]
    [SerializeField] float moveDuration = 0.5f; // 이동에 걸리는 시간
    [SerializeField] Ease moveEase;

    [Space(20)]
    [SerializeField] float jumpDuration = 0.5f;    // 점프 지속 시간
    [SerializeField] float jumpHeight = 3f;      // 점프 높이
    [SerializeField] Ease jumpEase;
    [SerializeField] float[] jumpIntervals = { 0.25f, 0.5f, 0.75f, 0.25f }; // 점프 시퀀스 타이밍 조절

    [Space(20)]
    [SerializeField] float slideDuration = 0.5f;    // 슬라이드 지속 시간


    [Space(20)]
    [SerializeField] MMF_Player feedbackImpact; // 아이템 획득시 연출
    [SerializeField] MMF_Player feedbackCrash;  // 장애물 충돌시 연출
    [SerializeField] MMF_Player feedbackInvincible;  // 무적효과 연출


    // 다른 클래스에 공개는 하지만 인스펙터 노출 안함
    [HideInInspector] public TrackManager trackMgr;

    private PlayerMove state;


    // 내부 사용 : 인스펙터 노출 안함
    private int currentLane = 1;
    private Vector3 targetpos;

    void Start()
    {
        SwitchCollider(true);
    }

    

    void Update()
    {
        if (pivot == null || GameManager.IsPlaying == false || GameManager.IsGameover == true)
            return;

        if (Input.GetButtonDown("Left") && currentLane > 0)
            HandleDirection(-1);

        if (Input.GetButtonDown("Right") && currentLane < trackMgr.laneList.Count-1 )
            HandleDirection(1);
        
        if (Input.GetButton("Jump"))
            HandleJump();          

        if (Input.GetButton("Slide"))
            HandleSlide();        
    }
    
    
    void OnTriggerEnter(Collider other)
    {           
        if (other.tag == "Collectable")        
        {
            feedbackImpact?.PlayFeedbacks();
            other.GetComponentInParent<Collectable>()?.Collect();
        }
        else if (other.tag == "Obstacle" && !GameManager.playerstate.HasAny(PlayerState.INVINCIBLE))
        {
            feedbackCrash?.PlayFeedbacks();
            feedbackInvincible?.PlayFeedbacks();            
        }

        other.enabled = false;
    }



    private Sequence _seqMove;
    // direction -1 이면 왼쪽 , +1 이면 오른쪽
    void HandleDirection(int direction)
    {
        if ( state == PlayerMove.Jump || state == PlayerMove.Slide ) return;

        state = PlayerMove.Move;

        var squash = direction switch { -1 => deformLeft, 1 => deformRight, _ => null };

        if (_seqMove != null)
        {
            _seqMove.Kill(true);
            state = PlayerMove.Move;
        }

        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count-1);

        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, pivot.position.y , pivot.position.z );

        _seqMove = DOTween.Sequence().OnComplete(()=> {squash.Factor = 0; state = PlayerMove.Idle; });
        _seqMove.Append(pivot.DOMove(targetpos, moveDuration));
        _seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration/2f, (v)=> squash.Factor = v ));
        _seqMove.Append(DOVirtual.Float(1f, 0f, moveDuration/2f, (v)=> squash.Factor = v ));
    }

    void HandleJump()
    {
        if ( state != PlayerMove.Idle ) return;

        state = PlayerMove.Jump;

        pivot.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
                .SetEase(jumpEase);

        deformJumpUp.Factor = 0f;
        deformJumpDown.Factor = 0f;  

        Sequence seq = DOTween.Sequence().OnComplete( ()=> state = PlayerMove.Idle );
        seq.Append(DOVirtual.Float( 0f, 1f, jumpDuration * jumpIntervals[0], v => deformJumpUp.Factor = v ));
        seq.Append(DOVirtual.Float( 1f, 0f, jumpDuration * jumpIntervals[1], v => deformJumpUp.Factor = v ));        
        seq.Join(DOVirtual.Float( 0f, 1f, jumpDuration * jumpIntervals[2], v => deformJumpDown.Factor = v ));
        seq.Append(DOVirtual.Float( 1f, 0f, jumpDuration * jumpIntervals[3], v => deformJumpDown.Factor = v ));        
    }

    void HandleSlide()
    {
        if ( state != PlayerMove.Idle ) return;

        state = PlayerMove.Slide;
        SwitchCollider(false);

        Sequence seq = DOTween.Sequence().OnComplete( ()=> 
        {
            state = PlayerMove.Idle;
            SwitchCollider(true);
        });
        seq.Append(DOVirtual.Float( 0f, -1f, slideDuration * 0.25f , v => deformSlide.Factor = v ));
        seq.AppendInterval(slideDuration * 0.25f);
        seq.Append(DOVirtual.Float( -1f, 0f, slideDuration * 0.5f, v => deformSlide.Factor = v ));        
    }

    // b : TRUE -> 기본모드 , FALSE -> 슬라이드
    void SwitchCollider(bool b)
    {
        colNormal.gameObject.SetActive(b);
        colSlide.gameObject.SetActive(!b);
    }



    public void OnCrash(bool b)
    {
        if (b)
        {
            GameManager.life -= 1;
        }

        GameManager.IsPlaying = !b;
    }

    // TRUE : 무적모드 , FALSE : 일반모드
    public void OnInvincible(bool b)
    {
        if (b)
            GameManager.playerstate |= PlayerState.INVINCIBLE; // 추가
        else
            GameManager.playerstate &= ~PlayerState.INVINCIBLE; // 제거
    }

}
