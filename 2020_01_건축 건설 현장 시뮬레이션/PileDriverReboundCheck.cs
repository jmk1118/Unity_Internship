using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 항타기 리바운드 체크를 관리하는 스크립트
/// </summary>
public class PileDriverReboundCheck : StateMachineBehaviour
{
    private GameObject pileDriver; // 항타기
    private GameObject hammer; // 망치
    private float boringDepth; // 구멍 깊이
    private float pileLength; // 말뚝 길이

    private bool hammerUp; // 망치를 올리고 내리고를 관리하는 불린값
    private float time; // 망치질 사이의 간격 계산용
    private float hammerStop; // 망치질을 멈춰야 하는 길이

    /// <summary>
    /// '리바운드 체크' 상태가 시작될 때 호출되는 함수
    /// 항타기, 해머 선언
    /// 불린값, 시간 초기화
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pileDriver = animator.gameObject;
        hammer = pileDriver.GetComponent<PileDriverControl>().Hammer();
        boringDepth = pileDriver.GetComponent<PileDriverControl>().BoringDepth();
        pileLength = pileDriver.GetComponent<PileDriverControl>().PileLength();

        hammerUp = true;
        time = Time.time;
        hammerStop = pileLength - boringDepth;

        pileDriver.GetComponent<PileDriverControl>().NowSequence("리바운드 체크");

        Debug.Log("항타기 리바운드 체크 시작 : " + Time.time);
    }

    /// <summary>
    /// '리바운드 체크' 상태일 때 호출되는 업데이트 함수
    /// 망치가 내려간 뒤 5초의 간격을 두고 다시 망치를 올리는 일을 반복한다.
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hammerUp)
        {
            if (Time.time - time > 5.0f)
            {
                hammer.transform.position += Vector3.up * 3 * Time.deltaTime;
                if (hammer.transform.position.y > hammerStop + 5)
                {
                    hammerUp = false;
                    hammer.GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
        else
        {
            //hammer.transform.position += Vector3.down * 15 * Time.deltaTime;
            if (hammer.transform.position.y < hammerStop)
            {
                hammer.GetComponent<Rigidbody>().isKinematic = true;
                hammerUp = true;
                time = Time.time;
            }
        }
    }

    /// <summary>
    /// '리바운드 체크' 상태가 종료됐을 때 호출되는 함수
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hammer.GetComponent<Rigidbody>().isKinematic = true; //계속 떨어지는 것을 막기 위해 리지드바디를 설정한다.    
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
