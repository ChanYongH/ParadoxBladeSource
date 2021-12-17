using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCol : MonoBehaviour
{
    //Enemy enemy;
    //사용X
    private void OnEnable()
    {
        //StartCoroutine(ObjOffCo());
        //enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }
    IEnumerator ObjOffCo()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    //플레이어 뒤에 생성되는 트리거(찌르기 모션을 받기 위한 트리거임)
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            other.transform.GetChild(1).gameObject.SetActive(true);
            Enemy.comboCol = true;
            AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/ComboCol"));
            //StartCoroutine(ComboTriggerCo());
        }
    }
    //사용X
    IEnumerator ComboTriggerCo()
    {
        Debug.Log("콤시 찌르기 온");
        Enemy.comboCol = true;
        yield return new WaitForSeconds(1);
        Enemy.comboCol = false;
        Debug.Log("콤시 찌르기 오프");
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            StartCoroutine(SwordPartCo(other)); // 콤보 콜라이더(뒤쪽)을 활성화 하고 꺼줌
            //other.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    //파티클 처리
    IEnumerator SwordPartCo(Collider swordPart)
    {
        yield return new WaitForSeconds(0.4f);
        swordPart.transform.GetChild(1).gameObject.SetActive(false);
    }
}
