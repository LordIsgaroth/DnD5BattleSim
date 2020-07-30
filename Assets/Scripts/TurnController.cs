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
    [SerializeField] private Tile tileAvalible;

    private Character character;

    private Vector3Int prevCoordinates;

    void Start()
    {
        character = currentCharacter.GetComponent<Character>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

        ShowSelectedTile(hit);

        if (Input.GetMouseButtonDown(0))
        {
            MoveCharacter(hit);
        }
    }

    void ShowSelectedTile(RaycastHit2D hit)
    {
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

    void MoveCharacter(RaycastHit2D hit)
    {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilemapMousePos = UILayer.WorldToCell(mouseWorldPos);

        character.Move(tilemapMousePos, 0);

        Debug.Log("Character moved to: " + tilemapMousePos);

        Hashtable avalibleTiles = CharacterMovement.GetAvalibleTiles(tilemapMousePos, character.CurrentSpeed, 1);
        ShowAvalibleTiles(avalibleTiles);
    }

    void ShowAvalibleTiles(Hashtable avalibleTiles)
    {
        foreach (Vector3Int position in avalibleTiles.Keys)
        {
            UILayer.SetTile(position, tileAvalible);
        }        
    }
}
