using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class StartPlayer : MonoBehaviour
{

    //This script is only used to make the player walk and ask for help at the start of game
    //Gets disabled after first input from kb or mouse
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
            controller.MoveBool(false);
        }
    }

}
