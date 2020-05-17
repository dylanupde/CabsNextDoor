using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControllerV3 : MonoBehaviour
{
    [SerializeField] float acceleration = 50f;
    [SerializeField] float brakeForce = 50f;

    Rigidbody rigidBody;
    PlayerInput playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerInput.GetCurrentFacingDirection();

        if (playerInput.moveButtonIsPressed)
        {
            Debug.Log("Adding force");
            rigidBody.AddForce(playerInput.currentFacingVector * acceleration);
        }


        playerInput.ResetPlayerInput();
    }
}
