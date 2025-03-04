using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TrackManager : MonoBehaviour
{

    [Space(20)]
    [SerializeField] PlayerControl playerPrefab;
    [SerializeField] Track trackRoad;
    [SerializeField] GameObject trackStart, trackFinish;



    [Space(20)]
    [Range(0f, 50f)] public float scrollSpeed = 10f;
    [Range(1, 100)] public int trackCount = 3;
    [Range(1, 5)] public int countdown = 3;


    [Space(20)]
    [SerializeField] List<Material> CurvedMaterials;
    [Range(0f, 0.5f), SerializeField] float CurvedFrequencyX = 0.2f;   // 주기
    [Range(0f, 10f), SerializeField] float CurvedAmplitudeX = 5f;   // 진폭

    [Range(0f, 0.5f), SerializeField] float CurvedFrequencyY = 0.1f;   // 주기
    [Range(0f, 10f), SerializeField] float CurvedAmplitudeY = 2f;   // 진폭




    private List<Track> trackList = new List<Track>(); // 생성한 트랙들 보관 
    private Transform camTransform;
    private IngameUI uiIngame;


    // 3Lane => 0:Left, 1:Center, 2:Right
    [HideInInspector] public List<Transform> laneList;   // 현재 트랙의 라인 정보를 전달


    // 캐시 데이터
    private int _curveAmount = Shader.PropertyToID("_CurveAmount");

    void Start()
    {
        GameManager.IsGameover = true;
        GameManager.IsPlaying = false;

        // 메인 카메라 Transform을 미리 받아온다.
        camTransform = Camera.main.transform;

        // 인게임 UI 를 미리 받아온다. 없으면 패스
        uiIngame = FindFirstObjectByType<IngameUI>();



        // 씬에 존재하는 모든 오브젝트를 가져와라
        //IngameUI[] uis = FindObjectsByType<IngameUI>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        // 씬에 존재하는 오브젝트들 중 아무거나 하나만 가져와라
        //IngameUI uiAny = FindAnyObjectByType<IngameUI>();
        // 씬에 존재하는 오브젝트들 중 생성한 순서에서 첫번째를 가져와라
        //IngameUI uiFirst = FindFirstObjectByType<IngameUI>();


        SpawnInitialTrack();
        SpawnStartZone();
        SpawnPlayer();

        StartCoroutine(CountdownTrack());

    }

    void Update()
    {
        CountdownTrack();

        if (GameManager.IsPlaying == false|| GameManager.IsGameover == true)
            return;

        RepositionTrack();
        SpawnFinishZone();
        BendTrack();

        GameManager.mileage += scrollSpeed * Time.smoothDeltaTime;
    }



    // 초기 트랙 생성 ( 한번만 실행 )
    void SpawnInitialTrack()
    {
        // 초기값 = 카메라의 z좌표
        Vector3 position = new Vector3(0f, 0f, camTransform.position.z);
        for (int i = 0; i < trackCount; i++)
        {
            //이전 ExitPoint 에 다음 EntryPoint 접합
            Track next = SpawnNextTrack(position, $"Track_{i}");
            position = next.ExitPoint.position;
        }

        BendTrack();
    }

    Track SpawnNextTrack(Vector3 position, string trackname)
    {
        //첫번째 ExitPoint 에 두번째 EntryPoint 접합
        Track Next = Instantiate(trackRoad, position, Quaternion.identity, transform);
        Next.name = trackname;
        Next.trackmgr = this;

        laneList = Next.laneList;

        trackList.Add(Next);

        return Next;
    }




    // 트랙 재배치
    void RepositionTrack()
    {
        if (trackList.Count <= 0) return;

        // 언제 재배치 하나 ? 답 : z축 < 0f -> 삭제 -> 리스트의 마지막에 생성
        // Track_0 => trackList[0]
        // trackList[0] => 트랙의 첫번째 값 가져오기
        // trackList[trackList.Count-1] => 트랙의 마지막 값 가져오기
        if (trackList[0].ExitPoint.position.z < camTransform.position.z)
        {
            Track last = trackList[trackList.Count - 1];
            SpawnNextTrack(last.ExitPoint.position, trackList[0].name);

            Destroy(trackList[0].gameObject);
            trackList.RemoveAt(0);
        }
    }


    float elapsedTime;

    void BendTrack()
    {
        //if (scrollSpeed <= 0f) return;

        // 0f ~ 1f => -1f ~ 1f;

        // 0 ->  *2 -1   -1
        // 1 ->  *2 -1    1
        // 0.5 -> *2 -1   0

        elapsedTime += Time.deltaTime;

        float rndX = Mathf.PerlinNoise1D(elapsedTime * CurvedFrequencyX) * 2f - 1f;
        rndX = rndX * CurvedAmplitudeX;
        float rndY = Mathf.PerlinNoise1D(elapsedTime * CurvedFrequencyY) * 2f - 1f;
        rndY = rndY * CurvedAmplitudeY;

        foreach (var m in CurvedMaterials)
            m.SetVector(_curveAmount, new Vector4(rndX, rndY, 0f, 0f));
    }



    // z값에 해당하는 트랙을 가져오기
    public Track GetTrackByZ(float z)
    {
        // 해당하는 트랙 찾아서 반환
        foreach (Track t in trackList)
        {
            if (z > t.EntryPoint.position.z && z <= t.ExitPoint.position.z)
                return t;
        }

        return null;
    }


    public void SetPhase(PhaseSO phase, float duration = 0.5f)
    {
        DOVirtual.Float(scrollSpeed, phase.scrollSpeed, duration, s => scrollSpeed = s).SetEase(Ease.InOutSine);
    }


    public void StopScrollTrack()
    {
        scrollSpeed = 0f;
    }

    private IEnumerator CountdownTrack()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.IsUiOpened == false);

            uiIngame.ShowInfo($"{countdown--}", 1.5f);
            yield return new WaitForSeconds(1f);

            if (countdown <= 0)
            {

                GameManager.IsPlaying = true;
                GameManager.IsGameover = false;

                yield break;
            }

            yield return new WaitForEndOfFrame();
        }

    }


    void SpawnPlayer()
        {
            PlayerControl player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.trackMgr = this;
        }

        void SpawnStartZone(float zpos = 3f)
        {
            // 3m 앞에 트랙이 있는지 판단
            Track t = GetTrackByZ(zpos);
            GameObject o = Instantiate(trackStart, t.ObstacleRoot);
            Vector3 pos = new Vector3(0f, 0f, zpos);
            o.transform.SetPositionAndRotation(pos, Quaternion.identity);
        }

        GameObject _finishzone;
        void SpawnFinishZone(float zpos = 60f)
        {
            if (_finishzone != null)
                return;

            if (GameManager.mileage + zpos < GameManager.mileageFinish)
                return;

            // 60m 앞에 트랙이 있는지 판단
            Track t = GetTrackByZ(zpos);
            _finishzone = Instantiate(trackFinish, t.ObstacleRoot);
            Vector3 pos = new Vector3(0f, 0f, zpos);
            _finishzone.transform.SetPositionAndRotation(pos, Quaternion.identity);
        }


    }
