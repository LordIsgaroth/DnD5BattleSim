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
    [SerializeField] private Tile tileAvalibleForMove;
    [SerializeField] private Tile tileAffected;
    [SerializeField] private Tile tileTarget;

    private Character character;

    private bool attackMode = false;
    private Vector3Int prevCoordinates;
    private Hashtable avalibleMovementTiles;
    private AreaOfEffect attackArea;

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
            if (attackMode) { PerformAttack(hit); }
            else { MoveCharacter(hit); }
        }
    }

    public void StartTurn()
    {
        character = currentCharacter.GetComponent<Character>();
        avalibleMovementTiles = AreaCalculations.GetTilesAvalibleForMovement(character);

        if(attackArea != null)
        {
            ClearTiles(attackArea.AffectedTiles);
            ClearTilesFromHashtable(attackArea.AffectedCharacters);
        }
        
        ShowAvalibleMovementTiles(avalibleMovementTiles);
    }    

    public void SwitchAttackMode()
    {
        if (attackMode)
        {
            attackMode = false;
            avalibleMovementTiles = AreaCalculations.GetTilesAvalibleForMovement(character);
            ClearTiles(attackArea.AffectedTiles);
            ClearTilesFromHashtable(attackArea.AffectedCharacters);
            ShowAvalibleMovementTiles(avalibleMovementTiles);
        }
        else
        {
            attackMode = true;
            ClearAvalibleMovementTiles(avalibleMovementTiles);
            attackArea = AreaCalculations.GetAreaOfEffect(Vector3Int.FloorToInt(character.transform.position), character.AttackRange, character.Team);
            ShowTiles(attackArea.AffectedTiles, tileAffected);
            ShowTilesAtCharacterPositions(attackArea.AffectedCharacters, tileTarget);
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

            ClearAvalibleMovementTiles(avalibleMovementTiles);

            avalibleMovementTiles = AreaCalculations.GetTilesAvalibleForMovement(character);
            ShowAvalibleMovementTiles(avalibleMovementTiles);
        }       
    }

    private void PerformAttack(RaycastHit2D hit)
    {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilemapMousePos = UILayer.WorldToCell(mouseWorldPos);

        if (attackArea.AffectedCharacters.Contains(tilemapMousePos))
        {
            Character targetCharacter = (Character)attackArea.AffectedCharacters[tilemapMousePos];
            Attack attack = character.PerformAttack(targetCharacter.ArmorClass);

            if (attack.Successfull)
            {
                targetCharacter.TakeDamage(attack);
            }
        }       
    }

    void ShowAvalibleMovementTiles(Hashtable avalibleTiles)
    {
        foreach (Vector3Int position in avalibleTiles.Keys)
        {
            UILayer.SetTile(position, tileAvalibleForMove);
        }        
    }

    void ShowTiles(HashSet<Vector3Int> setOfTiles, Tile tileType)
    {
        foreach (Vector3Int position in setOfTiles)
        {
            UILayer.SetTile(position, tileType);
        }
    }

    void ShowTilesAtCharacterPositions(Hashtable characters, Tile tileType)
    {
        foreach (Vector3Int position in characters.Keys)
        {
            UILayer.SetTile(position, tileType);
        }
    }

    void ClearTiles(HashSet<Vector3Int> setOfTiles)
    {
        foreach (Vector3Int position in setOfTiles)
        {
            UILayer.SetTile(position, null);
        }
    }

    void ClearTilesFromHashtable(Hashtable tiles)
    {
        foreach (Vector3Int position in tiles.Keys)
        {
            UILayer.SetTile(position, null);
        }
    }

    void ClearAvalibleMovementTiles(Hashtable avalibleTiles)
    {
        foreach (Vector3Int position in avalibleTiles.Keys)
        {
            UILayer.SetTile(position, null);
        }

        avalibleTiles.Clear();
    }

    void clearTilesAtCharacterPositions(List<Character> characters)
    {
        foreach (Character character in characters)
        {
            UILayer.SetTile(Vector3Int.FloorToInt(character.transform.position), null);
        }
    }
}
