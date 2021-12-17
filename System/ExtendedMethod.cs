using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendedMethod
{

    //�⺻���� Ÿ�̸Ӹ� ������ �Ű������� ������ �ȵ�
    // ���� ���ð�, ��ƼŬ�� ���� �ð����ȸ� �����Ű�� ������ �Ű������� ����
    //�̿ϼ���
    public static bool Timer(this float repeatTime, GameObject setActive)
    {
        float nowTime = 0;
        nowTime += Time.time;
        if (nowTime > repeatTime)
        {
            if (setActive != null)
                setActive.SetActive(false);
            nowTime = 0;
            return true;
        }
        else
        {
            if (setActive != null)
                setActive.SetActive(true);
            return false;
        }
    }

    public static bool Between(this float bet, float min, float max)
    {
        if (min <= bet && bet <= max)
            return true;
        else
            return false;
    }

    public static bool Between(this int bet, int min, int max)
    {
        if (min <= bet && bet <= max)
            return true;
        else
            return false;
    }
}
