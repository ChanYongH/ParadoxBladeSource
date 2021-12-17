using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttributeAdd : Stones
{
    float[,] increase = new float[3, 3]; // �Ӽ���ȭ������ ���
    public static int[] forceTime = new int[3] { 0, 0, 0 };  // �Ӽ� ��ȭ������ ���
    void Awake()
    {
        if (StageManager.stageNumber.Between(2, 4)) // (Ȯ�� �޼��� ���) 2~4���������� ��ȭ���� ������ 1
            stoneLevel = 1;
        else if (StageManager.stageNumber.Between(5, 7)) // (Ȯ�� �޼��� ���) 5~7���������� ��ȭ���� ������ 2
            stoneLevel = 2;
        switch (attrib)
        {
            case 0:
                itemName = "�� �Ӽ� ��ȭ��";
                itemEffect = "�� �Ӽ� ���ݷ� " + ((stoneLevel +1) * 10) + "% ��ȭ";
                break;
            case 1:
                itemName = "��ȭ�� �Ӽ� ��ȭ��";
                itemEffect = "��ȭ�� �Ӽ� ���ݷ� " + ((stoneLevel + 1) * 10) + "% ��ȭ";
                break;
            case 2:
                itemName = "EMP �Ӽ� ��ȭ��";
                itemEffect = "EMP �Ӽ� ���ݷ� " + ((stoneLevel + 1) * 10) + "% ��ȭ";
                break;
        }
    }
    //ȿ�� ����
    public override void AddAbility()
    {
        forceTime[attrib]++; // ��ȭ�� �����ǰ�
        Debug.Log("�Ӽ� ��ȭ ���");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // % ��ŭ ��ȭ
                increase[i, j] = ((i + 1) * 10) / 100f;
            }
        }
        Player.itemForce[stoneLevel, attrib] = increase[stoneLevel, attrib]; // �÷��̾��� �Ӽ��� ���� ��ȭ ���� �־���
                                                                            // %�� ���� ���ϴ°� Enemy.OnHit()���� ó��
        base.AddAbility();
    }
}

