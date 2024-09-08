using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider jumpSlider;
    [SerializeField] private Slider scaleSlider;
    [SerializeField] Canvas _canvas;
    private Camera _camera;

    PlayerController controller;
    void Start()
    {
        _camera = Camera.main;
        controller = GetComponent<PlayerController>();
        speedSlider.onValueChanged.AddListener((v) => { controller.speed = v; });
        jumpSlider.onValueChanged.AddListener((j) => { controller.jumpSpeed = j; });
        scaleSlider.onValueChanged.AddListener((s) => { transform.localScale = Vector3.one * s; });
    }
    bool crossed = false;
    [SerializeField] GameObject finishImage;
    // Update is called once per frame
    void Update()
    {
        _camera.transform.position = transform.localPosition + new Vector3(0, 1, -10);
        _canvas.transform.position = transform.localPosition;
        float x_dir = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        controller.Move(x_dir, jump);

        if (transform.position.y <= -6)
        {
            controller.Kill();
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            controller.respawn(-1);
            
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(0);

        }

        if (transform.position.x > 130 && !crossed)
        {
            crossed = true;
            finishImage.SetActive(true);
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

}
