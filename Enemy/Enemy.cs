using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using TMPro;
public class Enemy : Character
{
    Player player;
    Animator ani;
    private int lv;
    private float nowTime = 0;
    private float nowTime2= 0;
    public float repeatTime = 5; // 
    public float attackTime = 0; // ���� ���ð�
    private bool attSuccess = false; //��� ���ϴµ�
    public float shield;
    public float maxShield;
    public int defChoiceNum;        //�÷��̾��� ���� ����
    public int attChoiceNum;        //������ ���� ����
    public bool attPattern = false;        //������ ���� ���� ���� ����
    public List<int> comboRandNum; // �޺� �ݶ��̴� ���� ������ �ϱ����� ���
    public List<int> stoneRandNum; // ��ȭ�� ���� ������ �ϱ����� ���
    public List<GameObject> uiEnemyHp; 
    public Image enemyShield;
    public static int comboCount = 3; // �޺� �ݶ��̴� Ȱ��ȭ ��
    //public GameObject[] attackReady = new GameObject[4]; // getchild�� �ذ�
    public GameObject stickMotion; // �޺� �ݶ��̴�
    public static bool comboCol = false; // �޺� ó��
    public GameObject enemyHit; // �¾��� �� ��ƼŬ
    public GameObject uiComboTime; // �޺� �ð��� �˷���
    public GameObject guardManager; // ���� ��� ����
    bool enemyAttackState = true; // ���������� ����ɷ��� �̰� true�� �Ǿ� ��
    bool hitTime = true; // �� ���� ���� �԰�
    public TextMeshProUGUI getMoneyText;

    //�������� 
    int dropMoney = 0; // ���� ��
    private float alphaColor = 1; 
    public GameObject[] stones = new GameObject[6]; // ��ȭ�� ���
    public GameObject table; // ��ȭ�� ���̺�(�̰� Ȱ��ȭ �Ǿ� ��ȭ�� ȹ�� ����)
    public GameObject store; // ����
    public Text MoneyText = null;
    //han's
    //������ ó���� ���� ���
    public bool isAddAF = false;        //�� ��ȭ ����
    public bool isAddBH = false;        //��ȭ�� ��ȭ ����
    public bool isAddCY = false;        //EMP ��ȭ ����
    public GameObject[] addForceItemEffect = new GameObject[3];

