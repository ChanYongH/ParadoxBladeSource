using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCol : MonoBehaviour
{
    //Enemy enemy;
    //���X
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

    //�÷��̾� �ڿ� �����Ǵ� Ʈ����(��� ����� �ޱ� ���� Ʈ������)
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
    //���X
    IEnumerator ComboTriggerCo()
    {
        Debug.Log("�޽� ��� ��");
        Enemy.comboCol = true;
        yield return new WaitForSeconds(1);
        Enemy.comboCol = false;
        Debug.Log("�޽� ��� ����");
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            StartCoroutine(SwordPartCo(other)); // �޺� �ݶ��̴�(����)�� Ȱ��ȭ �ϰ� ����
            //other.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    //��ƼŬ ó��
    IEnumerator SwordPartCo(Collider swordPart)
    {
        yield return new WaitForSeconds(0.4f);
        swordPart.transform.GetChild(1).gameObject.SetActive(false);
    }
}
