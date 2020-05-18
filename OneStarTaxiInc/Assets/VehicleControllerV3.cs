using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControllerV3 : MonoBehaviour
{
    [SerializeField] SphereCollider[] wheelColls;
    [SerializeField] Transform centerOfMassTransform;
    [SerializeField] PhysicMaterial zeroFrictionMaterial;
    [SerializeField] PhysicMaterial highFrictionMaterial;
    [SerializeField] float acceleration = 50f;
    [SerializeField] float brakeForce = 50f;
    [SerializeField] float turnSpeed = 1f;
    [SerializeField] float maxSpeed = 9999999f;
    [SerializeField] float wheelGroundCheckDist = 0.1f;
    [SerializeField] float rotateBackUpSpeed = 5f;
    [SerializeField] float degreesOffToRotateBack = 30f;
    [SerializeField] float groundTestDist = 0.5f;

    Rigidbody rigidBody;
    PlayerInput playerInput;
    int layerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMassTransform.localPosition;
        layerMask = LayerMask.GetMask("Ground", "Wall");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerInput.GetCurrentFacingDirection();
        float numOfWheelsOnTheGround = HowManyWheelsAreOnTheGround();

        if (numOfWheelsOnTheGround >= 2)
        {
            if (playerInput.moveButtonIsPressed)
            {
                SetAllWheelsToPhysMat(zeroFrictionMaterial);

                Vector2 flattenedVelocityVector = new Vector2(rigidBody.velocity.x, rigidBody.velocity.z);
                if (flattenedVelocityVector.magnitude <= maxSpeed)
                {

                    RaycastHit raycastHit;
                    if (Physics.Raycast(transform.position, -transform.up, out raycastHit, groundTestDist))
                    {
                        Vector3 desiredMoveVector = (Vector3.ProjectOnPlane(playerInput.currentFacingVector, raycastHit.normal)).normalized;
                        rigidBody.AddForce(desiredMoveVector * acceleration);
                    }
                    else
                    {
                        rigidBody.AddForce(playerInput.currentFacingVector * acceleration);
                    }
                }
            }
            else
            {
                SetAllWheelsToPhysMat(highFrictionMaterial);
            }
        }

        if (numOfWheelsOnTheGround >= 3) RotateToVelocityDirection();
        RotateBackToNormal();

        playerInput.ResetPlayerInput();
    }




    private void SetAllWheelsToPhysMat(PhysicMaterial inputPhysMat)
    {
        foreach (SphereCollider thisColl in wheelColls)
        {
            thisColl.material = inputPhysMat;
        }
    }
    

    private void RotateToVelocityDirection()
    {
        Vector2 flattenedForwardVector = new Vector2(transform.forward.x, transform.forward.z);
        Vector2 flattenedVelocityVector = new Vector2(rigidBody.velocity.x, rigidBody.velocity.z);
        float angle = Vector2.SignedAngle(flattenedForwardVector, flattenedVelocityVector);

        if (flattenedVelocityVector.magnitude > 1.5f)
        {
            if (angle > 0f)
            {
                transform.Rotate(0f, Mathf.Clamp(-turnSpeed, -Mathf.Abs(angle), Mathf.Abs(angle)), 0f, Space.World);
            }
            else
            {
                transform.Rotate(0f, Mathf.Clamp(turnSpeed, -Mathf.Abs(angle), Mathf.Abs(angle)), 0f, Space.World);
            }
        }
    }


    private int HowManyWheelsAreOnTheGround()
    {
        int numOfWheelsOnTheGround = 0;

        foreach (SphereCollider thisColl in wheelColls)
        {
            RaycastHit raycastHit;

            if (Physics.SphereCast(thisColl.transform.position, (thisColl.radius * thisColl.transform.lossyScale.z) - (wheelGroundCheckDist * 0.25f), -transform.up, out raycastHit, wheelGroundCheckDist))
            {
                numOfWheelsOnTheGround++;
            }
        }

        return numOfWheelsOnTheGround;
    }



    private void RotateBackToNormal()
    {
        float adjustedRotZ = transform.localEulerAngles.z % 360f;
        if (adjustedRotZ < 0f) adjustedRotZ += 360f;

        if (adjustedRotZ >= 0f && adjustedRotZ < 180f)
        {
            if (adjustedRotZ > degreesOffToRotateBack)
            {
                Debug.Log("I should rotate right cuz adjustedrotz is " + adjustedRotZ);
                transform.Rotate(0f, 0f, Mathf.Clamp(-rotateBackUpSpeed, -Mathf.Abs(adjustedRotZ), Mathf.Abs(adjustedRotZ)), Space.Self);
            }

        }
        else
        {
            float degreesAwayFromPerfection = 360 - adjustedRotZ;
            if (degreesAwayFromPerfection > degreesOffToRotateBack)
            {
                Debug.Log("I should rotate left cuz degrees away is " + degreesAwayFromPerfection + " and adjustedrotz is " + adjustedRotZ);
                transform.Rotate(0f, 0f, Mathf.Clamp(rotateBackUpSpeed, -Mathf.Abs(degreesAwayFromPerfection), Mathf.Abs(degreesAwayFromPerfection)), Space.Self);
            }
        }
    }
}
