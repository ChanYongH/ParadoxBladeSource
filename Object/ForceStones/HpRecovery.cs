using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpRecovery : Stones
{
    private void Awake()
    {
        itemName = "ü�� ȸ����";
        itemEffect = "Hp ��� ȸ��";
    }
    public override void AddAbility()
    {
        player.Hp = player.MaxHp;
        base.AddAbility();
        //player.Hp = player.MaxHp;
        Debug.Log("ȸ�����");
    }
}
