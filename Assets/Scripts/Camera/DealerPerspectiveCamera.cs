using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerPerspectiveCamera : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    public float turnSpeed = 5f;
    public float maxAngleX = 35f;
    public float maxAngleY = 35f;

    float yRotate = -35f;
    float xRotate = -35f;

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        transform.Rotate(0, horizontalInput * turnSpeed * Time.deltaTime, 0);
        transform.Rotate(-verticalInput * turnSpeed * Time.deltaTime, 0, 0);

        //This is used to avoid rotating on the z axis;
        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);

        //Check angle limits for y axis
        if(q.eulerAngles.y > maxAngleY && q.eulerAngles.y < 180){
            yRotate = maxAngleY;
            q.eulerAngles = new Vector3( q.eulerAngles.x, yRotate, 0);
        }
        if(q.eulerAngles.y < 360 - maxAngleY && q.eulerAngles.y > 180){
            yRotate = 360 - maxAngleY;
            q.eulerAngles = new Vector3( q.eulerAngles.x, yRotate, 0);
        }

        //Check angle limits for x axis
        if(q.eulerAngles.x > maxAngleX && q.eulerAngles.x < 180){
            xRotate = maxAngleX;
            q.eulerAngles = new Vector3(xRotate, q.eulerAngles.y, 0);
        }
        if(q.eulerAngles.x < 360 - maxAngleX && q.eulerAngles.x > 180){
            xRotate = 360 - maxAngleX;
            q.eulerAngles = new Vector3(xRotate, q.eulerAngles.y, 0);
        }

        transform.rotation = q;
    }
}
