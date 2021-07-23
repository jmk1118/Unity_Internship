using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 항타기가 케이싱을 제거하는 과정을 담당하는 클래스
/// </summary>
public class PileDriverDeleteCasing : StateMachineBehaviour
{
    private GameObject pileDriver; // 천공 깊이 입력값을 알기위해 가져온다.
    private GameObject carBody; // 항타기 몸체
    private GameObject auger; // 오거를 회전시키기 위해 가져온다.
    private GameObject drill; // 드릴 오브젝트
    private GameObject casingLid; // 케이싱 리드 오브젝트
    private GameObject casing; // 케이싱  오브젝트
    private GameObject wire; // 와이어 오브젝트
    private Vector3 augerRotateSpeed; // 오거 회전 속도
    private float casingHeight; // 케이싱 높이

    private bool bodyRotate; // 몸체가 회전을 해야할 때 true
    private bool augerDown; // 케이싱을 건지기 위해 오거가 내려가는 과정일 때 true
    private bool casingUp; // 케이싱을 건져 올리는 과정일 때 true
    private bool pileDriverRotate; // 케이싱을 건져 올린 후 항타용 망치를 맞추기 위해 차체를 돌리는 과정일 때 true

    private Vector3 augerDownSpeed = new Vector3(0, -0.4f, 0); // 오거 인발 속도

    /// <summary>
    /// '케이싱 제거' 상태에 들어갔을 때 호출되는 함수
    /// 변수에 오브젝트 할당
    /// 불린값 초기화
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pileDriver = animator.gameObject;
        carBody = pileDriver.GetComponent<PileDriverControl>().Body();
        auger = pileDriver.GetComponent<PileDriverControl>().Auger();
        drill = auger.transform.GetChild(0).gameObject;
        casingLid = pileDriver.GetComponent<PileDriverControl>().CasingLid();
        casing = pileDriver.GetComponent<PileDriverControl>().Casing();
        wire = pileDriver.GetComponent<PileDriverControl>().Wire();
        augerRotateSpeed = new Vector3(0, 0, pileDriver.GetComponent<PileDriverControl>().AugerRotateSpeed());
        casingHeight = pileDriver.GetComponent<PileDriverControl>().PileLength() - pileDriver.GetComponent<PileDriverControl>().BoringDepth() + 1;

        bodyRotate = true;
        augerDown = false;
        casingUp = false;
        pileDriverRotate = false;

        pileDriver.GetComponent<PileDriverControl>().NowSequence("케이싱 제거");

        Debug.Log("항타기 케이싱 제거 작업 시작 : " + Time.time);
    }

    /// <summary>
    /// '케이싱 제거' 상태일 때 호출되는 업데이트 함수
    /// 차체 회전 -> 오거 하강 -> 케이싱 건져 올리기 -> 차체 회전을 진행한다.
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(bodyRotate)
        {
            carBody.transform.Rotate(Vector3.forward * Time.deltaTime);
            //Debug.Log(carBody.transform.rotation.eulerAngles);
            if (carBody.transform.rotation.eulerAngles.y > 359.5f) 
            {
                carBody.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                bodyRotate = false;
                augerDown = true;
            }
        }
        else if(augerDown) //케이싱을 건지기 위해 오거가 내려가는 과정
        {
            auger.transform.position += augerDownSpeed * Time.deltaTime; //오거 포지션 다운
            drill.transform.Rotate(augerRotateSpeed);
            if(auger.transform.position.y <= casing.transform.position.y) //오거 포지션이 일정 이상 내려가면 다음 페이즈로 넘어간다.
            {
                pileDriver.GetComponent<PileDriverControl>().DirtOn();
                augerDown = false;
                casingUp = true;
            }
        }
        else if (casingUp) //케이싱을 건져 올리는 과정
        {
            casing.transform.parent = casingLid.transform;
            casing.transform.position = casingLid.transform.position;

            casingLid.transform.position += augerDownSpeed * -1 * Time.deltaTime; //케이싱 포지션 업
            casing.transform.Rotate(augerRotateSpeed * -180 * Time.deltaTime);
            auger.transform.position += augerDownSpeed * -1 * Time.deltaTime;
            drill.transform.Rotate(augerRotateSpeed * 180 * Time.deltaTime);

            if(casing.transform.position.y >= 0)
                pileDriver.GetComponent<PileDriverControl>().DirtOff();

            if (casing.transform.position.y >= casingHeight) //케이싱 포지션이 casingLid에 다다르면 다음 페이즈로 넘어간다.
            {
                casingUp = false;
                pileDriverRotate = true;
            }
        }
        else if(pileDriverRotate) //차체를 돌리는 과정
        {
            carBody.transform.Rotate(Vector3.back * Time.deltaTime); //차체를 회전한다.
            //Debug.Log(pileDriver.transform.rotation.eulerAngles.y);
            if (carBody.transform.rotation.eulerAngles.y < 340.0f && carBody.transform.rotation.eulerAngles.y != 0) //차체가 일정 이상 회전하면 항타로 넘어간다.
            {
                animator.SetBool("StopDeleteCasing", true);
                carBody.transform.rotation = Quaternion.Euler(new Vector3(-90, 340.0f, 0));
            }
        }

        
        if (wire.transform.position.y < 70)
            wire.transform.position += Vector3.up * Time.deltaTime;
        else
            wire.SetActive(false);
        
    }

    // '케이싱 제거' 상태가 종료됬을 때 호출되는 함수
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("StopDeleteCasing", false);   
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
