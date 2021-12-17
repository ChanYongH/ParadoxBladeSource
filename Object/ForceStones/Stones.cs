using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stones : MonoBehaviour
{
    protected Player player;
    public GameObject rightHandCamera;// ��鸮�� ȿ���� ������ ������ ������
    public GameObject stayHand;// ��ȭ���� ��� Ʈ���� ���� �� ������ ��ƼŬ
    public GameObject destoryStone;// ��ȭ���� �μ��� �� ������ ��ƼŬ
    public string itemName; // ��ȭ�� �̸�(ĵ������ ���)
    public string itemEffect; // ��ȭ�� ȿ��(ĵ������ ���)
    //UIó��
    public GameObject itemInfoCanvas;
    public GameObject selectCanvas;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI infoText2;

    public bool enter = true; // �Ⱦ��µ�
    public bool stoneEnter = false; //�Ⱦ��µ�
    public int attrib = 0; // �Ӽ��� �°� ��ȭ�ǰ� �ϱ����� ����(Player.attrib�� �����ͼ� ��ġ��Ŵ)
    public float _time; // Ÿ�̸�
    public bool timer = false; // Ÿ�̸�
    public static int stoneLevel = 0; // ���������� �ö󰡸� ���淹���� �ö󰡼� �ö󰡴� ���� ������
    public static bool isStoneDestory = false; // ��ȭ���� �ı��Ǹ� ���� ���������� �� ���ְ� ó�� �ϱ����� ���
    public bool grabOn = false; // ������ ��ü���� ó�� �� �� �ֵ��� ó��(����ƽ �׷��� �� ���� �۵��ؼ� ��������) 
    public TextMeshProUGUI getForce; // ���� ������������ ���� �ɷ�
    public TextMeshProUGUI forceListText; // ���� �ɷµ�
    public static List<string> forceListString = new List<string>(); // ��� �ſ��� �� �� �ְ� �ϱ� ���ؼ� staticó��
    //��ü �ϳ��ϳ��� �׷� ����
    //public int setStone = 0;
    public GameObject store; // ������ ������ �� ó���ϱ� ���� ���
    public bool onStore = false; // ������ ���Դ��� �ȳ��Դ��� Ȯ��

    //�ʱ�ȭ
    public void OnEnable()
    {
        isStoneDestory = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        stayHand = player.transform.GetChild(0).GetChild(0).GetChild(6).GetChild(0).gameObject;
        destoryStone = player.transform.GetChild(0).GetChild(0).GetChild(6).GetChild(1).gameObject;
        itemInfoCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        infoText = itemInfoCanvas.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        infoText2 = itemInfoCanvas.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        selectCanvas = GameObject.FindGameObjectWithTag("SelectCanvas");
        getForce = selectCanvas.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        forceListText = selectCanvas.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();
        store = GameObject.FindGameObjectWithTag("Store");
    }
    //�����ð����� Ʈ���� ������ �ı�, ���� ���������� ���� ĵ����
    public void Update()
    {
        if (OVRGrabber.grab && grabOn)
            StoneTrigger();
        //�׸� ���¿��� ���� �ð����� Ʈ���Ÿ� ������ ������ �߻� 
        if (!timer)
            _time = Time.time;
        Debug.Log("�ð�" + _time);
        if (Time.time > _time)
        {
            Debug.Log("�ð� ���");
            AddAbility();
            
        }
        if (isStoneDestory)
        {
            gameObject.SetActive(false);
            if(!onStore)
                selectCanvas.transform.GetChild(0).gameObject.SetActive(true);
            else
                selectCanvas.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    //�ð� ����
    public void DestoryTime(float time, bool state)
    {
        timer = state;
        _time = Time.time + time;
        //StartCoroutine(CameraShakeCo());
        stayHand.SetActive(state);
    }
    //���X
    IEnumerator CameraShakeCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }
    //�����ð����� Ʈ���� ������ �ı� ���������� ó��
    public void StoneTrigger()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) //�ڵ� �׸� ����//if(m_grabbedObj.tag != "Sword")
        {
            AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/StoneStay"));
            Debug.Log("Ʈ���� �ٿ�");
            //3�� ���� Ʈ���Ÿ� ������ ������
            DestoryTime(3, true);
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("Ʈ���� ��");
            DestoryTime(0, false);
        }
        else
        {
            Debug.Log("�� ���ô±���");
        }
    }
    //�ı����� �� �����ϰ� ����Ǵ� �̺�Ʈ��(�ڽĿ��� base.AddAbility�� ���)
    public virtual void AddAbility()
    {
        AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/Stonedestroy"));
        AudioManager.PlaySfx(null);
        stayHand.SetActive(false);
        destoryStone.SetActive(true);
        //��ȭ���
        getForce.text = itemEffect;
        forceListString.Add(itemEffect);
        for (int i = 0; i < forceListString.Count; i++)
            forceListText.text += forceListString[i] + System.Environment.NewLine;

        FileManager.AutoSave(Player.inventory, player.money,
                        (int)player.MaxHp, (int)player.Hp,
                        (int)player.Att, ++FileManager.loadStageCount);
        //Debug.Log("�÷��̾� �κ��丮" + player.inventory);
        gameObject.SetActive(false);
        isStoneDestory = true;
        //���� Ȯ���� ��������
        //int ranNum;
        //ranNum = Random.Range(0, 1);
        //if (ranNum % 2 == 0)
        //if(ranNum == 0)
            //OnStore();
    }
    //������ ����
    public void OnStore()
    {
        onStore = true;
        store.transform.GetChild(0).gameObject.SetActive(true);
    }
    //��ȭ�� ���� �����ֱ�
    public void ShowInfo()
    {
        itemInfoCanvas.transform.GetChild(0).gameObject.SetActive(true);
        infoText.text = itemName;
        infoText2.text = itemEffect; 
    }

    //��ü �ϳ��ϳ��� �׷� �Ұ��� ����
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<OVRGrabber>() != null)
            grabOn = true;
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<OVRGrabber>() != null)
            grabOn = false;
    }

}
