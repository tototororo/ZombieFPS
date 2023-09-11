using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    // 메인 카메라 트랜스폼
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        //자기 자신의 방향을 카메라의 방향과 일치시킨다.
        transform.forward = target.forward;
    }
}
