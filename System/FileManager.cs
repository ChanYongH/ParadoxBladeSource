using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.CompilerServices;


public class FileManager : GameManager<FileManager>
{
    public GameObject player;
    //public GameObject[] playerClone;

    //public static GameObject loadPlayer;
    public static int[] loadInventory;
    public static int loadMoney;
    public static int loadMaxHp;
    public static int loadHp;
    public static int loadAtt;
    public static int loadStageCount;

    public static int loadVolumeLv;        //������ 1~10���� ����
    public static int loadBlindLv;         //������ 1~10���� ����

    private void Start()
    {
        string filePath = Application.persistentDataPath + "/AutoSave.json";
        //loadPlayer = new GameObject();

        //�÷��̸� ������ ��, ���̺� ������ ���ٸ� �ϳ� �����.
        if (File.Exists(filePath))
        {
            //Debug.Log("����");
        }
        else
        {

            //Debug.Log("����");
            //����Ʈ. �ʱ� ����
            DataField firstData = new DataField();
            //������ ������ �ʱ�ȭ. �÷��̾ ���⼭ ���� �����´�.
            firstData.inventory = new int[6];
            firstData.money = 0;
            firstData.maxHp = 3;
            firstData.hp = firstData.maxHp;
            firstData.att = 10;
            firstData.stageCount = 1;


            firstData.volumeLv = 5;
            firstData.blindLv = 5;
            //�����͸� jsonŸ������ ��ȯ
            string tempData = JsonUtility.ToJson(firstData);

            //�����(���, ������)
            File.WriteAllText(filePath, tempData);
        }

        //�ڵ� �׽�Ʈ
        //AutoSave(player, 12);
        //OnLoad();
        //Setting(10, 10);
        //OnLoad();               //�÷��̾�, 12, 10, 10
    }


    /// <summary>
    /// �� ������ ȯ�� ����, �ڵ� ����, �ҷ����⿡ ���� ��
    /// </summary>

    //ȯ�� �������� ����, �þ߰� ������ ��. �÷��̾� ���� �� �������� ������ ħ������ �ʱ� ���� �и�
    public static void OptionSetting(int volume, int blind)
    {
        //���� ���
        string filePath = Application.persistentDataPath + "/AutoSave.json";

        //�Ͻ������� �ҷ��� ���� ����
        string fromJson = File.ReadAllText(filePath);
        DataField loadData = JsonUtility.FromJson<DataField>(fromJson);

        //���̺� ����
        DataField data = new DataField();
        data.inventory = loadData.inventory;
        data.money = loadData.money;
        data.maxHp = loadData.maxHp;
        data.hp = loadData.hp;
        data.att = loadData.att;
        data.stageCount = loadData.stageCount;

        data.volumeLv = volume;
        data.blindLv = blind;
        string tempData = JsonUtility.ToJson(data);

        File.WriteAllText(filePath, tempData);
    }

    //�÷��̾� ������ �������� ���� ������ ��. ����, �þ߰� ������ ħ������ �ʱ� ���� �и�
    public static void AutoSave(int[] invetory, int money, int maxHp, int hp, int att, int stageCount, [CallerMemberName] string caller = "")
    {
        //���� ���
        string filePath = Application.persistentDataPath + "/AutoSave.json";

        //�Ͻ������� �ҷ��� ���� ����
        string fromJson = File.ReadAllText(filePath);
        DataField loadData = JsonUtility.FromJson<DataField>(fromJson);

        //���̺� ����
        DataField data = new DataField();
        //data.player = player;
        data.inventory = new int[6];
        data.money = money;
        data.maxHp = maxHp;
        data.hp = hp;
        data.att = att;
        data.stageCount = stageCount;

        data.volumeLv = loadData.volumeLv;
        data.blindLv = loadData.blindLv;
        string tempData = JsonUtility.ToJson(data);

        File.WriteAllText(filePath, tempData);
        Debug.Log("�������� �׽�Ʈ : " + caller);
    }

    public static void OnLoad()
    {
        //������ �ҷ�����
        string filePath = Application.persistentDataPath + "/AutoSave.json";
        string fromJson = File.ReadAllText(filePath);
        DataField loadData = JsonUtility.FromJson<DataField>(fromJson);
        //Debug.Log("�ҷ��� ��: '" + loadData.player + "' / '" + loadData.stageCount + "' / '" + loadData.volumeLv + "' / '" + loadData.blindLv + "'");



        //�ܺο��� ������ ����ƽ ������ �� ����
        //loadPlayer = loadData.player;
        loadInventory = loadData.inventory;
        loadMoney = loadData.money;
        loadMaxHp = loadData.maxHp;
        loadHp = loadData.hp;
        loadAtt = loadData.att;
        
        loadStageCount = loadData.stageCount;
        loadVolumeLv = loadData.volumeLv;
        loadBlindLv = loadData.blindLv;

    }
}


[System.Serializable]
public class DataField
{
    //public GameObject player;
    //�÷��̾� ����
    public int[] inventory;
    public int money;
    public int maxHp;
    public int hp;
    public int att;

    //�������� ����
    public int stageCount;

    //������ �þ߰�
    public int volumeLv;        //������ 1~10���� ����
    public int blindLv;         //������ 1~10���� ����
}