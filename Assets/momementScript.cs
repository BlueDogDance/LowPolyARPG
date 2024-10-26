using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class momementScript : MonoBehaviour
{
    public float rotateSpeed;
    public float targetRotation;
    float r;
    private float speed;
    public float defaultSpeed;
    public float acceleration;
    public List<float> speedMods;
    //public Transform Camera;
    public Transform orientation;
    public float fallSpeed;
    float currentSpeed;
    float verticalVelocity;
    public bool canMove;
    public float dashSpeed;
    float dashTimeLeft;
    // Start is called before the first frame update

    void Start()
    {
        speed = defaultSpeed;
        verticalVelocity = -0.5f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimeLeft <= 0)
        {
            dashTimeLeft = 0.5f;
        }
        dashTimeLeft -= Time.deltaTime;

        if (dashTimeLeft > 0)
        {
            canMove = false;
            Vector3 movement = new Vector3();
            movement = transform.right * -1 * dashSpeed;
            transform.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
            GetComponent<Animator>().SetBool("IsDashing", true);
        }
        else
        {
            canMove = true;
            GetComponent<Animator>().SetBool("IsDashing", false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verticalVelocity = -1 * fallSpeed;

        if (canMove)
        {
            orientation.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);

            speed = defaultSpeed;
            foreach (float mod in speedMods)
            {
                speed *= mod;
            }

            GetComponent<Animator>().SetFloat("WalkSpeed", currentSpeed / 3);


            float xAxis = Input.GetAxisRaw("Horizontal");
            float yAxis = Input.GetAxisRaw("Vertical");

            if (xAxis != 0 || yAxis != 0)
            {
                if (currentSpeed + acceleration > currentSpeed)
                {
                    currentSpeed = speed;
                }
                else
                {
                    currentSpeed += acceleration;
                }
            }
            else
            {
                currentSpeed = 0;
            }

            if (xAxis != 0 || yAxis != 0)
            {
                Vector3 movement = new Vector3((xAxis / Mathf.Sqrt(Mathf.Abs(xAxis) + Mathf.Abs(yAxis))), 0, (yAxis / Mathf.Sqrt(Mathf.Abs(xAxis) + Mathf.Abs(yAxis))));
                movement = orientation.forward * movement.z + orientation.right * movement.x;
                movement = new Vector3(movement.x, 0, movement.z);
                transform.GetComponent<CharacterController>().Move(movement * currentSpeed + new Vector3(0,verticalVelocity,0));
                //transform.GetComponent<Rigidbody>().velocity = movement * currentSpeed;
                CalculateRotations(xAxis, yAxis);
            }
            else
            {
                transform.GetComponent<CharacterController>().Move(new Vector3(0, verticalVelocity, 0));
            }




            if (xAxis != 0 | yAxis != 0)
            {
                gameObject.GetComponent<Animator>().SetBool("isWalking", true);
            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            }

        }
    }

    private void CalculateRotations(float xAxis, float yAxis)
    {
        targetRotation = Mathf.Atan2(xAxis, yAxis) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, orientation.rotation.y + targetRotation + 90, ref r, rotateSpeed);
        transform.rotation = Quaternion.Euler(0, Angle, 0);
    }



}
