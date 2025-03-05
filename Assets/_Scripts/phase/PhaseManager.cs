using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomInspector;


public class PhaseManager : MonoBehaviour
{

    [HorizontalLine("기본속성"), HideField] public bool _l0;
    [SerializeField] float updateInterval = 1f;


    [HorizontalLine("Phase Data 속성"), HideField] public bool _l1;
    [SerializeField, Foldout] List<PhaseSO> phaseList = new List<PhaseSO>();
    
    private TrackManager trkMgr;
    private ObstacleManager obsMgr;
    private CollectableManager colMgr;
    private IngameUI uiIngame;




    IEnumerator Start()
    {
        trkMgr = FindFirstObjectByType<TrackManager>();
        obsMgr = FindFirstObjectByType<ObstacleManager>();
        colMgr = FindFirstObjectByType<CollectableManager>();
        uiIngame = FindFirstObjectByType<IngameUI>();

        GetFinishline();

        uiIngame.SetMileage(phaseList);
        
        yield return new WaitUntil( ()=> GameManager.IsGameover == false && GameManager.IsPlaying == true);
        StartCoroutine(IntervalUpdate());
    }


    IEnumerator IntervalUpdate()
    {
        if(phaseList == null || phaseList.Count <= 0)
            yield break;


        int i = 0;

        while( true )
        {
            PhaseSO phase = phaseList[i];
            if (GameManager.mileage >= phase.mileage)
            {
                SetPhase(phase);
                i++;
            }

            if (i >= phaseList.Count)
            {
                GameClear(phase);
                yield break;
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }

    void GetFinishline()
    {
        PhaseSO phaseEnd = phaseList.LastOrDefault();
        GameManager.mileageFinish = phaseEnd.mileage;
    }


    void SetPhase(PhaseSO phase)
    {
        uiIngame?.SetPhase(phase);
        trkMgr?.SetPhase(phase);
        obsMgr?.SetPhase(phase);
        colMgr?.SetPhase(phase);
    }

    void GameClear(PhaseSO phase)
    {
        SetPhase(phase);

        GameManager.IsPlaying = false;
        GameManager.IsGameover = true;
    }

}
