using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    // 목표가 될 트랜스폼 컴퍼넌트
    public Transform target;

    private void Update()
    {
        // 목표의 위치와 카메라의 위치를 일치시킨다.
        transform.position = target.position; 
    }
}
