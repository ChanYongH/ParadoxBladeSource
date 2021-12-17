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
    //�÷��̾� ���� ���� �����ϸ� ���� ����
    //- �÷��̾� ���� ��ġ�� �޾Ƽ� �� ��ġ�� ��ƼŬ�� �߻���Ŵ
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sword")
        {
            OVRGrabber.grab = false;
            transform.GetChild(0).gameObject.SetActive(true); // ���� ���� ������ ��ƼŬ �߻�
            transform.GetChild(0).gameObject.transform.position = other.transform.position; // ���� ��ġ�� ��ƼŬ ������
            AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/GuardAttack"));
            player.enemyGuardAttack++;
            StartCoroutine(PartOffCo());
        }
    }
    //��ƼŬ setfalse
    IEnumerator PartOffCo()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
