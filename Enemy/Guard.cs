using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    Player player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    //플레이어 검이 방어막을 공격하면 스택 쌓임
    //- 플레이어 검의 위치를 받아서 그 위치에 파티클을 발생시킴
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sword")
        {
            OVRGrabber.grab = false;
            transform.GetChild(0).gameObject.SetActive(true); // 몬스터 방어막을 때리면 파티클 발생
            transform.GetChild(0).gameObject.transform.position = other.transform.position; // 맞은 위치에 파티클 터지게
            AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/GuardAttack"));
            player.enemyGuardAttack++;
            StartCoroutine(PartOffCo());
        }
    }
    //파티클 setfalse
    IEnumerator PartOffCo()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
