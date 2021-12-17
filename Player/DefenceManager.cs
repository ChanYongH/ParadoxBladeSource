using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceManager : MonoBehaviour
{
    Shield shield;
    MeshRenderer mesh;
    void Awake()
    {
        shield = GameObject.FindGameObjectWithTag("Shield").GetComponent<Shield>();
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    //사용X
    void DefenceState(bool state)
    {
        if (state)
            Debug.Log("방어 성공");
        else
            Debug.Log("방어 실패");
        shield.defence = true;
    }
    //사용X
    void OnEnable()
    {
        shield.defence = false;
    }
    //비활성화 됐을 때 원래 색깔로 돌아감
    void OnDisable()
    {
        mesh.material.color = Color.white;
    }
}
