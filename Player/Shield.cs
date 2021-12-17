using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    bool enter = true;
    public GameObject stopObj;
    public GameObject trackingObj;
    public Quaternion shieldRotate;
    public static bool selectItem = false; // �� ���� ���� �ǰ�
    public bool defence = false;
    Player player;

    public GameObject[] showItem = new GameObject[6] { null, null, null, null, null, null};
    //public GameObject[] setItemObj = new GameObject[2];
    //public GameObject[] itemUseTrigger = new GameObject[2];
    public int setItem; // ������ ����
    public GameObject canvas = null; // ������ â
    public GameObject finger = null ; // ������ â�� ������ �� ����
    public bool useItem = false; // �ֵθ��� ��� �ϰ� ��

    ////////////////////////////////////////////////////////////////////////
    ///Han's �ڵ�
    public bool isHelmet = false;
    public GameObject helmetEffect = null;

    //public Transform nowPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    //������ ���, ��� ����,���� ó��
    private void OnTriggerEnter(Collider other)
    {
        ItemTemp item;
        item = other.GetComponent<ItemTemp>();

        if (other.CompareTag("UseItem"))
            item.ItemEffect();
        if (other.CompareTag("EnemyAttack") && !defence && isHelmet == false)//isHelmet�� ���ǿ� �߰�
        {
            player.Hp -= 1;
            AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/PlayerHit"));
        }
        if(other.CompareTag("EnemyAttack") && defence) //��� ����
        {
            AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/GuardSuccess"));
        }


            //�߰� �ڵ�. ���� ���� Ÿ�̹��̸�, �� ����������, ����� ���� �ִ� ���
        if (other.CompareTag("EnemyAttack") && !defence && isHelmet == true)       //isHelmet�� ���ǿ� �߰�
            isHelmet = false;
        if(other.CompareTag("ShieldZone"))
            AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/PlayerGuradStay"));
    }
    //��� �ϰ� ���� ��
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ShieldZone"))
        {
            defence = true;
            other.GetComponent<MeshRenderer>().material.color = Color.green;
            //AudioManager.ChangeBgm(Resources.Load<AudioClip>("Sound/SFX/PlayerGuradStay"));
        }
    }
    //�� ���� ��
    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ShieldZone"))
        {
            other.GetComponent<MeshRenderer>().material.color = Color.white;
            defence = false;
            //StartCoroutine(DefenceOff());
        }
    }

    IEnumerator DefenceOff()
    {
        yield return new WaitForSeconds(0.5f);
        defence = false;
    }
    //if(other.GetComponent<ItemTemp>() != null)
    //other.GetComponent<ItemTemp>().ShowInfo(false);
    //��ư�� ������ �� ������ �κ��丮 ����
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/ItemOpen"));
            Debug.Log("������ ����");
            Time.timeScale = 0.1f;
            ShowInventory(true);
            finger.SetActive(true);
            transform.SetParent(stopObj.transform, true);
        }
        else if(OVRInput.GetUp(OVRInput.Button.Three))
        {
            Time.timeScale = 1f;
            selectItem = false;
            ShowInventory(false);
            finger.SetActive(false);
            transform.SetParent(trackingObj.transform, false);
            transform.position = trackingObj.transform.position;
            transform.localRotation = shieldRotate;
        }
        if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LHand).magnitude >= 2.5f)
            useItem = true;
        else
            useItem = false;
        if (helmetEffect != null)
        {
            if (isHelmet)
                helmetEffect.SetActive(true); // ���� ��Ŀ�� ����
            else
                helmetEffect.SetActive(false);
        }

    }
    void ShowInventory(bool state)
    {
        canvas.SetActive(state);
        if (state)
        {
            for (int i = 0; i < showItem.Length; i++)
            {
                if (Player.inventory[i] > 0)
                    showItem[i].SetActive(true);
                else
                    showItem[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < showItem.Length; i++)
                showItem[i].SetActive(false);
        }
    }
}
