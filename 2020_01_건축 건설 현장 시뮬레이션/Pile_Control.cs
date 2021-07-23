using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 말뚝 오브젝트를 초기화하고, 와이어 끝부분에 붙이고, 이동시키는 클래스
/// </summary>
public class Pile_Control : MonoBehaviour
{
    [SerializeField] private Transform hanger; // 와이어 끝 부분에 존재하는 오브젝트
    private Vector3 updatePosition; // 회전용 3차원벡터

    private int pileLength; // 말뚝의 길이
    private Vector3 groundVector; // 말뚝에서 hanger를 가리키는 벡터
    private Vector3 hangerVector; // hanger에서 와이어를 가리키는 벡터

    /// <summary>
    /// 회전용 3차원벡터 초기화
    /// </summary>
    void Start()
    {
        updatePosition = Vector3.zero;
    }

    /// <summary>
    /// 말뚝이 와이어를 바라보는 방향을 저장하는 함수
    /// </summary>
    public void HangerOn()
    {
        // 말뚝에서 hanger를 가리키는 벡터를 구한다. 단 y축은 무시한다.
        groundVector = hanger.position - transform.position; 
        groundVector = new Vector3(groundVector.x, 0, groundVector.z);
        groundVector = Vector3.Normalize(groundVector);

        // hanger에서 와이어(hanger의 parent)를 가리키는 벡터를 구한다. 단, y축은 무시한다.
        hangerVector = Vector3.Normalize(hanger.position - (hanger.transform.parent.position + Vector3.down * hanger.transform.parent.position.y));
    }

    /// <summary>
    /// 말뚝의 길이를 인자로 받아 말뚝을 초기화하는 함수
    /// </summary>
    /// <param name="pileLengthValue">말뚝 길이</param>
    public void NewPile(int pileLengthValue)
    {
        // 인자로 받은 말뚝 길이만큼 말뚝의 크기를 늘리고, 지정된 초기 위치로 초기화한다.
        transform.localScale = new Vector3(1, 1, pileLengthValue);
        transform.position = new Vector3(transform.position.x - pileLengthValue * 0.9f * Mathf.Cos(30 * Mathf.Deg2Rad), transform.position.y, transform.position.z + pileLengthValue * 0.9f * Mathf.Sin(30 * Mathf.Deg2Rad));
        transform.Rotate(new Vector3(0, 30, 0));
        pileLength = pileLengthValue;

        // 말뚝의 끝부분에 와이어가 연결 될 수 있도록 와이어의 위치를 초기화한다.
        float hangerDistance = Vector3.Distance(transform.GetChild(1).position, hanger.parent.position + Vector3.down * hanger.parent.position.y);
        float wireHeight = Mathf.Sqrt(1024.0f - hangerDistance * hangerDistance); // 삼각함수로 와이어의 길이, 말뚝과 와어이 사이의 길이를 통해 와이어가 있어야 할 높이를 구한다.
        hanger.transform.parent.position += Vector3.down * (hanger.transform.parent.position.y - wireHeight);

        hanger.transform.parent.LookAt(transform.GetChild(1)); // 와이어가 말뚝을 바라보게 회전
        HangerOn(); // 말뚝이 와이어를 바라보는 방향 저장
    }

    /// <summary>
    /// 말뚝이 와이어에 매달릴 때까지 와이어를 당기는 함수
    /// </summary>
    public void PileMove()
    {
        // 와이어 밑부분으로 올때까지 와이어를 당긴다. (기울어진 와이어를 회전시킨다)
        if (Vector3.Distance(hanger.transform.position + Vector3.down * hanger.transform.position.y, hanger.transform.parent.position + Vector3.down * hanger.transform.parent.position.y) > 0.5f)
            hanger.transform.parent.Rotate(hangerVector * -2 * Time.deltaTime);

        // 와이어에 연결된 말뚝이 세워질 때까지 와이어를 위로 이동시킨다.
        if (hanger.transform.parent.transform.position.y < 32 + pileLength * 0.8f) //말뚝을 땅에 세워둘 정도로만 가져온다.
        {
            hanger.transform.parent.transform.position += Vector3.up * 0.4f * Time.deltaTime; //와이어 밑부분으로 올때까지 와이어 높이를 올린다.
        }

        //말뚝 위치를 재설정한다.
        updatePosition = hanger.position + Vector3.down * hanger.position.y - groundVector * Mathf.Sqrt(Mathf.Abs(0.81f * pileLength * pileLength - hanger.position.y * hanger.position.y));
        transform.position = updatePosition;
        transform.LookAt(hanger); //말뚝이 와이어를 바라보도록 한다.
    }
}
