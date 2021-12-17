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
    public float attackTime = 0; // 공격 대기시간
    private bool attSuccess = false; //사용 안하는듯
    public float shield;
    public float maxShield;
    public int defChoiceNum;        //플레이어의 공격 패턴
    public int attChoiceNum;        //몬스터의 공격 패턴
    public bool attPattern = false;        //몬스터의 공격 패턴 지정 여부
    public List<int> comboRandNum; // 콤보 콜라이더 랜덤 생성을 하기위해 사용
    public List<int> stoneRandNum; // 강화석 랜덤 생성을 하기위해 사용
    public List<GameObject> uiEnemyHp; 
    public Image enemyShield;
    public static int comboCount = 3; // 콤보 콜라이더 활성화 수
    //public GameObject[] attackReady = new GameObject[4]; // getchild로 해결
    public GameObject stickMotion; // 콤보 콜라이더
    public static bool comboCol = false; // 콤보 처리
    public GameObject enemyHit; // 맞았을 때 파티클
    public GameObject uiComboTime; // 콤보 시간을 알려줌
    public GameObject guardManager; // 적의 방어 패턴
    bool enemyAttackState = true; // 공격패턴이 수행될려면 이게 true가 되야 함
    bool hitTime = true; // 한 번만 피해 입게
    public TextMeshProUGUI getMoneyText;

    //대현이형 
    int dropMoney = 0; // 얻은 돈
    private float alphaColor = 1; 
    public GameObject[] stones = new GameObject[6]; // 강화석 목록
    public GameObject table; // 강화석 테이블(이게 활성화 되야 강화석 획득 가능)
    public GameObject store; // 상점
    public Text MoneyText = null;
    //han's
    //아이템 처리를 위해 사용
    public bool isAddAF = false;        //불 강화 여부
    public bool isAddBH = false;        //생화학 강화 여부
    public bool isAddCY = false;        //EMP 강화 여부
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
    //공격타입을 지정했다
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
            Debug.Log("실드 량 : " + shield);
            if (Shield <= 0)
            {
                ComboSystem();
                Debug.Log("실드");
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
    //적의 가드게이지 실시간처리
    //적의 공격패턴(n초)
    void Update()
    {
        enemyShield.fillAmount = shield/(maxShield);
        for (int i = 0; i < uiEnemyHp.Count; i++)
            uiEnemyHp[i].SetActive(false);
        for(int i = 0; i < Hp; i++)
            uiEnemyHp[i].SetActive(true);

        //아이템 이펙트 처리 수정 필요 
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
            Debug.Log("가드매니저온");
        }
        else
        {
            guardManager.SetActive(false);
            Debug.Log("가드매니저오프");
        }


        if (AttPattern == false)
        {
            AttPattern = true;
            //OnDefence();//그리기
            OnAttack();//그리기
        }
        //타이머 하나 더 넣어서 공격 주기 설정
        //5초 후 실행
        if (enemyAttackState)
        {
            if (nowTime >= attackTime) //
            {
                transform.GetChild(9).GetChild(AttChoiceNum).gameObject.SetActive(true); // 몬스터 공격 콜라이더 활성화(플레이어 방어 활성화)
                transform.GetChild(9).GetChild(4).gameObject.SetActive(false); // 실질적으로 공격하는 콜라이더
                nowTime2 += Time.deltaTime;
                EnemyAttack(2); // 2초 뒤에 공격
                attackTime = 2;//Random.Range(2, 5);
            }
        }
        nowTime += Time.deltaTime;
    }
    //적 공격처리
    public void EnemyAttack(float attTime)
    {
        if (nowTime2 >= attTime)
        {
            transform.GetChild(9).GetChild(AttChoiceNum).gameObject.SetActive(false);
            transform.GetChild(9).GetChild(4).gameObject.SetActive(true);//공격콜라이더 활성화
            Debug.Log("공격대기시간지나고 공격했음" + attackTime);

            //애니메이션 진행
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
    //안씀
    public virtual void OnDefence()
    {
        DefChoiceNum = Random.Range(0, 3);
        transform.GetChild(0).GetChild(DefChoiceNum).gameObject.SetActive(true);
    }
    //방향
    public virtual void OnAttack()
    {
        AttChoiceNum = Random.Range(0, 3);
        //transform.GetChild(1).GetChild(AttChoiceNum).gameObject.SetActive(true);
    }
    //실드가 까임
    //속성 + 강화석 + 아이템 모두 처리 
    public override void OnHit(int playerAttrib, float[,] item, float damage)
    {
        AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/EnemyHit"));
        nowTime = 0;
        for (int i = 0; i < transform.GetChild(9).childCount; i++)
            transform.GetChild(9).GetChild(i).gameObject.SetActive(false);
        //transform.GetChild(11).GetChild(AttChoiceNum).gameObject.SetActive(true);
        //enemyHit.SetActive(true);
        float[] finalDamage = new float[4];                 //[1배,2배,0.5배,0배]
        float[, ] addForceValue = new float[2,3];           //[2배, 0.5배 대미지][속성종류]
        for (int i = 0; i < 3; i++)
        {
            //초기화
            addForceValue[0, i] = 0; // 2배 대미지
            addForceValue[1, i] = 0; // 0.5배 대미지
        }
        if (playerAttrib > 0)               //0:기본대미지 1: 불  // 2: 생화학 // 3:emp
        {
            for (int i = 0; i < AttributeAdd.forceTime[playerAttrib - 1]; i++)
            {
                //강화석 계산
                addForceValue[0, playerAttrib - 1] += ((damage * 2) * item[Stones.stoneLevel, playerAttrib - 1]);
                addForceValue[1, playerAttrib - 1] += ((damage * 0.5f) * item[Stones.stoneLevel, playerAttrib - 1]);
            }
        }
        finalDamage[0] = damage; // 1배 데미지
        if (playerAttrib > 0)
        {
            //최종대미지(상성처리 + 강화석으로 강화된 값)
            finalDamage[1] = damage * 2 + addForceValue[0, playerAttrib - 1];       // 2배 대미지
            finalDamage[2] = damage * 0.5f + addForceValue[0, playerAttrib - 1];    // 0.5배 대미지
        }
        finalDamage[3] = damage * 0; // 데미지 무효화
        //속성 강화 아이템                                                                                             //han's 코드
        if (isAddAF == true)
            finalDamage[1] *= 1.2f;
        if (isAddBH == true)
            finalDamage[2] *= 1.2f;
        if (isAddCY == true)
            finalDamage[3] *= 1.2f;
        //속성 처리
        //면
        if (hitTime)
        {
            if (setAttrib == 0)
            {
                switch (playerAttrib)
                {
                    case 0:
                        Shield -= finalDamage[0];
                        break;
                    //불 속성
                    case 1:
                        Shield -= finalDamage[1];
                        break;
                    //생화학 속성
                    case 2:
                        Shield -= finalDamage[2];
                        break;
                    //emp 속성
                    case 3:
                        Shield -= finalDamage[3];
                        break;
                }
            }
            //특수 섬유
            else if (setAttrib == 1)
            {
                switch (playerAttrib)
                {
                    case 0:
                        Shield -= finalDamage[0];
                        break;
                    //불 속성
                    case 1:
                        Shield -= finalDamage[2]; 
                        break;
                    //생화학 속성
                    case 2:
                        Shield -= finalDamage[1];
                        break;
                    //emp 속성
                    case 3:
                        Shield -= finalDamage[2]; 
                        break;
                }
            }
            //로봇
            else if (setAttrib == 2)
            {
                switch (playerAttrib)
                {
                    case 0:
                        Shield -= finalDamage[0];
                        break;
                    //불 속성
                    case 1:
                        Shield -= finalDamage[3];
                        break;
                    //생화학 속성
                    case 2:
                        Shield -= finalDamage[3];
                        break;
                    //emp 속성
                    case 3:
                        Shield -= finalDamage[1]; 
                        break;
                }
            }
        }
        Debug.Log("대미지 테스트 " + finalDamage[1]);
        Debug.Log("대미지 강화 :" + addForceValue[0, 1]);
        //if (finalDamage[1] >= 20)
        //    enemyHit.GetComponent<ParticleSystem>().startColor = Color.red;
        //if (finalDamage[2] <= 15)
        //    enemyHit.GetComponent<ParticleSystem>().startColor = Color.blue;
        //if (finalDamage[3] <= 0)
        //    enemyHit.GetComponent<ParticleSystem>().startColor = Color.black;
        enemyHit.SetActive(true); // 맞았을 때 파티클
        StartCoroutine(HitTimeCo()); // 공격 대기시간
        GetComponent<Animator>().SetTrigger("Guard");
        //GetComponent<Animator>().SetTrigger("Return");

        //AttPattern = false;

        //for (int i = 0; i < 4; i++)
        //{
        //    transform.GetChild(0).GetChild(i).transform.gameObject.SetActive(false);
        //    transform.GetChild(1).GetChild(i).transform.gameObject.SetActive(false);
        //}
    }

    //몬스터가 한 번만 맞게 하기위해 처리
    IEnumerator HitTimeCo()
    {
        hitTime = false;
        yield return new WaitForSeconds(0.5f);
        enemyHit.SetActive(false);
        hitTime = true;
    }
    public override void OnCrisis()
    {
        Debug.Log("플레이어 화남");
    }

    //실드가 0되면 콤보시스템 시작
    public void ComboSystem()
    {
        StartCoroutine(StickCol());
        //GetComponent<Animator>().SetTrigger("Return");
        //nowTime = -100;
    }
    //콤보 처리 코루틴(정리필요)
    IEnumerator StickCol()
    {
        enemyAttackState = false;
        uiComboTime.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        GetComponent<Animator>().SetBool("Groggy", true);
        //플레이어 뒤에 콜라이더를 켜줌(찌르기 모션을 위한 콜라이더)
        stickMotion.SetActive(true);
        //객체 생성 지점
        //---------------------------------------------------------------------
        //콤보 시스템 활성화
        transform.GetChild(8).gameObject.SetActive(true);
        //콤보 카운트는 활성화되는 콜라이더 수를 설정 해줌
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
        Debug.Log("중복 처리 : " + overlapNum);
        while (overlapNum)
        {
            comboRandNum.Clear();
            for (int i = 0; i < comboCount; i++)
                comboRandNum.Add(Random.Range(0, transform.GetChild(8).GetChild(1).childCount));
            overlapNum = comboRandNum[0] == comboRandNum[1] || comboRandNum[0] == comboRandNum[2]
                        || comboRandNum[1] == comboRandNum[2];
            Debug.Log("중복 처리 와일 : " + overlapNum);
        }
        //랜덤 콜라이더 활성화
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
        StartCoroutine(ComboFillAmountCo()); // 지속시간을 보여 줌
        //5초 후에 바로 꺼짐
        yield return new WaitForSeconds(10);
        CoComboManager(false);
        //추가 수정
        for (int i = 0; i < transform.GetChild(8).GetChild(1).childCount; i++)
            transform.GetChild(8).GetChild(1).GetChild(i).gameObject.SetActive(false);
        transform.GetChild(8).gameObject.SetActive(false);
        //for (int i = 0; i < transform.GetChild(8).GetChild(1).childCount; i++) // 원래 0임
        //    transform.GetChild(8).GetChild(1).GetChild(i).gameObject.SetActive(false);
        stickMotion.SetActive(false);
        uiComboTime.SetActive(false);
        comboRandNum.Clear();
        Shield = maxShield;
        enemyAttackState = true;
        Debug.Log("콤보가 끝난 후 실드 값" + Shield);
        nowTime = 0;
    }
    //콤보 코루틴 최적화를 위해 사용
    void CoComboManager(bool state)
    {
        if (!state)
        {
            comboRandNum.Clear();
            uiComboTime.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        }
        transform.GetChild(8).gameObject.SetActive(state);
        for (int i = 0; i < transform.GetChild(8).GetChild(1).childCount; i++) // 원래 0이였음
            transform.GetChild(8).GetChild(1).GetChild(i).gameObject.SetActive(state);
        stickMotion.SetActive(state);
        uiComboTime.SetActive(state);
        GetComponent<Animator>().SetBool("Groggy", state);
    }
    //콤보 시간을 알려줌
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


    //죽음
    //돈 획득, 
    //n초뒤에 강화석이나 상점이 나오게 처리
    public override void Dead([CallerMemberName] string caller = "")
    {
        AudioManager.ChangeBgm(Resources.Load<AudioClip>("Sound/Music/StoreAmbience"));
        
        //돈 그리기
        //transform.GetChild(2).gameObject.SetActive(true);
        dropMoney = Random.Range(1000, 1200);
        //StartCoroutine(AlphaCo());
        //transform.GetChild(2).GetComponent<TextMesh>().text = dropMoney.ToString();
        //transform.GetChild(2).GetComponent<TextMesh>().color = new Color(1, 1, 1, alphaColor);
        player.GetComponent<Player>().money += dropMoney;
        ani.SetBool("Dead", true);
        //ani.SetBool("Dead", false);
        isDead = true;
        getMoneyText.text = "획득한 돈 : " + dropMoney.ToString();
        //추가 수정
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
    //n초뒤에 강화석이나 상점이 나오게 실질적으로 처리
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

    /// 12/03 추가분 코드. OnDie()이벤트 이후 실행됨
    
    // 랜덤으로 강화석이 나오게 설정
    public void OnRooting([CallerMemberName] string caller = "")
    {
        //전리품 획득에 관한 코드 기술
        //스톤 랜덤 전시
        table.SetActive(true);
        for (int i = 0; i < 3; i++)
            stoneRandNum.Add(Random.Range(0, stones.Length));
        bool overlapNum = stoneRandNum[0] == stoneRandNum[1] || stoneRandNum[0] == stoneRandNum[2] // 수정필요
                || stoneRandNum[1] == stoneRandNum[2];

        while (overlapNum)
        {
            stoneRandNum.Clear();
            for (int i = 0; i < 3; i++)
                stoneRandNum.Add(Random.Range(0, stones.Length));
            //for (int i = 0; i < stoneRandNum.Count; i++)
            overlapNum = stoneRandNum[0] == stoneRandNum[1] || stoneRandNum[0] == stoneRandNum[2] // 수정필요
                || stoneRandNum[1] == stoneRandNum[2];
            //Debug.Log("중복 처리 와일 : " + overlapNum);
        }
        for (int i = 0; i < stones.Length; i++)
            stones[i].SetActive(false);

        for (int i = 0; i < stoneRandNum.Count; i++)
            stones[stoneRandNum[i]].SetActive(true);

        //stoneRandNum.Clear();

        table.SetActive(true);

        //하나 선택시 사라지는 부분 필요


        //랜덤 확률로 상점가기
        //ranNum = Random.Range(0, 10);
        //if (ranNum % 2 == 0)
        //    OnStore();
        //FileManager.AutoSave(player.inventory, player.money,
        //                        (int)player.MaxHp, (int)player.Hp,
        //                        (int)player.Att, ++FileManager.loadStageCount);

        Debug.Log("stateCheck " + caller);
        //씬 이동은 플레이어 측에서 원할 때 갈 수 있게끔 하는 것도 고려
        //StageManager.ChangeStage();
    }
    // 스토어 활성화
    public void OnStore()
    {
        store.gameObject.SetActive(true);
    }
}
