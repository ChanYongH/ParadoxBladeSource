using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHpAdd : Stones
{
    private void Awake()
    {
        itemName = "체력 강화석";
        itemEffect = "최대체력 증가";
    }
    public override void AddAbility()
    {
        //gameObject.SetActive(false);
        player.MaxHp += stoneLevel+1;
        Debug.Log("최대체력사용");
        base.AddAbility();
    }
}
