using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnController : MonoBehaviour
{
    [SerializeField] private GameObject currentCharacter;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap UILayer;
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Canvas userInterface;
    [SerializeField] private Tile tileSelected;

    void Start()
    {
        //UILayer.SetTile(new Vector3Int(0, 0, -1), tileSelected);
    }

    Vector3Int prevCoordinates;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilemapMousePos = UILayer.WorldToCell(mouseWorldPos);
            
            if (UILayer.GetTile(tilemapMousePos) == null)
            {
                UILayer.SetTile(tilemapMousePos, tileSelected);
            }

            if (tilemapMousePos != prevCoordinates)
            {
                UILayer.SetTile(prevCoordinates, null);
            }

            prevCoordinates = tilemapMousePos;
        }
    }
}