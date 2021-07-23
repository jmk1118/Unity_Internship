using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

/// <summary>
/// by MK, 수직도 조정 상태를 담당하는 클래스
/// </summary>
public class PileDriverVertical : StateMachineBehaviour
{
    /// <summary>
    /// '수직도 조정' 상태에 들어가면 호출되는 함수
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<PileDriverControl>().NowSequence("수직도 조정");
        animator.gameObject.GetComponent<PileDriverControl>().BackHoe1().GetComponent<PlayableDirector>().Play(animator.gameObject.GetComponent<PileDriverControl>().BackHoe1Vertical()); //백호우 1 수직도 조정 시퀀스 시작


        Debug.Log("수직도 조정 시작" + Time.time);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{ 
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
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
