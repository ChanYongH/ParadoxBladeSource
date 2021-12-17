using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tazdingo : ItemTemp
{
    //////////////////////////////////
    //han's 코드
    //지속시간
    IEnumerator TazCo()
    {
        player.transform.GetComponent<Player>().isImmortality = true;
        yield return new WaitForSeconds(activeTime);     //5초간 무적
        player.transform.GetComponent<Player>().isImmortality = false;
    }

    //////////////////////////////////
    void Awake()
    {
        //itemName = "";          //몰?루
        //kind = 1;               //몰?루
        itemUse = true; //정의 안해주면 false상태임
    }
    public override void ItemEffect()
    {
        if (itemUse)
        {
            Debug.Log("아이템 불사장치를 썼습니다");
            StartCoroutine(ItemTimeCo());

            //han's 코드
            StartCoroutine(TazCo());
            //player.transform.GetComponent<Player>().inventory[2]--;
            Player.inventory[2]--;
        }
    }
}
