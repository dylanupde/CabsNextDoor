using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public Vector3 currentFacingVector;
    [HideInInspector] public bool upIsPressed, downIsPressed, leftIsPressed, rightIsPressed, jumpIsPressed, attackIsPressed, isRunning, moveButtonIsPressed;

    public bool isPlayer2 = false;

    Transform cameraTransform;
    Vector3 rightMovementVector, upMovementVector;
    KeyCode leftButton, rightButton, upButton, downButton, attackButton;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        rightMovementVector = cameraTransform.right.normalized;
        upMovementVector = cameraTransform.forward;
        upMovementVector.y = 0f;
        upMovementVector = upMovementVector.normalized;

        currentFacingVector = rightMovementVector;

        if (!isPlayer2)
        {
            leftButton = KeyCode.A;
            rightButton = KeyCode.D;
            upButton = KeyCode.W;
            downButton = KeyCode.S;
            attackButton = KeyCode.Space;
        }
        else
        {
            leftButton = KeyCode.Keypad4;
            rightButton = KeyCode.Keypad6;
            upButton = KeyCode.Keypad8;
            downButton = KeyCode.Keypad5;
            attackButton = KeyCode.Keypad0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInput();
    }




    private void GetPlayerInput()
    {
        if (Input.GetKey(leftButton)) leftIsPressed = true;
        if (Input.GetKey(rightButton)) rightIsPressed = true;
        if (Input.GetKey(upButton)) upIsPressed = true;
        if (Input.GetKey(downButton)) downIsPressed = true;
        if (Input.GetKey(attackButton)) attackIsPressed = true;
    }


    public void GetCurrentFacingDirection()
    {
        isRunning = true;

        if (leftIsPressed)
        {
            if (upIsPressed)
            {
                currentFacingVector = Vector3.Slerp(upMovementVector, -rightMovementVector, 0.5f);
            }
            else if (downIsPressed)
            {
                currentFacingVector = Vector3.Slerp(-rightMovementVector, -upMovementVector, 0.5f);
            }
            else
            {
                currentFacingVector = -rightMovementVector;
            }
        }
        else if (rightIsPressed)
        {
            if (upIsPressed)
            {
                currentFacingVector = Vector3.Slerp(upMovementVector, rightMovementVector, 0.5f);
            }
            else if (downIsPressed)
            {
                currentFacingVector = Vector3.Slerp(rightMovementVector, -upMovementVector, 0.5f);
            }
            else
            {
                currentFacingVector = rightMovementVector;
            }
        }
        else if (upIsPressed)
        {
            currentFacingVector = upMovementVector;
        }
        else if (downIsPressed)
        {
            currentFacingVector = -upMovementVector;
        }
        else
        {
            isRunning = false;
        }

        moveButtonIsPressed = upIsPressed || downIsPressed || leftIsPressed || rightIsPressed;
        currentFacingVector = currentFacingVector.normalized;
    }


    public void ResetPlayerInput()
    {
        leftIsPressed = false;
        rightIsPressed = false;
        upIsPressed = false;
        downIsPressed = false;
        jumpIsPressed = false;
        attackIsPressed = false;
    }
}
