using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    //Sliders to change attribute values
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider jumpSlider;
    [SerializeField] private Slider scaleSlider;
    [SerializeField] private Canvas _canvas;

    //Check if player started game
    private bool locked = true;

    //Check if player crossed the finish line
    private bool crossed = false;
    [SerializeField] private GameObject finishImage;
    
    private Camera _camera;
    private PlayerController controller;
    void Start()
    {

        _camera = Camera.main;
        controller = GetComponent<PlayerController>();


        //Initialize slider functions
        speedSlider.onValueChanged.AddListener((v) => { controller.speed = v; });
        jumpSlider.onValueChanged.AddListener((j) => { controller.jumpSpeed = j; });
        scaleSlider.onValueChanged.AddListener((s) => { 
            transform.localScale = Vector3.one * s;
            //_camera.orthographicSize = 4 + s;
        });
    }
    

    
    void Update()
    {
        
        //Reposition camera and canvas relative to player
        //DO NOT MAKE CHILDREN, ISSUES CAUSED BY SCALING
        _camera.transform.position = transform.localPosition + new Vector3(0, 1, -10);
        _canvas.transform.position = transform.localPosition;


        
        if (locked)
        {
            if (Input.anyKey)
            {
                locked = false;
            }
            else
            {
                return;
            }
        }

        //<------------------------INPUTS-------------------------
        float x_dir = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        controller.Move(x_dir, jump);

        //Respawn player to last checkpoint
        if (Input.GetKeyDown(KeyCode.R))
        {
            controller.respawn(-1);
            transform.localScale = Vector3.one;
            scaleSlider.value = 1;

        }

        //Reset Scene
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(0);
        }
        //-------------------------INPUTS------------------------>

        CheckPlayerFell();

        CheckFinishLine();
    }
    


    void CheckPlayerFell()
    {
        //Kill player for falling
        if (transform.position.y <= -6)
        {
            controller.Kill();
        }
    }
    void CheckFinishLine()
    {
        if (!crossed && transform.position.x > 130)
        {
            crossed = true;
            StartCoroutine(FinishImage());
            StartCoroutine(MakeBigger());
        }
    }

    IEnumerator MakeBigger()
    {
        while (transform.localScale.x < 1)
        {
            transform.localScale += Vector3.one * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FinishImage()
    {
        float currentTime = Time.time;
        finishImage.SetActive(true);
        while (Time.time < currentTime + 5)
        {
            yield return null;
        }
        finishImage.SetActive(false);
        yield return null;

    }

}
