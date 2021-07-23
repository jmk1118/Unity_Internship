using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 말뚝을 올리는 과정을 담당하는 클래스
/// </summary>
public class PileDriverPileReady : StateMachineBehaviour
{
    private GameObject pileDriver; // 항타기
    private GameObject carBody; // 항타기 차체(바퀴 위)
    private GameObject wire; // 와이어 오브젝트
    private GameObject pile; // 말뚝 오브젝트
    private Vector3 wireSpeed; // 와이어가 올라가는 속도
    private int pileLength; // 말뚝 길이 입력값

    private Vector3 bodyRotateSpeed = new Vector3(0, 0, -1); // 차체 회전 속도

    /// <summary>
    /// '말뚝 이동' 상태가 시작될 때 호출되는 함수
    /// 변수에 오브젝트 할당
    /// 말뚝을 와이어에 걸린 상태로 만든다. (PileControl의 함수 호출)
    /// 와이어가 올라가는 속도 초기화
    /// 말뚝 길이 입력값을 가져온다.
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pileDriver = animator.gameObject;
        carBody = pileDriver.GetComponent<PileDriverControl>().Body();
        wire = pileDriver.GetComponent<PileDriverControl>().Wire();
        pile = pileDriver.gameObject.GetComponent<PileDriverControl>().Pile();
        pile.GetComponent<Pile_Control>().HangerOn();
        wireSpeed = new Vector3(0, 0.7f, 0);
        pileLength = animator.gameObject.GetComponent<PileDriverControl>().PileLength();

        pileDriver.GetComponent<PileDriverControl>().NowSequence("파일 준비");

        pileDriver.GetComponent<PileDriverControl>().ManMaster().GetComponent<ManMasterControl>().StartPile();

        

        Debug.Log("항타기 말뚝 준비 시작 : " + Time.time);
    }

    /// <summary>
    /// '말뚝 이동' 상태일 때 호출되는 업데이트 함수
    /// 와이어를 와이어가 올라가는 속도만큼 올린다.
    /// 말뚝 길이 + 일정 높이에 다다르면 파일 설치 과정으로 넘어간다.
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (carBody.transform.rotation.eulerAngles.y > 350.0f || carBody.transform.rotation.eulerAngles.y == 0) //차체가 18도 돌때까지 회전
            carBody.transform.Rotate(bodyRotateSpeed * Time.deltaTime);

        if (wire.transform.position.y < pileLength + 40.0f) //와이어를 올림
        {
            wire.transform.position += wireSpeed * Time.deltaTime;
            pile.transform.position += wireSpeed * Time.deltaTime;
        }
        else //와이어가 다 올라오면 말뚝의 움직임을 고정시키고 다음 과정으로 넘어간다.
        {
            animator.SetBool("StopPileReady", true);
        }
    }

    /// <summary>
    /// '말뚝 이동' 상태가 끝났을 때 호출되는 함수
    /// 말뚝을 와이어에 걸리지 않은 상태로 만든다.
    /// StopPiling 변수를 초기화한다.
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("StopPileReady", false);
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
