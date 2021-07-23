using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// by MK, 항타기 이동 과정을 담당하는 클래스
/// </summary>
public class PileDriverMove : StateMachineBehaviour
{
    private GameObject pileDriver; // 항타기 오브젝트
    private GameObject wire; // 와이어 오브젝트
    float wireDistance; // 와이어 길이
    int moveDistanceValue; // 이동해야할 길이

    private Vector3 moveSpeed = new Vector3(-2 / 47.0f, 0, 0); // 항타기가 이동하는 속도


    /// <summary>
    /// '이동' 상태를 시작할 때 호출되는 함수
    /// 변수에 오브젝트 할당
    /// 필요없어진 StartMove를 초기화
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pileDriver = animator.gameObject;
        wire = pileDriver.GetComponent<PileDriverControl>().Wire();
        moveDistanceValue = pileDriver.GetComponent<PileDriverControl>().MoveDistance();
        wireDistance = 32 - wire.transform.position.y;
        animator.SetBool("StartMove", false);

        pileDriver.GetComponent<PileDriverControl>().RailGo(true);

        pileDriver.GetComponent<PileDriverControl>().Rail().GetComponent<Animator>().speed = 0.5f;

        pileDriver.GetComponent<PileDriverControl>().BackHoe1().GetComponent<PlayableDirector>().Play(pileDriver.GetComponent<PileDriverControl>().BackHoe1Move()); //백호우 1 이동 시퀀스 시작

        pileDriver.GetComponent<PileDriverControl>().NowSequence("항타기 이동");

        Debug.Log("항타기 이동 시작 : " + Time.time);
    }

    /// <summary>
    /// '이동' 상태일 때 호출되는 업데이트 함수
    /// 항타기의 속도에 맞춰 작업 위치로 이동시킨다.
    /// 작업 위치에 도착하면 작업위치에 정확시 재위치시킨다.
    /// 다음 과정으로 넘어가기 위해 StopMove를 true로 바꾼다.
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pileDriver.transform.position += moveSpeed * Time.deltaTime; //moveSpeed만큼 항타기를 이동시킨다.

        if (wire.transform.position.y < 32)
        {
            //Debug.Log(wire.transform.position.y);
            wire.transform.position += Vector3.up * (2 / 47.0f) * (wireDistance / moveDistanceValue) * Time.deltaTime;
        }

        if(pileDriver.transform.position.x < 0) //항타기가 기준점을 통과하면
        {
            pileDriver.transform.position = Vector3.zero; //작업위치에 정확히 위치시킨다.
            animator.SetBool("StopMove", true);
        }
    }

    /// <summary>
    /// '이동' 상태가 종료되면 호출되는 함수
    /// 필요없어진 StopMove를 초기화시킨다.
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wire.transform.position = new Vector3(wire.transform.position.x, 32, wire.transform.position.z);
        animator.SetBool("StopMove", false);
        pileDriver.GetComponent<PileDriverControl>().RailGo(false);
        pileDriver.GetComponent<PileDriverControl>().Rail().GetComponent<Animator>().speed = 0;
        pileDriver.GetComponent<PileDriverControl>().ManMaster().GetComponent<ManMasterControl>().MoveFinish(); //작업반장 이동 종료
        

    }

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
