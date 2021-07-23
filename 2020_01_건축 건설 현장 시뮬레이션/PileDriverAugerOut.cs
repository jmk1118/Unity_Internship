using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 오거를 인발하는 클래스
/// </summary>
public class PileDriverAugerOut : StateMachineBehaviour
{
    private GameObject pileDriver; // 항타기
    private GameObject auger; // 오거 오브젝트,인발시키기 위해 가져온다.
    private GameObject drill; // 드릴
    private GameObject casingLid; // 케이싱리드(케이싱 윗부분)
    private GameObject hammer; // 망치
    private float boringDepth; // 입력받은 천공깊이
    private float pileLength; // 입력받은 말뚝길이
    private Vector3 augerRotateSpeed; // 오거 회전 속도

    private GameObject wire; // 와이어
    private GameObject pile; // 말뚝

    private Vector3 augerUpSpeed = new Vector3(0, 0.213f, 0); //오거 인발 속도

    /// <summary>
    /// '오거 인발' 상태가 시작될 때 실행되는 함수. 변수에 오브젝트들을 할당한다.
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pileDriver = animator.gameObject;
        auger = pileDriver.GetComponent<PileDriverControl>().Auger();
        drill = auger.transform.GetChild(0).gameObject;
        casingLid = pileDriver.GetComponent<PileDriverControl>().CasingLid();
        hammer = pileDriver.GetComponent<PileDriverControl>().Hammer();
        boringDepth = pileDriver.GetComponent<PileDriverControl>().BoringDepth();
        pileLength = pileDriver.GetComponent<PileDriverControl>().PileLength();
        augerRotateSpeed = new Vector3(0, 0, pileDriver.GetComponent<PileDriverControl>().AugerRotateSpeed());

        wire = pileDriver.GetComponent<PileDriverControl>().Wire();
        pile = pileDriver.GetComponent<PileDriverControl>().Pile();

        pileDriver.GetComponent<PileDriverControl>().NowSequence("오거 인발");

        Debug.Log("항타기 오거 인발 시작 : " + Time.time);
    }

    /// <summary>
    /// '오거 인발' 상태일때 실행되는 업데이트 함수.
    /// 드릴을 역회전 시킨다.
    /// 오거를 일정 속도로 인발한다.
    /// 오거가 완전히 인발되면 파일 준비 과정으로 넘어간다.
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        drill.transform.Rotate(augerRotateSpeed * 180 * Time.deltaTime); //드릴을 천공때와는 역회전 시킨다.
        auger.transform.position += augerUpSpeed * Time.deltaTime; //오거를 올린다.

        /*
        if(casingLid.transform.position.y < pileLength - boringDepth + 1) //casingLid가 튀어나온 말뚝만큼 높아지게 이동한다.
            casingLid.transform.position += augerUpSpeed * Time.deltaTime;
        */

        if (hammer.transform.position.y < pileLength - boringDepth + 1) //망치를 박힌 말뚝보다 높아지게 이동시킨다.
            hammer.transform.position += augerUpSpeed * Time.deltaTime;

        if (auger.transform.position.y > pileLength - boringDepth + 4) //오거가 완전히 인발되면 다음 과정으로 넘어간다.
        {
            animator.SetBool("StopAugerOut", true);
        }


        pile.GetComponent<Pile_Control>().PileMove();

    }

    /// <summary>
    /// '오거 인발' 상태가 종료되면 호출되는 함수
    /// 다음 상태를 호출할 때 사용한 StopAugerOut 변수를 다시 false로 전환한다.
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("StopAugerOut", false);
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
