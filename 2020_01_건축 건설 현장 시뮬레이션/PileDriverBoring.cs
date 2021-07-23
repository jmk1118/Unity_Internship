using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// by MK, 항타기 천공 과정을 담당하는 클래스
/// </summary>
public class PileDriverBoring : StateMachineBehaviour
{
    private GameObject pileDriver; // 천공 깊이 입력값을 알기위해 가져온다.
    private GameObject auger; // 오거(드릴 윗부분)
    private GameObject drill; // 드릴
    private GameObject casingLid; // 케이싱리드(케이싱 윗부분)
    private GameObject casing; // 케이싱을 회전시키기 위해 가져온다.
    private int boringDepthValue; // 오거를 얼마나 넣어야하는지 알기위해 가져오는 천공 깊이 입력값.
    private Vector3 augerRotateSpeed; // 오거 회전 속도

    private Vector3 augerDownSpeed = new Vector3(0, -4f / 523f, 0); // 오거 하강 속도

    /// <summary>
    /// '천공' 상태에 들어갔을 때 호출되는 함수
    /// 항타기, 오거, 케이싱을 선언한다.
    /// 오거를 얼마나 깊숙히, 오래 천공시켜야 하는지 알기 위해 천공 깊이 입력값을 가져온다.
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pileDriver = animator.gameObject; 
        auger = pileDriver.GetComponent<PileDriverControl>().Auger();
        drill = auger.transform.GetChild(0).gameObject;
        casingLid = pileDriver.GetComponent<PileDriverControl>().CasingLid();
        casing = pileDriver.GetComponent<PileDriverControl>().Casing();
        boringDepthValue = pileDriver.GetComponent<PileDriverControl>().BoringDepth();
        augerRotateSpeed = new Vector3(0, 0, pileDriver.GetComponent<PileDriverControl>().AugerRotateSpeed());

        pileDriver.GetComponent<PileDriverControl>().DirtOn(); //흙먼지를 활성화시킨다.

        pileDriver.GetComponent<PileDriverControl>().Lorry().GetComponent<PlayableDirector>().Play(pileDriver.GetComponent<PileDriverControl>().LorryDriveLeft());

        pileDriver.GetComponent<PileDriverControl>().NowSequence("천공 및 케이싱 삽입");

        pileDriver.GetComponent<PileDriverControl>().ManSub().GetComponent<ManSubControl>().BoringStart(); //비계공 위치이동

        Debug.Log("항타기 천공 시작 : " + Time.time);
    }

    /// <summary>
    /// '천공' 상태일 때 실행되는 업데이트 함수
    /// 일정 속도로 오거를 회전시킨다.
    /// 일정 속도로 오거를 땅 속으로 밀어넣는다.
    /// 입력된 천공 깊이에 도달하면 오거 인발 과정으로 넘어간다.
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        drill.transform.Rotate(augerRotateSpeed * -180 * Time.deltaTime);
        casing.transform.Rotate(augerRotateSpeed * 180 * Time.deltaTime); // 케이싱은 드릴과는 역회전한다.
        auger.transform.position += augerDownSpeed * Time.deltaTime; // 523초에 4m진행 속도, 오거와 함께 드릴도 같이 내려간다.
        casingLid.transform.position += augerDownSpeed * Time.deltaTime; // 케이싱리드와 케이싱이 함꼐 내려간다.

        if (auger.transform.position.y < -1 * boringDepthValue) // 입력받은 천공깊이값에 도달하면 하강을 종료한다.
        {
            pileDriver.GetComponent<PileDriverControl>().SteamON();
            animator.SetBool("StopBoring", true);

            pileDriver.GetComponent<PileDriverControl>().ManSub().GetComponent<ManSubControl>().PileReady(); //비계공 파일 준비
        }
    }

    /// <summary>
    /// '천공' 상태가 종료될 때 호출되는 함수
    /// 케이싱과 케이싱lid의 연결을 끊는다.
    /// 필요없어진 StopBoring을 초기화한다.
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        casing.transform.parent = null; //케이싱을 케이싱리드에서 분리시킨다.
        pileDriver.GetComponent<PileDriverControl>().DirtOff(); //흙먼지를 비활성화한다.

        pileDriver.GetComponent<PileDriverControl>().ManMaster().GetComponent<ManMasterControl>().BoringFinish();
        animator.SetBool("StopBoring", false);
        //Debug.Log("천공 종료 : " + Time.time);
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
