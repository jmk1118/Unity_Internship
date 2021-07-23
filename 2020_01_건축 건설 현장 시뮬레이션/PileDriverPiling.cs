using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 말뚝 설치 과정을 담당하는 클래스
/// </summary>
public class PileDriverPiling : StateMachineBehaviour
{
    private GameObject wire; // 와이어 오브젝트
    private GameObject pile; // 말뚝 오브젝트
    private GameObject casing; // 케이싱

    private bool moving; // 말뚝 이동 과정
    private bool droping; // 말뚝 낙하 과정
    private Vector3 casingPosition; // 케이싱의 위치
    private Vector3 casingDirection; // 말뚝 기준으로 케이싱이 있는 방향
    private float boringDepth; // 천공 깊이 입력값
    private float pileLength; // 말뚝 길이 입력값

    /// <summary>
    /// '말뚝 설치' 상태가 시작될 때 호출되는 함수
    /// 와이어, 말뚝, 케이싱 초기화
    /// 불린값 초기화
    /// 케에싱 위치, 방향 값 입력
    /// 천공 깊이, 말뚝 길이 가져오기
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wire = animator.gameObject.GetComponent<PileDriverControl>().Wire();
        pile = animator.gameObject.GetComponent<PileDriverControl>().Pile();
        casing = animator.gameObject.GetComponent<PileDriverControl>().Casing();

        moving = true;
        droping = false;
        casingPosition = casing.transform.position;
        casingDirection = Vector3.Lerp(casingPosition, pile.transform.position, 1);
        casingDirection = Vector3.Normalize(new Vector3(casingDirection.x, 0, casingDirection.z));

        boringDepth = animator.gameObject.GetComponent<PileDriverControl>().BoringDepth();
        pileLength = animator.gameObject.GetComponent<PileDriverControl>().PileLength();

        animator.gameObject.GetComponent<PileDriverControl>().NowSequence("파일 설치");

        Debug.Log("항타기 말뚝 이동 및 낙하 시작 : " + Time.time);
    }

    /// <summary>
    /// '말뚝 설치' 상태일 때 호출되는 업데이트 함수
    /// 말뚝을 끌고오고, 말뚝을 낙하시키는 과정으로 이루어진다.
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(moving) //말뚝을 가져오는 과정
        {
            //wire.transform.position -= casingDirection * 2 * Time.deltaTime; //와이어와 말뚝을 함번에 가져온다.
            pile.transform.position -= casingDirection * 2 * Time.deltaTime;

            if (Vector3.Distance(pile.transform.position - Vector3.up * pile.transform.position.y, casingPosition - Vector3.up * casingPosition.y) < 0.5f) //xz평면의 거리값으로 가까워지면 낙하한다.
            {
                wire.transform.GetChild(9).GetComponent<HingeJoint>().connectedBody = wire.transform.GetChild(9).GetChild(0).GetComponent<Rigidbody>(); //와이어와 말뚝의 연결을 끊는다.
                pile.transform.position = new Vector3(casingPosition.x, pile.transform.position.y, casingPosition.z); //정확하게 떨어뜨리기 위해 말뚝 위치를 0,0에 맞추고 떨어뜨린다.
                pile.transform.rotation = Quaternion.Euler(-90, 0, 0);
                moving = false;
                droping = true;
            }
        }
        if(droping) //말뚝을 낙하시키는 과정
        {
            pile.transform.position += Vector3.down * 10 * Time.deltaTime; //말뚝을 낙하시킨다.

            if(pile.transform.position.y <= -boringDepth) //말뚝을 입력받은 천공깊이값만큼 박는다.
            {
                pile.transform.position = new Vector3(0, -boringDepth, 0);
                animator.SetBool("StopPiling", true);
            }
        }
    }

    /// <summary>
    /// '말뚝 설치' 상태가 종료됐을 때 호출되는 함수
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("StopPiling", false);
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
