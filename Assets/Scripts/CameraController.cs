using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CameraController : MonoBehaviour
{
    [SerializeField] private Camera camera;

    [Range(0f, 10f)] public float moveSpeed = 10f;
    [Range(0f, 5f)] public float sensitivity = 3f;

    private Vector2 tempCenter, targetDirection, tempMousePos;

    public bool isDragging { get; private set; }

    private void Update()
    {
        UpdateInput();
        UpdatePosition();
    }

    private void UpdateInput()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (Input.GetMouseButtonDown(1)) OnPointDown(mousePosition);
        else if (Input.GetMouseButtonUp(1)) OnPointUp(mousePosition);
        else if (Input.GetMouseButton(1)) OnPointMove(mousePosition);
    }

    private void UpdatePosition()
    {
        float speed = Time.deltaTime * this.moveSpeed;
        float tempSens = 0f;

        if (this.isDragging)
        {
            tempSens = this.sensitivity;
        }
        else 
        { 
            tempSens = Mathf.Lerp(tempSens, 0f, speed);
        }

        Vector3 newPosition = camera.transform.position + (Vector3)this.targetDirection * tempSens;
        camera.transform.position = Vector3.Lerp(camera.transform.position, newPosition, speed);
    }

    private void OnPointDown(Vector2 mousePosition) 
    {
        this.tempCenter = GetWorldPoint(mousePosition);
        this.targetDirection = Vector2.zero;
        this.tempMousePos = mousePosition;
        this.isDragging = true;
    }

    private void OnPointMove(Vector2 mousePosition)
    {
        if (this.isDragging)
        {
            Vector2 point = GetWorldPoint(mousePosition);

            float sqrDist = (this.tempCenter - point).sqrMagnitude;

            if (sqrDist > 0.1f)
            {
                this.targetDirection = (this.tempMousePos - mousePosition).normalized;
                this.tempMousePos = mousePosition;
            }
        }
    }

    private void OnPointUp(Vector2 mousePosition) 
    {
        this.isDragging = false;
    }

    private Vector2 GetWorldPoint(Vector2 mousePosition)
    {
        Vector2 point = Vector2.zero;

        Ray ray = camera.ScreenPointToRay(mousePosition);

        Vector3 normal = Vector3.forward;
        Vector3 position = Vector3.zero;
        Plane plane = new Plane(normal, position);

        float distance;
        plane.Raycast(ray, out distance);
        point = ray.GetPoint(distance);

        return point;
    }
}
