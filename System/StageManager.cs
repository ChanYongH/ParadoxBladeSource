using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Runtime.CompilerServices;

public class StageManager : GameManager<StageManager>
{
    //Vector3 startPosition;
    public GameObject player;       //����� �÷��̾� ������ ���� ��쿡 ���. ��ĥ ��, �̰��� �÷��̾� �������� �־��ٰ�
    public static GameObject playerClone;
    public static int stageNumber;
    public Text tempText1;
    public static Text stageCheck;
    public Text tempText2;
    public static Text fieldCheck;
    //public static int fieldMax = 6;

    public static bool isCreate = false;
    public static int maxStage = 7;

    // Start is called before the first frame update
    void Start()
    {
        FileManager.OnLoad();
        //player = FileManager.loadPlayer;
        //Debug.Log(FileManager.loadPlayer);

        //�����̸� ���� �ʿ䰡 �ִٸ� �̰���
        //playerClone = player;

        //���������� ������, ���� ���ǰ� �ִ� �ʵ�(��)�� �������� �����ֱ�
        stageCheck = tempText1;
        fieldCheck = tempText2;
    }

    void OnEnable()
    {
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FileManager.OnLoad();

        if(FileManager.loadStageCount <= maxStage)
            Instantiate(player);
        //Instantiate(FileManager.loadPlayer);
    }

    public static void ChangeStage()
    {
        FileManager.OnLoad();

        stageNumber = FileManager.loadStageCount;
        //Debug.Log(stageNumber + "_��������");
        stageCheck.text = "stageCount: " + stageNumber;

        //Debug.Log(FileManager.loadStageCount);
        if (FileManager.loadStageCount > maxStage)
        {
            SceneManager.LoadScene("Player_Win_Ending");
            AudioManager.ChangeBgm(Resources.Load<AudioClip>("Sound/Music/GameClear"));
        }
        else if (stageNumber > 6 || stageNumber <= 6)
        {
            stageNumber %= 7;
            SceneManager.LoadScene(stageNumber + "_Stage");
        }
        fieldCheck.text = "fieldcheck: " + stageNumber;
        AudioManager.ChangeBgm(Resources.Load<AudioClip>("Sound/Music/Battle"));
        //�� �̸��� "stageCount_�̸�"���� ����
        //SceneManager.LoadScene(stageNumber + "_Stage");
        //Debug.Log(stageNumber + "_��������");
    }
}
