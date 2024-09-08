using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class StartPlayer : MonoBehaviour
{
    [SerializeField] GameObject helpMe;
    PlayerController controller;

    float stopTime = 0;
    float walkDir = 1;
    float walkTime = 2;
    bool letJump = true;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Destroy(helpMe);
            this.enabled = false;
        }
        


        if (Time.time < stopTime)
        {
            return;
        }

        if (Time.time < walkTime)
        {
            if (letJump)
            {
                controller.Move(walkDir, true);
                letJump = false;
            }
            else
            {
                controller.Move(walkDir, false);
            }
            
        }

        else
        {
            letJump = true;
            walkDir *= -1;
            walkTime = Time.time + 5;
            stopTime = Time.time+1;
        }
    }

}
