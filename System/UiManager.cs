using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManager : GameManager<UiManager>
{
    public Canvas tempCanvas = null;
    public static Canvas canvas = null;

    public Text tempVolumeText = null;
    public static Text volumeText = null;

    //�ӽ÷� ����� ��
    //public static  TextMeshProUGUI volumeTextTest;

    public Text tempBlindAngle = null;
    public static Text blindAngleText = null;

    public static int volume;
    public static int blindAngle;
    private bool isLoad = false;

    private void Start()
    {
        //�ӽ�
        //volumeTextTest = GameObject.Find("VolumeTextTest").GetComponent<TextMeshProUGUI>();
        //canvas = tempCanvas;
        //volumeText = tempVolumeText;
        //blindAngleText = tempVolumeText;
        //canvas.transform.GetChild(1).gameObject.SetActive(false);
        //volumeText = canvas.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        //blindAngleText = canvas.transform.GetChild(1).GetChild(1).GetComponent<Text>();
    }

    private void Update()
    {
        if (isLoad == false)
        {
            //����� �� �ҷ�����
            FileManager.OnLoad();
            volume = FileManager.loadVolumeLv;
            blindAngle = FileManager.loadBlindLv;
            isLoad = true;

            //�ð����� ���
            //olumeText.text = "����: " + volume;
            //blindAngleText.text = "�þ߰�: " + blindAngle;
        }
    }

    //���� ����
    public static void PushStartButton() 
    {
        //Debug.Log(FileManager.loadPlayer);

        FileManager.AutoSave(FileManager.loadInventory, 0, 3, 3, 10,  0);

        //Debug.Log(FileManager.loadPlayer);
        //StageManager.ChangeStage(); // �������
        //SceneManager.LoadScene("2_Stage");
    }

    //���� �ҷ�����
    public static void PushLoadButton() 
    {
        //StageManager.ChangeStage(); // �������
    }

    //ȯ�� ����
    public static void PushOptionButton()
    {
        canvas.transform.GetChild(0).gameObject.SetActive(false);
        canvas.transform.GetChild(1).gameObject.SetActive(true);
    }

    //���� ��ư
    public static void PushExitButton()
    {
        Debug.Log("��� �����մϴ�.");
        Application.Quit();
    }

    //ȯ�� ���� -> ���� �ø���
    public static void PushVolumeUp()
    {
        if (volume >= 10) { }
        else
            volume++;
        volumeText.text = "����: " + volume;
    }

    //�ӽ÷� ����� ��
    //public static void SetVolume(int value)
    //{
    //    volume = value;
    //    volumeTextTest.text = value.ToString();
    //}

    //ȯ�� ���� -> ���� ���߱�
    public static void PushVolumeDown()
    {
        if (volume <= 0) { }
        else
            volume--;
        volumeText.text = "����: " + volume;
    }

    //ȯ�� ���� -> �þ߰� ����
    public static void PushBlindAngleUp()
    {
        if (blindAngle >= 10) { }
        else
            blindAngle++;
        blindAngleText.text = "�þ߰�: " + blindAngle;
    }

    //ȯ�� ���� -> �þ߰� ����
    public static void PushBlindAngleDown()
    {
        if (blindAngle <= 0) { }
        else
            blindAngle--;
        blindAngleText.text = "�þ߰�: " + blindAngle;
    }

    //ȯ�� ���� -> ���� �޴���. �������� ���嵵 �����
    public static void PushToMainMenu()
    {
        canvas.transform.GetChild(0).gameObject.SetActive(true);
        canvas.transform.GetChild(1).gameObject.SetActive(false);
        //������ ���� .json ���� ����
        FileManager.OptionSetting(volume, blindAngle);
        AudioManager.OnVolumControl(volume);
    }
}
