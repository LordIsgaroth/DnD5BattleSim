using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnController : MonoBehaviour
{
    public GameObject currentCharacter;
    public Grid grid;
    public Tilemap UILayer;
    public Tile tileSelected;
        
    void Start()
    {
        UILayer.SetTile(new Vector3Int(0, 0, -1), tileSelected);
    }

    Vector3Int previousCoordinates;

    // Update is called once per frame
    void Update()
    {
        Vector3Int mousePosition = Vector3Int.CeilToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector3 mousePosition1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int tileCoordinates = UILayer.WorldToCell(new Vector3(mousePosition.x - 1, mousePosition.y - 1, mousePosition.z));
        Vector3 tileCoordinates1 = UILayer.WorldToLocal(mousePosition1);

        UILayer.SetTile(tileCoordinates, tileSelected);

        if (previousCoordinates != tileCoordinates)
        {
            UILayer.SetTile(previousCoordinates, null);
        }
        previousCoordinates = tileCoordinates;

        if (Input.GetMouseButtonDown(0))
        {
            currentCharacter.transform.position = tileCoordinates;
            Debug.Log("!");
            Debug.Log(currentCharacter.transform.position);
            Debug.Log("!");
        }            
    }
}