using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraedgeMoving : MonoBehaviour
{
    public float moveSpeed = 5f; // 카메라 이동 속도
    public float boundaryThickness = 10f; // 화면 경계 두께

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 moveDirection = Vector3.zero;

        
        if (mousePosition.x < boundaryThickness && mousePosition.y < boundaryThickness)
        {
            moveDirection += new Vector3(-1, 0, -1).normalized; 
        }
        
        else if (mousePosition.x > Screen.width - boundaryThickness && mousePosition.y < boundaryThickness)
        {
            moveDirection += new Vector3(1, 0, -1).normalized;
        }
       
        else if (mousePosition.x < boundaryThickness && mousePosition.y > Screen.height - boundaryThickness)
        {
            moveDirection += new Vector3(-1, 0, 1).normalized; 
        }
       
        else if (mousePosition.x > Screen.width - boundaryThickness && mousePosition.y > Screen.height - boundaryThickness)
        {
            moveDirection += new Vector3(1, 0, 1).normalized; 
        }
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
        
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    
}
