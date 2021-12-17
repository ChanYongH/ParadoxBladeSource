using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Store : MonoBehaviour
{
    Player player;
    public int itemList = 0; // Item.Kind
    public int price = 0; // ����
    //UIó��
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI playerMoney;
    //ui���� ó��
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        itemPrice.text = "���� : " + price + "Gold";
    }
    //�÷��̾� �������� �ǽð����� �ҷ���
    public void Update()
    {
        playerMoney.text = "������ : " + player.money + "Gold";
    }
    //������ ����(�÷��̾��κ��丮[itemList]++)
    public void SaleItem() // ������ ���� 
    {
        //player.inventory[itemList]++; // ���
        if (player.money >= price)
        {
            //player.inventory[itemList]++;
            Player.inventory[itemList]++;
            player.money -= price;
        }
        else
            Debug.Log("���� ��� ����");
    }
}
