using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VolumeManager : MonoBehaviour
{
    public int value;
    int value2;
    //public TextMeshProUGUI text =null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //text.text = value.ToString();
        //UiManager.SetVolume(value2);
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Sword"))
            //value2 = value;
    }
}
