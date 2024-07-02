using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraedgeMoving : MonoBehaviour
{
    public float moveSpeed = 5f; // 카메라 이동 속도
    public float boundaryThickness= 10f; // 화면 경계 두께
    public Vector2 panLimit;
    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 moveDirection = Vector3.zero;

        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, initialPosition.x - panLimit.x, initialPosition.x + panLimit.x);
        newPosition.z = Mathf.Clamp(newPosition.z, initialPosition.z - panLimit.y, initialPosition.z + panLimit.y);


        if (mousePosition.x < boundaryThickness)
        {
            moveDirection += Vector3.left;
        }
     
        else if (mousePosition.x > Screen.width - boundaryThickness)
        {
            moveDirection += Vector3.right;
        }

    
        if (mousePosition.y < boundaryThickness)
        {
            moveDirection += Vector3.back; 
        }
       
        else if (mousePosition.y > Screen.height - boundaryThickness)
        {
            moveDirection += Vector3.forward; 
        }

 
        transform.position = newPosition;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    
}
