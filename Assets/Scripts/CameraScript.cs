using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float cam_speed = 15.0f;
    /*private float scroll_speed = 100.0f;*/
    private float screenHeight = Screen.height / 11;
    private float screenWidth = Screen.width / 11;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Reusing camera code from B1
        Vector3 position = Camera.main.transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            position.z += cam_speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            position.z -= cam_speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position.x += cam_speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= cam_speed * Time.deltaTime;
        }
        /* Commenting out this part of the camera script for now
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * scroll_speed * Time.deltaTime;*/

        Camera.main.transform.position = position;
    }
}
