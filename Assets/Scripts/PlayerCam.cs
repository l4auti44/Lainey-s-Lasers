using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [Header("Mouse sensibility")]
    public float sensX_mouse;
    public float sensY_mouse;

    [Header("Controller sensibility")]
    public float sensX_cont;
    public float sensY_cont;

    private float sensX;
    private float sensY;

    public Transform orientantion;

    float xRotation;
    float yRotation;
    private Vector2 inputMove;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = inputMove.x * Time.deltaTime * sensX;
        float mouseY = inputMove.y * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotate cam and oritation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientantion.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void OnLook(InputValue inputValue)
    {
        inputMove = inputValue.Get<Vector2>();
        sensX = sensX_mouse;
        sensY = sensY_mouse;
        
    }

    private void OnLookWithController(InputValue inputValue) 
    {
        inputMove = inputValue.Get<Vector2>();
        sensX = sensX_cont;
        sensY = sensY_cont;
    }

}
