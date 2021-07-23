using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 항타기 리바운드 체크 준비 과정을 담당하는 클래스
/// </summary>
public class PileDriverReboundCheckReady : StateMachineBehaviour
{
    private GameObject pileDriver; // 항타기

    /// <summary>
    /// '리바운드 체크 준비' 상태가 시작될 때 호출되는 함수
    /// 항타기 변수에 오브젝트 할당
    /// 작업반장의 리바운드 체크 준비 애니메이션 실행
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pileDriver = animator.gameObject;

        pileDriver.GetComponent<PileDriverControl>().NowSequence("리바운드 체크 준비");

        pileDriver.GetComponent<PileDriverControl>().ManMaster().GetComponent<ManMasterControl>().StartRebound();

        pileDriver.GetComponent<PileDriverControl>().ManSub().GetComponent<ManSubControl>().ReboundCheck(); //비계공 리바운드 준비

        Debug.Log("항타기 리바운드 체크 준비 시작 : " + Time.time);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //     
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