    public int DefChoiceNum
    {
        get { return defChoiceNum; }
        set
        {
            defChoiceNum = value;
            //Debug.Log(defChoiceNum);
        }
    }
    public int AttChoiceNum
    {
        get { return attChoiceNum; }
        set
        {
            attChoiceNum = value;
        }
    }
    //����Ÿ���� �����ߴ�
    public bool AttPattern
    {
        get { return attPattern; }
        set { attPattern = value; }
    }
    public float Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
            Debug.Log("�ǵ� �� : " + shield);
            if (Shield <= 0)
            {
                ComboSystem();
                Debug.Log("�ǵ�");
            }
        }
    }
    private void Awake()
    {
        int randNum = Random.Range(0,2);
        setAttrib = randNum;
        hp = maxHp;
        shield = maxShield;
        ani = GetComponent<Animator>();
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        for (int i = 0; i < transform.GetChild(10).GetChild(0).childCount; i++)
            uiEnemyHp.Add(transform.GetChild(10).GetChild(0).GetChild(i).gameObject);
        transform.GetChild(10).GetChild(1).GetChild(setAttrib).gameObject.SetActive(true);
        getMoneyText = GameObject.FindGameObjectWithTag("SelectCanvas").transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();


    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        stickMotion = player.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).gameObject;
        store = GameObject.FindGameObjectWithTag("Store").transform.GetChild(0).gameObject;

    }
    //���� ��������� �ǽð�ó��
    //���� ��������(n��)
    void Update()
    {
        enemyShield.fillAmount = shield/(maxShield);
        for (int i = 0; i < uiEnemyHp.Count; i++)
            uiEnemyHp[i].SetActive(false);
        for(int i = 0; i < Hp; i++)
            uiEnemyHp[i].SetActive(true);

        //������ ����Ʈ ó�� ���� �ʿ� 
        if (isAddAF)
            addForceItemEffect[0].SetActive(true);
        else
            addForceItemEffect[0].SetActive(false);

        if (isAddBH)
            addForceItemEffect[1].SetActive(true);
        else
            addForceItemEffect[1].SetActive(false);

        if (isAddCY)
            addForceItemEffect[2].SetActive(true);
        else
            addForceItemEffect[2].SetActive(false);

        if (Shield > 0)
        {
            guardManager.SetActive(true);
            Debug.Log("����Ŵ�����");
        }
        else
        {
            guardManager.SetActive(false);
            Debug.Log("����Ŵ�������");
        }


        if (AttPattern == false)
        {
            AttPattern = true;
            //OnDefence();//�׸���
            OnAttack();//�׸���
        }
        //Ÿ�̸� �ϳ� �� �־ ���� �ֱ� ����
        //5�� �� ����
        if (enemyAttackState)
        {
            if (nowTime >= attackTime) //
            {
                transform.GetChild(9).GetChild(AttChoiceNum).gameObject.SetActive(true); // ���� ���� �ݶ��̴� Ȱ��ȭ(�÷��̾� ��� Ȱ��ȭ)
                transform.GetChild(9).GetChild(4).gameObject.SetActive(false); // ���������� �����ϴ� �ݶ��̴�
                nowTime2 += Time.deltaTime;
                EnemyAttack(2); // 2�� �ڿ� ����
                attackTime = 2;//Random.Range(2, 5);
            }
        }
        nowTime += Time.deltaTime;
    }
    //�� ����ó��
    public void EnemyAttack(float attTime)
    {
        if (nowTime2 >= attTime)
        {
            transform.GetChild(9).GetChild(AttChoiceNum).gameObject.SetActive(false);
            transform.GetChild(9).GetChild(4).gameObject.SetActive(true);//�����ݶ��̴� Ȱ��ȭ
            Debug.Log("���ݴ��ð������� ��������" + attackTime);

            //�ִϸ��̼� ����
            if (AttChoiceNum == 0)
                GetComponent<Animator>().SetTrigger("Right_Left");
            if (AttChoiceNum == 1)
                GetComponent<Animator>().SetTrigger("Down_Up");
            if (AttChoiceNum == 2)
                GetComponent<Animator>().SetTrigger("Up_Down");
            if (AttChoiceNum == 3)
                GetComponent<Animator>().SetTrigger("Left_Right");
            AttPattern = false;
            //GetComponent<Animator>().SetTrigger("Return");
            nowTime = 0;
            nowTime2 = 0;
            //repeatTime = 2;//Random.Range(5, 7);
        }
    }
    //�Ⱦ�
    public virtual void OnDefence()
    {
        DefChoiceNum = Random.Range(0, 3);
        transform.GetChild(0).GetChild(DefChoiceNum).gameObject.SetActive(true);
    }
    //����
    public virtual void OnAttack()
    {
        AttChoiceNum = Random.Range(0, 3);
        //transform.GetChild(1).GetChild(AttChoiceNum).gameObject.SetActive(true);
    }
    //�ǵ尡 ����
    //�Ӽ� + ��ȭ�� + ������ ��� ó�� 
    public override void OnHit(int playerAttrib, float[,] item, float damage)
    {
        AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/EnemyHit"));
        nowTime = 0;
        for (int i = 0; i < transform.GetChild(9).childCount; i++)
            transform.GetChild(9).GetChild(i).gameObject.SetActive(false);
        //transform.GetChild(11).GetChild(AttChoiceNum).gameObject.SetActive(true);
        //enemyHit.SetActive(true);
        float[] finalDamage = new float[4];                 //[1��,2��,0.5��,0��]
        float[, ] addForceValue = new float[2,3];           //[2��, 0.5�� �����][�Ӽ�����]
        for (int i = 0; i < 3; i++)
        {
            //�ʱ�ȭ
            addForceValue[0, i] = 0; // 2�� �����
            addForceValue[1, i] = 0; // 0.5�� �����
        }
        if (playerAttrib > 0)               //0:�⺻����� 1: ��  // 2: ��ȭ�� // 3:emp
        {
            for (int i = 0; i < AttributeAdd.forceTime[playerAttrib - 1]; i++)
            {
                //��ȭ�� ���
                addForceValue[0, playerAttrib - 1] += ((damage * 2) * item[Stones.stoneLevel, playerAttrib - 1]);
                addForceValue[1, playerAttrib - 1] += ((damage * 0.5f) * item[Stones.stoneLevel, playerAttrib - 1]);
            }
        }
        finalDamage[0] = damage; // 1�� ������
        if (playerAttrib > 0)
        {
            //���������(��ó�� + ��ȭ������ ��ȭ�� ��)
            finalDamage[1] = damage * 2 + addForceValue[0, playerAttrib - 1];       // 2�� �����
            finalDamage[2] = damage * 0.5f + addForceValue[0, playerAttrib - 1];    // 0.5�� �����
        }
        finalDamage[3] = damage * 0; // ������ ��ȿȭ
        //�Ӽ� ��ȭ ������                                                                                             //han's �ڵ�
        if (isAddAF == true)
            finalDamage[1] *= 1.2f;
        if (isAddBH == true)
            finalDamage[2] *= 1.2f;
        if (isAddCY == true)
            finalDamage[3] *= 1.2f;
        //�Ӽ� ó��
        //��
        if (hitTime)
        {
            if (setAttrib == 0)
            {
                switch (playerAttrib)
                {
                    case 0:
                        Shield -= finalDamage[0];
                        break;
                    //�� �Ӽ�
                    case 1:
                        Shield -= finalDamage[1];
                        break;
                    //��ȭ�� �Ӽ�
                    case 2:
                        Shield -= finalDamage[2];
                        break;
                    //emp �Ӽ�
                    case 3:
                        Shield -= finalDamage[3];
                        break;
                }
            }
            //Ư�� ����
            else if (setAttrib == 1)
            {
                switch (playerAttrib)
                {
                    case 0:
                        Shield -= finalDamage[0];
                        break;
                    //�� �Ӽ�
                    case 1:
                        Shield -= finalDamage[2]; 
                        break;
                    //��ȭ�� �Ӽ�
                    case 2:
                        Shield -= finalDamage[1];
                        break;
                    //emp �Ӽ�
                    case 3:
                        Shield -= finalDamage[2]; 
                        break;
                }
            }
            //�κ�
            else if (setAttrib == 2)
            {
                switch (playerAttrib)
                {
                    case 0:
                        Shield -= finalDamage[0];
                        break;
                    //�� �Ӽ�
                    case 1:
                        Shield -= finalDamage[3];
                        break;
                    //��ȭ�� �Ӽ�
                    case 2:
                        Shield -= finalDamage[3];
                        break;
                    //emp �Ӽ�
                    case 3:
                        Shield -= finalDamage[1]; 
                        break;
                }
            }
        }
        Debug.Log("����� �׽�Ʈ " + finalDamage[1]);
        Debug.Log("����� ��ȭ :" + addForceValue[0, 1]);
        //if (finalDamage[1] >= 20)
        //    enemyHit.GetComponent<ParticleSystem>().startColor = Color.red;
        //if (finalDamage[2] <= 15)
        //    enemyHit.GetComponent<ParticleSystem>().startColor = Color.blue;
        //if (finalDamage[3] <= 0)
        //    enemyHit.GetComponent<ParticleSystem>().startColor = Color.black;
        enemyHit.SetActive(true); // �¾��� �� ��ƼŬ
        StartCoroutine(HitTimeCo()); // ���� ���ð�
        GetComponent<Animator>().SetTrigger("Guard");
        //GetComponent<Animator>().SetTrigger("Return");

        //AttPattern = false;

        //for (int i = 0; i < 4; i++)
        //{
        //    transform.GetChild(0).GetChild(i).transform.gameObject.SetActive(false);
        //    transform.GetChild(1).GetChild(i).transform.gameObject.SetActive(false);
        //}
    }

    //���Ͱ� �� ���� �°� �ϱ����� ó��
    IEnumerator HitTimeCo()
    {
        hitTime = false;
        yield return new WaitForSeconds(0.5f);
        enemyHit.SetActive(false);
        hitTime = true;
    }
    public override void OnCrisis()
    {
        Debug.Log("�÷��̾� ȭ��");
    }

    //�ǵ尡 0�Ǹ� �޺��ý��� ����
    public void ComboSystem()
    {
        StartCoroutine(StickCol());
        //GetComponent<Animator>().SetTrigger("Return");
        //nowTime = -100;
    }
    //�޺� ó�� �ڷ�ƾ(�����ʿ�)
    IEnumerator StickCol()
    {
        enemyAttackState = false;
        uiComboTime.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        GetComponent<Animator>().SetBool("Groggy", true);
        //�÷��̾� �ڿ� �ݶ��̴��� ����(��� ����� ���� �ݶ��̴�)
        stickMotion.SetActive(true);
        //��ü ���� ����
        //---------------------------------------------------------------------
        //�޺� �ý��� Ȱ��ȭ
        transform.GetChild(8).gameObject.SetActive(true);
        //�޺� ī��Ʈ�� Ȱ��ȭ�Ǵ� �ݶ��̴� ���� ���� ����
        for (int i = 0; i < comboCount; i++)
            comboRandNum.Add(Random.Range(0, transform.GetChild(8).GetChild(1).childCount));
        bool overlapNum = comboRandNum[0] == comboRandNum[1] || comboRandNum[0] == comboRandNum[2]
                || comboRandNum[1] == comboRandNum[2];
        if (comboCount == 4)
        {
            overlapNum = comboRandNum[0] == comboRandNum[1] || comboRandNum[0] == comboRandNum[2]
                || comboRandNum[0] == comboRandNum[3] || comboRandNum[1] == comboRandNum[2]
                || comboRandNum[1] == comboRandNum[3] || comboRandNum[2] == comboRandNum[3];
        }
        Debug.Log("�ߺ� ó�� : " + overlapNum);
        while (overlapNum)
        {
            comboRandNum.Clear();
            for (int i = 0; i < comboCount; i++)
                comboRandNum.Add(Random.Range(0, transform.GetChild(8).GetChild(1).childCount));
            overlapNum = comboRandNum[0] == comboRandNum[1] || comboRandNum[0] == comboRandNum[2]
                        || comboRandNum[1] == comboRandNum[2];
            Debug.Log("�ߺ� ó�� ���� : " + overlapNum);
        }
        //���� �ݶ��̴� Ȱ��ȭ
        for (int i = 0; i < transform.GetChild(8).GetChild(1).childCount; i++)
            transform.GetChild(8).GetChild(1).GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < comboRandNum.Count; i++)
            transform.GetChild(8).GetChild(1).GetChild(comboRandNum[i]).gameObject.SetActive(true);
        //bool[] comboColState = new bool[3] { false, false, false };
        //int comboState = 0;
        //for (int i = 0; i < comboRandNum.Count; i++)
        //{
        //    if (transform.GetChild(10).GetChild(1).GetChild(comboRandNum[i]).gameObject.activeSelf)
        //        comboColState[i] = true;
        //    for (int j = 0; j < comboColState.Length; j++)
        //    {
        //        if (comboColState[j])
        //            comboState++;
        //    }
        //}
        //if(comboState >= 3)
        //    CoComboManager(false);
        StartCoroutine(ComboFillAmountCo()); // ���ӽð��� ���� ��
        //5�� �Ŀ� �ٷ� ����
        yield return new WaitForSeconds(10);
        CoComboManager(false);
        //�߰� ����
        for (int i = 0; i < transform.GetChild(8).GetChild(1).childCount; i++)
            transform.GetChild(8).GetChild(1).GetChild(i).gameObject.SetActive(false);
        transform.GetChild(8).gameObject.SetActive(false);
        //for (int i = 0; i < transform.GetChild(8).GetChild(1).childCount; i++) // ���� 0��
        //    transform.GetChild(8).GetChild(1).GetChild(i).gameObject.SetActive(false);
        stickMotion.SetActive(false);
        uiComboTime.SetActive(false);
        comboRandNum.Clear();
        Shield = maxShield;
        enemyAttackState = true;
        Debug.Log("�޺��� ���� �� �ǵ� ��" + Shield);
        nowTime = 0;
    }
    //�޺� �ڷ�ƾ ����ȭ�� ���� ���
    void CoComboManager(bool state)
    {
        if (!state)
        {
            comboRandNum.Clear();
            uiComboTime.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        }
        transform.GetChild(8).gameObject.SetActive(state);
        for (int i = 0; i < transform.GetChild(8).GetChild(1).childCount; i++) // ���� 0�̿���
            transform.GetChild(8).GetChild(1).GetChild(i).gameObject.SetActive(state);
        stickMotion.SetActive(state);
        uiComboTime.SetActive(state);
        GetComponent<Animator>().SetBool("Groggy", state);
    }
    //�޺� �ð��� �˷���
    IEnumerator ComboFillAmountCo()
    {
        uiComboTime.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(1);
            uiComboTime.transform.GetChild(0).GetComponent<Image>().fillAmount -= 0.1f;
            if (uiComboTime.transform.GetChild(0).GetComponent<Image>().fillAmount <= 0)
                break;
        }
    }


    //����
    //�� ȹ��, 
    //n�ʵڿ� ��ȭ���̳� ������ ������ ó��
    public override void Dead([CallerMemberName] string caller = "")
    {
        AudioManager.ChangeBgm(Resources.Load<AudioClip>("Sound/Music/StoreAmbience"));
        
        //�� �׸���
        //transform.GetChild(2).gameObject.SetActive(true);
        dropMoney = Random.Range(1000, 1200);
        //StartCoroutine(AlphaCo());
        //transform.GetChild(2).GetComponent<TextMesh>().text = dropMoney.ToString();
        //transform.GetChild(2).GetComponent<TextMesh>().color = new Color(1, 1, 1, alphaColor);
        player.GetComponent<Player>().money += dropMoney;
        ani.SetBool("Dead", true);
        //ani.SetBool("Dead", false);
        isDead = true;
        getMoneyText.text = "ȹ���� �� : " + dropMoney.ToString();
        //�߰� ����
        for (int i = 0; i < transform.GetChild(8).GetChild(1).childCount; i++)
            transform.GetChild(8).GetChild(1).GetChild(i).gameObject.SetActive(false);
        transform.GetChild(8).gameObject.SetActive(false);
        uiComboTime.SetActive(false);
        guardManager.SetActive(false);
        transform.GetChild(10).GetChild(1).gameObject.SetActive(false);
        enemyShield.transform.parent.gameObject.SetActive(false);
        StartCoroutine(RootingCo());
    }
    //OnRooting();
    //gameObject.SetActive(false);
    //n�ʵڿ� ��ȭ���̳� ������ ������ ���������� ó��
    IEnumerator RootingCo()
    {
        yield return new WaitForSeconds(2);
        int ranNum;
        ranNum = Random.Range(0, 1);
        if (ranNum == 0)
            store.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ani.SetBool("Dead", false);
        OnRooting();
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    /// 12/03 �߰��� �ڵ�. OnDie()�̺�Ʈ ���� �����
    
    // �������� ��ȭ���� ������ ����
    public void OnRooting([CallerMemberName] string caller = "")
    {
        //����ǰ ȹ�濡 ���� �ڵ� ���
        //���� ���� ����
        table.SetActive(true);
        for (int i = 0; i < 3; i++)
            stoneRandNum.Add(Random.Range(0, stones.Length));
        bool overlapNum = stoneRandNum[0] == stoneRandNum[1] || stoneRandNum[0] == stoneRandNum[2] // �����ʿ�
                || stoneRandNum[1] == stoneRandNum[2];

        while (overlapNum)
        {
            stoneRandNum.Clear();
            for (int i = 0; i < 3; i++)
                stoneRandNum.Add(Random.Range(0, stones.Length));
            //for (int i = 0; i < stoneRandNum.Count; i++)
            overlapNum = stoneRandNum[0] == stoneRandNum[1] || stoneRandNum[0] == stoneRandNum[2] // �����ʿ�
                || stoneRandNum[1] == stoneRandNum[2];
            //Debug.Log("�ߺ� ó�� ���� : " + overlapNum);
        }
        for (int i = 0; i < stones.Length; i++)
            stones[i].SetActive(false);

        for (int i = 0; i < stoneRandNum.Count; i++)
            stones[stoneRandNum[i]].SetActive(true);

        //stoneRandNum.Clear();

        table.SetActive(true);

        //�ϳ� ���ý� ������� �κ� �ʿ�


        //���� Ȯ���� ��������
        //ranNum = Random.Range(0, 10);
        //if (ranNum % 2 == 0)
        //    OnStore();
        //FileManager.AutoSave(player.inventory, player.money,
        //                        (int)player.MaxHp, (int)player.Hp,
        //                        (int)player.Att, ++FileManager.loadStageCount);

        Debug.Log("stateCheck " + caller);
        //�� �̵��� �÷��̾� ������ ���� �� �� �� �ְԲ� �ϴ� �͵� ���
        //StageManager.ChangeStage();
    }
    // ����� Ȱ��ȭ
    public void OnStore()
    {
        store.gameObject.SetActive(true);
    }
}
