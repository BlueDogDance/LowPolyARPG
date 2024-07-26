using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class momementScript : MonoBehaviour
{
    public float rotateSpeed;
    public float targetRotation;
    float r;
    public float speed;
    public float defaultSpeed;
    public List<float> speedMods;
    float currentSpeed;
    // Start is called before the first frame update

    void Start()
    {
        speed = defaultSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = defaultSpeed;
        foreach (float mod in speedMods)
        {
            speed *= mod;
        }

        GetComponent<Animator>().SetFloat("WalkSpeed", currentSpeed / 3);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speed * 2;
        }
        else
        {
            currentSpeed = speed;
        }

        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        if(xAxis != 0 || yAxis != 0)
        {
            Vector3 movement = new Vector3((xAxis / Mathf.Sqrt(Mathf.Abs(xAxis) + Mathf.Abs(yAxis))) * currentSpeed, transform.GetComponent<Rigidbody>().velocity.y, (yAxis / Mathf.Sqrt(Mathf.Abs(xAxis) + Mathf.Abs(yAxis))) * currentSpeed);
            //transform.GetComponent<CharacterController>().Move(movement);
            transform.GetComponent<Rigidbody>().velocity = movement;
        }
        

        CalculateRotations(xAxis, yAxis);

        if(xAxis != 0 | yAxis != 0)
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", true);
        } else
        {
            gameObject.GetComponent<Animator>().SetBool("isWalking", false);
        }

        

        
    }

    private void CalculateRotations(float xAxis, float yAxis)
    {
        if (xAxis == 0 && yAxis == 1)
        {
            targetRotation = 0;
        }
        else if (xAxis == 0 && yAxis == -1)
        {
            targetRotation = 180;
        }
        else if (xAxis == 1 && yAxis == 0)
        {
            targetRotation = 90;
        }
        else if (xAxis == 1 && yAxis == 1)
        {
            targetRotation = 45;
        }
        else if (xAxis == 1 && yAxis == -1)
        {
            targetRotation = 135;
        }
        else if (xAxis == -1 && yAxis == 0)
        {
            targetRotation = 270;
        }
        else if (xAxis == -1 && yAxis == 1)
        {
            targetRotation = 315;
        }
        else if (xAxis == -1 && yAxis == -1)
        {
            targetRotation = 225;
        }
        float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation + 90, ref r, rotateSpeed);
        transform.rotation = Quaternion.Euler(0, Angle, 0);
    }
}
