using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 조수 오브젝트의 애니메이션 변경 함수를 가진 클래스
/// </summary>
public class ManSubControl : MonoBehaviour
{
    /// <summary>
    /// 조수 애니메이션, 위치를 초기화한다.
    /// </summary>
    public void Reset()
    {
        GetComponent<Animator>().Rebind();
    }

    /// <summary>
    /// 현재 조수의 위치에서 멈춰선다.
    /// </summary>
    public void MoveStart()
    {
        GetComponent<Animator>().SetBool("MoveStart", true);
    }
    
    /// <summary>
    /// 천공 작업을 시작한다.
    /// </summary>
    public void BoringStart()
    {
        GetComponent<Animator>().SetBool("BoringStart", true);
    }

    /// <summary>
    /// 말뚝 박기 보조를 시작한다.
    /// </summary>
    public void PileReady()
    {
        GetComponent<Animator>().SetBool("PileReady", true);
    }

    /// <summary>
    /// 말뚝질 과정을 기록한다.
    /// </summary>
    public void ReboundCheck()
    {
        GetComponent<Animator>().SetBool("ReboundCheck", true);
    }
}
