using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public float att;
    public int setAttrib = 0;
    public bool isDead;

    //han이 virtual을 추가함. 금세 삭제함
    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (Hp <= 0)
                Dead();
            else if (Hp <= 1)
                OnCrisis();
            Debug.Log(name + "의 현재 체력" + Hp);
        }
    }
    public float MaxHp
    {
        get
        {
            return maxHp;
        }
        set
        {
            maxHp = value;
            Debug.Log(name + "의 최대 체력" + MaxHp);
        }
    }
    public float Att
    {
        get
        {
            return att;
        }
        set
        {
            att = value;
            Debug.Log(name + "의 공격력" + Att);
        }
    }
    public bool OnAttack(bool state)
    {
        if (state)
            return true;
        else
            return false;
    }
    public virtual void Dead(string caller = "") {}
    public virtual void OnHit(int playerAttrib, float[,] item, float damage) { }
    public virtual void OnCrisis(){}
    //버튼을 누르면 순서대로 바뀜

}
