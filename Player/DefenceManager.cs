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

    //���X
    void DefenceState(bool state)
    {
        if (state)
            Debug.Log("��� ����");
        else
            Debug.Log("��� ����");
        shield.defence = true;
    }
    //���X
    void OnEnable()
    {
        shield.defence = false;
    }
    //��Ȱ��ȭ ���� �� ���� ����� ���ư�
    void OnDisable()
    {
        mesh.material.color = Color.white;
    }
}
