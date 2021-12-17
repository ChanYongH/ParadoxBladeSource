using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemTemp : MonoBehaviour
{
    public string itemName; // ������ �̸�(�ڽĿ��� ���� ��� ��)
    public int kind = 0; // �������� ����(�������� �����ϰų� ��� �� �� ���, �κ��丮�� ó��)
    // ������ UIó��
    public GameObject infomation = null;  
    public TextMeshProUGUI infoText = null; 
    //public List<GameObject> Effects;
    public bool itemUse = true; // ������ ��� ���ð�
    public float activeTime; // ������ ���ӽð�
    public GameObject itemEffect = null; // ������ ��ƼŬ, ������Ʈ

    /// /////////////////////////////////
    /// ���⼭ ���� Han's �ڵ�
    public GameObject player;
    public GameObject enemy;

    //������
    private void Awake()
    {
    }
    //�÷��̾�� ���ʹ̸� �ҷ���
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    /// ������� Han's �ڵ�
    /////////////////////////////////////
    //������ ���
    public virtual void ItemEffect()
    {
    }
    //������ ���� ����
    public virtual void ShowInfo(bool state)
    { 
    }
    //������ ���� ���ð� �ڷ�ƾ
    public IEnumerator ItemTimeCo()
    {
        itemUse = false;
        yield return new WaitForSeconds(5);
        itemUse = true;
    }
}
