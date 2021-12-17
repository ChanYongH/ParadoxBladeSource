using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    Player player;
    bool temp = true;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //�ֵѷ��� ��
    //���� ����
    //UIó��
    private void OnTriggerEnter(Collider other)
    {
        //���� ����
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (enemy.Shield > 0)
            {
                enemy.OnHit(player.setAttrib, Player.itemForce, player.Att);
            }
        }
        if (Enemy.comboCol) // �޺� Ʈ����(����)�� Ȱ��ȭ �Ǹ� on
        {
            //enemy.Hp -= 10;
            Debug.Log("�޽� ����� ����");
            if (other.CompareTag("Combo"))
            {
                AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/ComboSuccess"));
                StartCoroutine(SpearTimeCo());
                other.gameObject.SetActive(false);
                //StartCoroutine(FalseTimeCo(other));
                other.transform.parent.parent.parent.GetComponent<Enemy>().Hp -= 1;
                Enemy.comboCol = false;
            }
        }

        IEnumerator SpearTimeCo()
        {
            transform.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        //UI����ó��
        if (other.CompareTag("UIStart") && temp)
        {
            player.Hp = 3;
            FileManager.AutoSave(FileManager.loadInventory, 0, 3, 3, 10, 0);
            StageManager.ChangeStage();
            //FileManager.AutoSave(FileManager.loadInventory, 0, FileManager.loadMaxHp, FileManager.loadHp, FileManager.loadAtt, 0);
            temp = false;
        }
        if (other.CompareTag("GameQuit"))
            Application.Quit();
        if (other.CompareTag("UIVolume"))
        {
            other.transform.GetChild(0).gameObject.SetActive(true);
            
        }
        if (other.CompareTag("UIVolumeUp"))
            UiManager.PushVolumeUp();
        if (other.CompareTag("UIVolumeDown"))
            UiManager.PushVolumeDown();


        if (other.CompareTag("PlayerDeadGoMenu"))
        {
            SceneManager.LoadScene(0);
        }

    }
}
