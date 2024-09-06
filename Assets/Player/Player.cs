using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour
{

    PlayerController controller;
    void Start()
    {
        controller = GetComponent<PlayerController> ();
    }

    // Update is called once per frame
    void Update()
    {

        float x_dir = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        controller.Move(x_dir,jump);
    }
}
