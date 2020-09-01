using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnController : MonoBehaviour
{
    [SerializeField] private GameObject currentCharacter;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap UILayer;
    [SerializeField] private Tilemap CursorLayer;
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Canvas userInterface;
    [SerializeField] private Tile tileSelected;
    [SerializeField] private Tile tileAvalible;

    private Character character;

    private bool attackMode = false;
    private Vector3Int prevCoordinates;
    private Hashtable avalibleMovementTiles;
    private Hashtable avalibleAttackTiles;


    void Start()
    {
        StartTurn();
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

    public void StartTurn()
    {
        character = currentCharacter.GetComponent<Character>();
        avalibleMovementTiles = CharacterMovement.GetAvalibleTiles(character);
        ShowAvalibleTiles(avalibleMovementTiles);
    }

    public void PerformAttack()
    {
        character.Attack();
    }

    public void SwitchAttackMode()
    {
        if (attackMode)
        {
            attackMode = false;
            avalibleMovementTiles = CharacterMovement.GetAvalibleTiles(character);
            ShowAvalibleTiles(avalibleMovementTiles);
        }
        else
        {
            attackMode = true;
            ClearAvalibleTiles(avalibleMovementTiles);
        }    
    }

    void ShowSelectedTile(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilemapMousePos = UILayer.WorldToCell(mouseWorldPos);

            if (CursorLayer.GetTile(tilemapMousePos) == null)
            {
                CursorLayer.SetTile(tilemapMousePos, tileSelected);
            }

            if (tilemapMousePos != prevCoordinates)
            {
                CursorLayer.SetTile(prevCoordinates, null);
            }

            prevCoordinates = tilemapMousePos;
        }
    }

    void MoveCharacter(RaycastHit2D hit)
    {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilemapMousePos = UILayer.WorldToCell(mouseWorldPos);

        if(avalibleMovementTiles.Contains(tilemapMousePos))
        {
            character.Move(tilemapMousePos, 0);
            character.ChangeCurrentSpeedByCost((int)avalibleMovementTiles[tilemapMousePos]);

            Debug.Log("Character moved to: " + tilemapMousePos);

            ClearAvalibleTiles(avalibleMovementTiles);

            avalibleMovementTiles = CharacterMovement.GetAvalibleTiles(character);
            ShowAvalibleTiles(avalibleMovementTiles);
        }       
    }

    void ShowAvalibleTiles(Hashtable avalibleTiles)
    {
        foreach (Vector3Int position in avalibleTiles.Keys)
        {
            UILayer.SetTile(position, tileAvalible);
        }        
    }

    void ClearAvalibleTiles(Hashtable avalibleTiles)
    {
        foreach (Vector3Int position in avalibleTiles.Keys)
        {
            UILayer.SetTile(position, null);
        }

        avalibleTiles.Clear();
    }
}
