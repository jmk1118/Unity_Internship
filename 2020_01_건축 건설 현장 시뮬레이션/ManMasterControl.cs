using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 관리자 오브젝트의 애니메이션 변경 함수를 가진 클래스
/// </summary>
public class ManMasterControl : MonoBehaviour
{
    /// <summary>
    /// 관리자 애니메이션, 위치를 초기화한다.
    /// </summary>
    public void Reset()
    {
        GetComponent<Animator>().Rebind();
    }

    /// <summary>
    /// 현재 관리자의 위치에서 멈춰선다.
    /// </summary>
    public void MoveFinish()
    {
        GetComponent<Animator>().SetBool("MoveFinish", true);
    }

    /// <summary>
    /// 천공 작업을 종료한다.
    /// </summary>
    public void BoringFinish()
    {
        GetComponent<Animator>().SetBool("BoringFinish", true);
    }

    /// <summary>
    /// 구멍 파기 지휘를 시작한다.
    /// </summary>
    public void StartPile()
    {
        GetComponent<Animator>().SetBool("StartPile", true);
    }

    /// <summary>
    /// 말뚝질을 시작한다.
    /// </summary>
    public void StartRebound()
    {
        GetComponent<Animator>().SetBool("StartRebound", true);
    }

    /// <summary>
    /// 관리자의 애니메이션을 종료한다.
    /// </summary>
    public void Finish()
    {
        GetComponent<Animator>().SetBool("Finish", true);
    }
}
