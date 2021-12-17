using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPAddForce : ItemTemp
{
    //////////////////////////////////
    //han's 코드. AF만 상정하고 기술
    //다른 코드에서는 isAddAF를 필요에 맞게 수정할 것

    IEnumerator AddForceCo()
    {
        enemy.GetComponent<Enemy>().isAddCY = true;
        yield return new WaitForSeconds(activeTime);
        enemy.GetComponent<Enemy>().isAddCY = false;
    }
    //////////////////////////////////
    
    void Awake()
    {
        //itemName = "";          //몰?루
        //kind = 1;               //몰?루
        itemUse = true;         //정의 안해주면 false상태임
    }
    public override void ItemEffect()
    {
        if (itemUse)
        {
            Debug.Log("속성 공격 아이템을 썼습니다");
            StartCoroutine(ItemTimeCo());

            ////////////////////////////////////////////////
            //han's 코드. 인벤토리 감소는 분화 하면서 수정할 것
            StartCoroutine(AddForceCo());
            //player.transform.GetComponent<Player>().inventory[5]--;
            Player.inventory[5]--;
            ////////////////////////////////////////////////
        }
    }
}
