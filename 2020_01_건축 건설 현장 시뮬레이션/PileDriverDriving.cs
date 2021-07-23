using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// by MK, 항타기의 항타 과정을 담당하는 클래스
/// </summary>
public class PileDriverDriving : StateMachineBehaviour
{
    private GameObject pileDriver; // 항타기
    private GameObject hammer; // 망치
    private float boringDepth; // 파야하는 깊이
    private float pileLength; // 말뚝의 길이

    private bool hammerUp; // 망치가 올라갔다 내려가는 과정을 담당하는 불린값
    private float hammerStop; // 망치가 멈춰야 할 높이
    private float time;

    /// <summary>
    /// '항타' 상태에 들어갈 때 호출되는 함수
    /// 변수에 오브젝트 할당
    /// 불린값 초기화
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pileDriver = animator.gameObject;
        hammer = pileDriver.GetComponent<PileDriverControl>().Hammer();
        //hammer.transform.GetChild(0).position += Vector3.down;
        boringDepth = pileDriver.GetComponent<PileDriverControl>().BoringDepth();
        pileLength = pileDriver.GetComponent<PileDriverControl>().PileLength();

        hammerUp = true;
        hammerStop = pileLength - boringDepth;
        time = Time.time;

        pileDriver.GetComponent<PileDriverControl>().BackHoe1().GetComponent<PlayableDirector>().Play(pileDriver.GetComponent<PileDriverControl>().BackHoe1AfterPile());

        pileDriver.GetComponent<PileDriverControl>().NowSequence("항타");

        Debug.Log("항타기 항타 시작 : " + Time.time);
    }

    /// <summary>
    /// '항타' 상태일 때 호출되는 업데이트 함수
    /// 망치가 올라갔다 내려가는 과정 반복
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hammerUp)
        {
            if (Time.time - time > 0.5f)
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

    // '항타' 상태가 종료될 때 호출되는 함수
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //hammer.transform.GetChild(0).position += Vector3.up;
        hammer.GetComponent<Rigidbody>().isKinematic = true;    
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
