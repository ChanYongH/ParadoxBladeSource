using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SangAddForce : ItemTemp
{
    //////////////////////////////////
    //han's �ڵ�. AF�� �����ϰ� ���
    //�ٸ� �ڵ忡���� isAddAF�� �ʿ信 �°� ������ ��

    IEnumerator AddForceCo()
    {
        enemy.GetComponent<Enemy>().isAddBH = true;
        yield return new WaitForSeconds(activeTime);
        enemy.GetComponent<Enemy>().isAddBH = false;
    }
    //////////////////////////////////
    
    void Awake()
    {
        //itemName = "";          //��?��
        //kind = 1;               //��?��
        itemUse = true;         //���� �����ָ� false������
    }
    public override void ItemEffect()
    {
        if (itemUse)
        {
            Debug.Log("�Ӽ� ���� �������� ����ϴ�");
            StartCoroutine(ItemTimeCo());

            ////////////////////////////////////////////////
            //han's �ڵ�. �κ��丮 ���Ҵ� ��ȭ �ϸ鼭 ������ ��
            StartCoroutine(AddForceCo());
            //player.transform.GetComponent<Player>().inventory[4]--;
            Player.inventory[4]--;
            ////////////////////////////////////////////////
        }
    }
}
