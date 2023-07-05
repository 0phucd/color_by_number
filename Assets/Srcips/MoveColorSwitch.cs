using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveColorSwitch : MonoBehaviour
{
   
    public float dragSpeed = 2f; // Tốc độ di chuyển camera khi kéo

 

    void Start()
    {
       
    }

    void Update()
    {
       
        // Kéo để di chuyển
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved &&  Input.GetTouch(0).position.y <= 500 )
        {
            Vector3 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            gameObject.transform.Translate(touchDeltaPosition.x * dragSpeed * Time.deltaTime, 0, 0);
           
        }
    }
}