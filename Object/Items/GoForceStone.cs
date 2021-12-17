using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForceStone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void ShowStoneScene()
    {
        transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
