using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class Player : Character
{
    private bool isLoad = false; // 불러오기 관련
    public GameObject playerSword; // 파티클 및 매테리얼 변경
    public static int[] inventory = new int[6] { 0, 0, 0, 0, 0, 0 }; // 인벤토리 소지유무 처리
    public float[] attirbAtt = new float[9]; // 상성에 의한 대미지 처리
    public static float[,] itemForce = new float[3,3]; // nomal ,불, 화학, emp
    public List<GameObject> uiPlayerHp; // PlayerHpUI처리
    public GameObject attackCol = null; // 휘둘렀을 때 활성화(Player Attack)
    //public GameObject comboCol = null;
    public int money;
    public Queue<int> setAttribQueue = new Queue<int>(); // 속성 변경
    //콤보
    public Transform comboStart; // 안 쓰는거같음
    public float comboStartSpot; // 안 쓰는거같음

    public int enemyGuardAttack = 0; // 적의 가드를 때리면 ++
    public List<GameObject> enemyGuardState; // enemyGuardAttack -> UI

    public bool isImmortality = false; // 죽음 지연 물약을 사용 하면
    public GameObject tazdingo; // 아이템 메테리얼

    public bool[] soundDelay = new bool[3]; // 사운드 한 번만 들리게 처리
    //int attKind = 5;
    //public int defKind;
    public string playerName = "";

    //초기화
    public virtual void Awake()
    {
        //maxHp = hp;
        //att = 10;
        uiPlayerHp = new List<GameObject>();
        //for (int i = 0; i < inventory.Length; i++) // 인벤토리 초기화
            //inventory[i] = 0;
        for (int i = 0; i < 4; i++)
            setAttribQueue.Enqueue(i);
        for (int i = 0; i < GameObject.Find("PlayerHp").transform.childCount; i++)
            uiPlayerHp.Add(GameObject.Find("PlayerHp").transform.GetChild(i).gameObject);
        for (int i = 0; i < GameObject.Find("EnemyShieldAttack").transform.childCount; i++)
            enemyGuardState.Add(GameObject.Find("EnemyShieldAttack").transform.GetChild(i).gameObject);

    }

    //불러오기
    //PlayerUI관련 실시간 처리
    //PlayerAttack관련 처리(벨로시티로 계산)

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
            Debug.Log("인벤토리 " + i + " " + inventory[i]);

        if (enemyGuardAttack > 2) // 몬스터 실드를 3번 쳤을 시 Hp가 감소
        {
            Hp--;
            enemyGuardAttack = 0;
        }
        for (int i = 0; i < enemyGuardState.Count; i++)
            enemyGuardState[i].SetActive(false);
        for (int i = 0; i < enemyGuardAttack; i++)
            enemyGuardState[i].SetActive(true);
        //PlayerHp처리(UI)
        for (int i = 0; i < uiPlayerHp.Count; i++)
            uiPlayerHp[i].SetActive(false);
        for (int i = 0; i < Hp; i++)
            uiPlayerHp[i].SetActive(true);
        if (setAttrib >= 1)
            playerSword.transform.GetChild(0).gameObject.SetActive(true); // 파티클
        else
            playerSword.transform.GetChild(0).gameObject.SetActive(false);

        if (OVRInput.GetDown(OVRInput.Button.One))
            SetAttribute();
        //받는 힘이 3이상이고 검을 장착하고있으면 발동
        bool basicAttack = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RHand).magnitude >= 3f && 
            ReturnPlace.sword;

        if (basicAttack)
        {
            Debug.Log("가속도 : " + OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RHand).magnitude);
            //Debug.Log("실드 공격!");
            //속성에 따라 다른 사운드 출력
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
                //어택 콜라이더 활성화
                attackCol.SetActive(true);
                //파티클
                attackCol.transform.GetChild(0).gameObject.SetActive(true);
            }
            Debug.Log("기본 공격2");
        }
        else
        {
            for (int i = 0; i < soundDelay.Length; i++)
                soundDelay[i] = true;
            //어택 콜라이더 비활성화
            if (attackCol != null)
            {
                attackCol.SetActive(false);
                attackCol.transform.GetChild(0).gameObject.SetActive(true);
            }
        }


        //Debug.Log("실드 테스트 : " + ReturnPlace.sword);
        Debug.Log("불 속성 강화 : " + itemForce[0,0]);
        Debug.Log("화학 속성 강화 : " + itemForce[0, 1]);
        Debug.Log("emp 속성 강화 : " + itemForce[0, 2]);
    }
    //죽음(씬 넘김)
    public override void Dead([CallerMemberName] string caller = "")
    {
        if (isImmortality)
            Hp = 1;
        else
        //Debug.Log("왜죽어?");
        {
            SceneManager.LoadScene("Player_Lose_Ending");
            AudioManager.ChangeBgm(Resources.Load<AudioClip>("Sound/Music/GameOver"));
        }
        //SceneManager.LoadScene(10);
    }
    //이건 안씀
    public override void OnHit(int playerAttrib, float[,] item, float damage)
    {
        Hp -= damage;
    }
    public override void OnCrisis()
    {
        Debug.Log("플레이어 화남");
        AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/Heartbeat"));
    }

    //속성 변경(파티클, 마테리얼, 사운드 변경)
    public void SetAttribute()
    {
        setAttrib = setAttribQueue.Dequeue();
        //리소스에 있는 메테리얼로 외형(색깔)을 변경
        playerSword.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Sword7/" + setAttrib);

        setAttribQueue.Enqueue(setAttrib); // 
        Debug.Log("큐 : " + setAttribQueue.Peek());

        //바꿀 때 마다 사운드를 재생하고 파티클의 색깔을 바꿔줌
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
