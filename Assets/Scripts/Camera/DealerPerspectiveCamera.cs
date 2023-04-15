using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerPerspectiveCamera : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    public float turnSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        transform.Rotate(0, horizontalInput * turnSpeed * Time.deltaTime, 0);
        transform.Rotate(-verticalInput * turnSpeed * Time.deltaTime, 0, 0);
        
        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
        transform.rotation = q;
    }
}
