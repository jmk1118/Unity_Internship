using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 오브젝트가 지정된 특정 오브젝트를 바라보게 하는 클래스
/// </summary>
public class Object_Lookat : MonoBehaviour
{
    [SerializeField] Transform lookObject; // 바라봐야 하는 오브젝트
    [SerializeField] bool lookBack = false; // 거꾸로 바라봐야 하는 경우 true로 한다.

    Vector3 flip = new Vector3(180, 0, 0); // 거꾸로 뒤집어지는 벡터

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(lookObject); // 오브젝트가 lookObject를 바라보게 한다.

        // 만약 거꾸로 바라봐야한다면, x축을 기준으로 180도 회전시킨다.
        if(lookBack)
        {
            transform.Rotate(flip);
        }     
    }
}
