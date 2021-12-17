using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private bool isLoad = false; // �ҷ����� ����
    public GameObject playerSword; // ��ƼŬ �� ���׸��� ����
    public static int[] inventory = new int[6] { 0, 0, 0, 0, 0, 0 }; // �κ��丮 �������� ó��
    public float[] attirbAtt = new float[9]; // �󼺿� ���� ����� ó��
    public static float[,] itemForce = new float[3,3]; // nomal ,��, ȭ��, emp
    public List<GameObject> uiPlayerHp; // PlayerHpUIó��
    public GameObject attackCol = null; // �ֵѷ��� �� Ȱ��ȭ(Player Attack)
    //public GameObject comboCol = null;
    public int money;
    public Queue<int> setAttribQueue = new Queue<int>(); // �Ӽ� ����
    //�޺�
    public Transform comboStart; // �� ���°Ű���
    public float comboStartSpot; // �� ���°Ű���

    public int enemyGuardAttack = 0; // ���� ���带 ������ ++
    public List<GameObject> enemyGuardState; // enemyGuardAttack -> UI

    public bool isImmortality = false; // ���� ���� ������ ��� �ϸ�
    public GameObject tazdingo; // ������ ���׸���

    public bool[] soundDelay = new bool[3]; // ���� �� ���� �鸮�� ó��
    //int attKind = 5;
    //public int defKind;
    public string playerName = "";

    //�ʱ�ȭ
    public virtual void Awake()
    {
        //maxHp = hp;
        //att = 10;
        uiPlayerHp = new List<GameObject>();
        //for (int i = 0; i < inventory.Length; i++) // �κ��丮 �ʱ�ȭ
            //inventory[i] = 0;
        for (int i = 0; i < 4; i++)
            setAttribQueue.Enqueue(i);
        for (int i = 0; i < GameObject.Find("PlayerHp").transform.childCount; i++)
            uiPlayerHp.Add(GameObject.Find("PlayerHp").transform.GetChild(i).gameObject);
        for (int i = 0; i < GameObject.Find("EnemyShieldAttack").transform.childCount; i++)
            enemyGuardState.Add(GameObject.Find("EnemyShieldAttack").transform.GetChild(i).gameObject);

    }

    //�ҷ�����
    //PlayerUI���� �ǽð� ó��
    //PlayerAttack���� ó��(���ν�Ƽ�� ���)

    public void Update()
    {
        if (isLoad == false)
        {
            FileManager.OnLoad();
            isLoad = true;
            //inventory = FileManager.loadInventory;
            money = FileManager.loadMoney;
            MaxHp = FileManager.loadMaxHp;
            Hp = FileManager.loadHp;
            Att = FileManager.loadAtt;
            //Debug.Log(Att);
            Debug.Log("inventory: " + inventory);
            Debug.Log("money: " + money);
            Debug.Log("maxHp: " + MaxHp);
            Debug.Log("Hp: " + Hp);
            Debug.Log("Att: " + Att);
        }
        if (isImmortality)
            tazdingo.SetActive(true);
        else
            tazdingo.SetActive(false);

        for (int i = 0; i < inventory.Length; i++)
            Debug.Log("�κ��丮 " + i + " " + inventory[i]);

        if (enemyGuardAttack > 2) // ���� �ǵ带 3�� ���� �� Hp�� ����
        {
            Hp--;
            enemyGuardAttack = 0;
        }
        for (int i = 0; i < enemyGuardState.Count; i++)
            enemyGuardState[i].SetActive(false);
        for (int i = 0; i < enemyGuardAttack; i++)
            enemyGuardState[i].SetActive(true);
        //PlayerHpó��(UI)
        for (int i = 0; i < uiPlayerHp.Count; i++)
            uiPlayerHp[i].SetActive(false);
        for (int i = 0; i < Hp; i++)
            uiPlayerHp[i].SetActive(true);
        if (setAttrib >= 1)
            playerSword.transform.GetChild(0).gameObject.SetActive(true); // ��ƼŬ
        else
            playerSword.transform.GetChild(0).gameObject.SetActive(false);

        if (OVRInput.GetDown(OVRInput.Button.One))
            SetAttribute();
        //�޴� ���� 3�̻��̰� ���� �����ϰ������� �ߵ�
        bool basicAttack = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RHand).magnitude >= 3f && 
            ReturnPlace.sword;

        if (basicAttack)
        {
            Debug.Log("���ӵ� : " + OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RHand).magnitude);
            //Debug.Log("�ǵ� ����!");
            //�Ӽ��� ���� �ٸ� ���� ���
            if (setAttrib == 1 && soundDelay[0])
            {
                AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/FireAttack"));
                soundDelay[0] = false;
            }
            else if (setAttrib == 2 && soundDelay[1])
            {
                AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/SangAttack"));
                soundDelay[1] = false;
            }
            else if (setAttrib == 3 && soundDelay[2])
            {
                AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/EmpAttack"));
                soundDelay[2] = false;
            }
            
            if (attackCol != null)
            {
                //���� �ݶ��̴� Ȱ��ȭ
                attackCol.SetActive(true);
                //��ƼŬ
                attackCol.transform.GetChild(0).gameObject.SetActive(true);
            }
            Debug.Log("�⺻ ����2");
        }
        else
        {
            for (int i = 0; i < soundDelay.Length; i++)
                soundDelay[i] = true;
            //���� �ݶ��̴� ��Ȱ��ȭ
            if (attackCol != null)
            {
                attackCol.SetActive(false);
                attackCol.transform.GetChild(0).gameObject.SetActive(true);
            }
        }


        //Debug.Log("�ǵ� �׽�Ʈ : " + ReturnPlace.sword);
        Debug.Log("�� �Ӽ� ��ȭ : " + itemForce[0,0]);
        Debug.Log("ȭ�� �Ӽ� ��ȭ : " + itemForce[0, 1]);
        Debug.Log("emp �Ӽ� ��ȭ : " + itemForce[0, 2]);
    }
    //����(�� �ѱ�)
    public override void Dead([CallerMemberName] string caller = "")
    {
        if (isImmortality)
            Hp = 1;
        else
        //Debug.Log("���׾�?");
        {
            SceneManager.LoadScene("Player_Lose_Ending");
            AudioManager.ChangeBgm(Resources.Load<AudioClip>("Sound/Music/GameOver"));
        }
        //SceneManager.LoadScene(10);
    }
    //�̰� �Ⱦ�
    public override void OnHit(int playerAttrib, float[,] item, float damage)
    {
        Hp -= damage;
    }
    public override void OnCrisis()
    {
        Debug.Log("�÷��̾� ȭ��");
        AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/Heartbeat"));
    }

    //�Ӽ� ����(��ƼŬ, ���׸���, ���� ����)
    public void SetAttribute()
    {
        setAttrib = setAttribQueue.Dequeue();
        //���ҽ��� �ִ� ���׸���� ����(����)�� ����
        playerSword.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Sword7/" + setAttrib);

        setAttribQueue.Enqueue(setAttrib); // 
        Debug.Log("ť : " + setAttribQueue.Peek());

        //�ٲ� �� ���� ���带 ����ϰ� ��ƼŬ�� ������ �ٲ���
        switch(setAttrib)
        {
            case 1:
                AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/SetAttributefire"));
                playerSword.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = Color.red;
                break;
            case 2:
                AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/SetAttribSang"));
                playerSword.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = Color.green;
                break;
            case 3:
                AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/SetAttribEMP"));
                playerSword.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = Color.blue;
                break;
        }
    }

}
