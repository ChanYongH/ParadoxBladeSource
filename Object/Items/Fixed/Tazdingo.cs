using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tazdingo : ItemTemp
{
    //////////////////////////////////
    //han's �ڵ�
    //���ӽð�
    IEnumerator TazCo()
    {
        player.transform.GetComponent<Player>().isImmortality = true;
        yield return new WaitForSeconds(activeTime);     //5�ʰ� ����
        player.transform.GetComponent<Player>().isImmortality = false;
    }

    //////////////////////////////////
    void Awake()
    {
        //itemName = "";          //��?��
        //kind = 1;               //��?��
        itemUse = true; //���� �����ָ� false������
    }
    public override void ItemEffect()
    {
        if (itemUse)
        {
            Debug.Log("������ �һ���ġ�� ����ϴ�");
            StartCoroutine(ItemTimeCo());

            //han's �ڵ�
            StartCoroutine(TazCo());
            //player.transform.GetComponent<Player>().inventory[2]--;
            Player.inventory[2]--;
        }
    }
}
