using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// 차량의 문, 루프, 트렁크, 사이드 미러 개폐용 클래스
/// </summary>
public class ViewInspector : MonoBehaviour
{
    public PlayableDirector doorLF; // 왼쪽 앞 문
    public PlayableDirector doorRF; // 오른쪽 앞 문
    public PlayableDirector doorLB; // 왼쪽 뒷 문
    public PlayableDirector doorRB; // 오른족 뒷 문
    public Transform trunk; // 트렁크
    public Transform sideMirrorL; // 왼쪽 사이드미러
    public Transform sideMirrorR; // 오른쪽 사이드미러

    // 오브젝트별 개폐 속도(%)
    [Range(0, 100)]
    public float doorLFTime; // 왼쪽 앞 문
    [Range(0, 100)]
    public float doorRFTime; // 오른쪽 앞 문
    [Range(0, 100)]
    public float doorLBTime; // 왼쪽 뒷 문
    [Range(0, 100)]
    public float doorRBTime; // 오른족 뒷 문
    [Range(0, 100)]
    public float trunkTime; // 트렁크
    [Range(0, 100)]
    public float sideMirrorLTime; // 왼쪽 사이드미러
    [Range(0, 100)]
    public float sideMirrorRTime; // 오른쪽 사이드미러

    // Update is called once per frame
    void Update()
    {
        doorLF.time = doorLF.duration * doorLFTime /100;
        doorRF.time = doorRF.duration * doorRFTime / 100;
        doorLB.time = doorLB.duration * doorLBTime / 100;
        doorRB.time = doorRB.duration * doorRBTime / 100;
        trunk.localRotation = Quaternion.Euler(0, 0, -80 * trunkTime / 100);
        sideMirrorL.localRotation = Quaternion.Euler(0, -50 * sideMirrorLTime / 100, 0);
        sideMirrorR.localRotation = Quaternion.Euler(0, 50 * sideMirrorRTime / 100, 0);
    }
}
