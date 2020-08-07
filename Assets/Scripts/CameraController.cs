using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera camera;

    private Vector3 MousePosition;
    private float angle;
    public float sensitivity = 1F;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition = Input.mousePosition;
    }

    void FixedUpdate()
    {
        /*angle = sensitivity * ((MousePosition.x - (Screen.width / 2)) / Screen.width);
        camera.transform.RotateAround(camera.transform.position, camera.transform.up, angle);
        angle = sensitivity * ((MousePosition.y - (Screen.height / 2)) / Screen.height);
        camera.transform.RotateAround(camera.transform.position, camera.transform.right, -angle);*/
    }
}
