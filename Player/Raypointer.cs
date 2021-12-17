using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Raypointer : MonoBehaviour
{
    private LineRenderer layser;        // ������
    private RaycastHit Collided_object; // �浹�� ��ü
    private GameObject currentObject;   // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü
    public GameObject mainCanvas = null;
    public TextMeshProUGUI text = null;
    public GameObject grabSwitch = null;
    public GameObject magnetPart = null;

    public float raycastDistance = 100f; // ������ ������ ���� �Ÿ�

    // Start is called before the first frame update
    void Start()
    {
        // ��ũ��Ʈ�� ���Ե� ��ü�� ���� ��������� ������Ʈ�� �ְ��ִ�.
        layser = this.gameObject.AddComponent<LineRenderer>();

        // ������ �������� ���� ǥ��
        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(0, 195, 255, 0.5f);
        layser.material = material;
        // �������� �������� 2���� �ʿ� �� ���� ������ ��� ǥ�� �� �� �ִ�.
        layser.positionCount = 2;
        // ������ ���� ǥ��
        layser.startWidth = 0.01f;
        layser.endWidth = 0.01f;
    }


    // Update is called once per frame

    //���� ������ ��, ��ư�� ������ ������ �׼��� ����
    void Update()
    {
        layser.SetPosition(0, transform.position); // ù��° ������ ��ġ
                                                   // ������Ʈ�� �־� �����ν�, �÷��̾ �̵��ϸ� �̵��� ���󰡰� �ȴ�.
                                                   //  �� �����(�浹 ������ ����)
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green, 0.5f);
        if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            magnetPart.SetActive(false);

        // �浹 ���� ��
        if (Physics.Raycast(transform.position, transform.forward, out Collided_object, raycastDistance))
        {
            layser.SetPosition(1, Collided_object.point);
            if(Collided_object.collider.gameObject.CompareTag("NextStage"))
            {
                Collided_object.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.gray;
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                    StageManager.ChangeStage();

            }

            // ��ȭ���� ���� �ϸ�
            if (Collided_object.collider.gameObject.CompareTag("Stone"))
            {
                text.text = Collided_object.transform.name;
                Collided_object.transform.GetComponent<ReturnPlace>().lightSwitch = true;
                Collided_object.transform.GetComponent<Stones>().ShowInfo();
                
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) //�� ������ �� ���� ���
                    AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/Magnet"));
                else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger)) // ���� �� ���� 
                    AudioManager.sfxController.Stop();
                if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
                {
                    Collided_object.transform.LookAt(this.transform.position); // ��ȭ���� �÷��̾� �������� ����
                    Collided_object.transform.Translate(Vector3.forward * 0.1f); // ������
                    OVRGrabber.grab = true;
                    magnetPart.SetActive(true);
                }
                else
                {
                    magnetPart.SetActive(false);
                }
            }
            else
            {
                GameObject.FindGameObjectWithTag("MainCanvas").transform.GetChild(0).gameObject.SetActive(false);
                //mainCanvas.SetActive(false);
            }
            if (Collided_object.collider.CompareTag("NextStone"))
            {
                //AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/UIRay"));
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/UISelect"));
                    Collided_object.collider.GetComponent<GoForceStone>().ShowStoneScene();
                }
            }
            //���� �κ�
            if (Collided_object.collider.GetComponent<Store>() != null)
            {
                //AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/ItemRay"));
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    AudioManager.PlaySfx(Resources.Load<AudioClip>("Sound/SFX/BuyItem"));
                    Collided_object.collider.GetComponent<Store>().SaleItem();
                }
            }
                // ��ŧ���� �� �����ܿ� ū ���׶�� �κ��� ���� ���
                //if (OVRInput.Get(OVRInput.Button.One))
                //{
                //    // ��ư�� ��ϵ� onClick �޼ҵ带 �����Ѵ�.
                //    //Collided_object.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                //    Collided_object.transform.position = this.transform.position;
                //}
                //else
                //{
                //    //Collided_object.collider.gameObject.GetComponent<Button>().OnPointerEnter(null);
                //    currentObject = Collided_object.collider.gameObject;
                //}


        }
        else
        {
            // �������� ������ ���� ���� ������ ������ �ʱ� ���� ���̸�ŭ ��� �����.
                layser.SetPosition(1, transform.position + (transform.forward * raycastDistance));
            // �ֱ� ������ ������Ʈ�� Button�� ���
            // ��ư�� ���� �����ִ� �����̹Ƿ� �̰��� Ǯ���ش�.
            if (currentObject != null)
            {
                //currentObject.GetComponent<Button>().OnPointerExit(null);
                currentObject = null;
            }

        }

    }

    private void LateUpdate()
    {
        // ��ư�� ���� ���        
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //layser.material.color = new Color(255, 255, 255, 0.5f);
        }

        // ��ư�� �� ���          
        else if (OVRInput.GetUp(OVRInput.Button.One))
        {
            //layser.material.color = new Color(0, 195, 255, 0.5f);
        }
    }

    IEnumerator ComingStone(RaycastHit obj)
    {
        bool tempbool = true;
        while (tempbool)
        {
            yield return new WaitForSeconds(0.5f);
            int temp = 0;
            temp++;
            obj.transform.Translate(Vector3.forward * 0.01f);
            Debug.Log("�������?");
            if (temp > 10)
                tempbool = false;
        }
    }
}