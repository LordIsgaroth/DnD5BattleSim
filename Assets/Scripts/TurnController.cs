using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnController : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap UILayer;
    [SerializeField] private Tilemap CursorLayer;
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Canvas userInterface;
    [SerializeField] private Tile tileCurrentCharacter;
    [SerializeField] private Tile tileSelected;
    [SerializeField] private Tile tileAvailableForMove;
    [SerializeField] private Tile tileAffected;
    [SerializeField] private Tile tileTarget;

    private GameController gameController;
    private Character currentCharacter = null;

    private bool attackMode = false;
    private Vector3Int prevCoordinates;
    private Hashtable avalibleMovementTiles;
    private AreaOfEffect attackArea;

    private StringEvent logEvent;

    void Awake()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        logEvent = new StringEvent();
    }

    void Start()
    {
        LogWindow logWindow = GameObject.Find("LogWindow").GetComponent<LogWindow>();
        logEvent.AddListener(logWindow.DisplayIntoLogWindow);

        StartTurn();
    }

    void Update()
    {
        if (!gameController.GamePaused)
        {
            RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

            ShowSelectedTile(hit);

            if (Input.GetMouseButtonDown(0))
            {
                if (attackMode) { PerformAttack(hit); }
                else { MoveCharacter(hit); }
            }
        }
    }

    public void StartTurn()
    {
        clearAllTiles();

        if (currentCharacter != null)
        {
            clearTileOnPosition(Vector3Int.FloorToInt(currentCharacter.transform.position));
        }

        currentCharacter = gameController.GetNextCharacter(currentCharacter);

        setTileOnPosition(Vector3Int.FloorToInt(currentCharacter.transform.position), tileCurrentCharacter);

        attackMode = false;

        avalibleMovementTiles = AreaCalculations.GetTilesAvalibleForMovement(currentCharacter);
        ShowTilesFromHashtable(avalibleMovementTiles, tileAvailableForMove);
    }    

    public void SwitchAttackMode()
    {
        if (attackMode)
        {
            attackMode = false;
            avalibleMovementTiles = AreaCalculations.GetTilesAvalibleForMovement(currentCharacter);

            if (attackArea != null)
            {
                ClearTiles(attackArea.AffectedTiles);
                ClearTilesFromHashtable(attackArea.AffectedCharacters);
            }

            ShowTilesFromHashtable(avalibleMovementTiles, tileAvailableForMove);
        }
        else
        {
            attackMode = true;
            ClearTilesFromHashtable(avalibleMovementTiles);

            if (currentCharacter.ActionAvailable)
            {
                attackArea = AreaCalculations.GetAreaOfEffect(Vector3Int.FloorToInt(currentCharacter.transform.position), currentCharacter.AttackRange, currentCharacter.Team);
                ShowTiles(attackArea.AffectedTiles, tileAffected);
                ShowTilesFromHashtable(attackArea.AffectedCharacters, tileTarget);
            }            
        }    
    }

    private void ShowSelectedTile(RaycastHit2D hit)
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

    private void MoveCharacter(RaycastHit2D hit)
    {
        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilemapMousePos = UILayer.WorldToCell(mouseWorldPos);

        if(avalibleMovementTiles.Contains(tilemapMousePos))
        {
            clearTileOnPosition(Vector3Int.FloorToInt(currentCharacter.transform.position));

            currentCharacter.Move(tilemapMousePos, 0);
            currentCharacter.ChangeCurrentSpeedByCost((int)avalibleMovementTiles[tilemapMousePos]);

            logEvent.Invoke(currentCharacter.name + " moved to: " + tilemapMousePos);

            ClearTilesFromHashtable(avalibleMovementTiles);

            setTileOnPosition(Vector3Int.FloorToInt(currentCharacter.transform.position), tileCurrentCharacter);

            avalibleMovementTiles = AreaCalculations.GetTilesAvalibleForMovement(currentCharacter);
            ShowTilesFromHashtable(avalibleMovementTiles, tileAvailableForMove);
        }       
    }

    private void PerformAttack(RaycastHit2D hit)
    {
        if (attackArea == null) return;

        Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilemapMousePos = UILayer.WorldToCell(mouseWorldPos);

        if (attackArea.AffectedCharacters.Contains(tilemapMousePos))
        {
            Character targetCharacter = (Character)attackArea.AffectedCharacters[tilemapMousePos];
            Attack attack = currentCharacter.PerformMainHandAttack(targetCharacter.ArmorClass);

            int hitValue = attack.HitRoll(Roll20Type.Normal); //В будущем добавить атаки с преимуществом/помехой в соответствии с необходимыми условиями

            logEvent.Invoke(currentCharacter.name + " attacks " + targetCharacter.name);
            
            if (attack.isCriticalFail)
            {
                logEvent.Invoke("Critical fail!");
            }
            else if (attack.isCriticalSuccess)
            {
                logEvent.Invoke("Critical hit!");
                int damage = attack.DamageRoll(true);
                targetCharacter.TakeDamage(damage);
                logEvent.Invoke(targetCharacter.name + " takes " + damage + " damage");
            }
            else if (hitValue >= targetCharacter.ArmorClass)
            {
                logEvent.Invoke("Hit value: " + hitValue);
                int damage = attack.DamageRoll();
                targetCharacter.TakeDamage(damage);
                logEvent.Invoke(targetCharacter.name + " takes " + damage + " damage");
            }
            else
            {
                logEvent.Invoke("Hit value: " + hitValue);
                logEvent.Invoke(currentCharacter.name + " misses");
            }

            ClearTiles(attackArea.AffectedTiles);
            ClearTilesFromHashtable(attackArea.AffectedCharacters);

            attackArea = null;
        }
    }

    private void ShowTilesFromHashtable(Hashtable Table, Tile tileType)
    {
        foreach (Vector3Int position in Table.Keys)
        {
            UILayer.SetTile(position, tileType);
        }        
    }

    private void ClearTilesFromHashtable(Hashtable tiles)
    {
        foreach (Vector3Int position in tiles.Keys)
        {
            UILayer.SetTile(position, null);
        }
    }

    private void ShowTiles(HashSet<Vector3Int> setOfTiles, Tile tileType)
    {
        foreach (Vector3Int position in setOfTiles)
        {
            UILayer.SetTile(position, tileType);
        }
    }

    private void ClearTiles(HashSet<Vector3Int> setOfTiles)
    {
        foreach (Vector3Int position in setOfTiles)
        {
            UILayer.SetTile(position, null);
        }
    }

    private void clearAllTiles()
    {
        if(avalibleMovementTiles != null)
        {
            ClearTilesFromHashtable(avalibleMovementTiles);
        }
        
        if (attackArea != null)
        {
            ClearTiles(attackArea.AffectedTiles);
            ClearTilesFromHashtable(attackArea.AffectedCharacters);
        }            
    }

    private void setTileOnPosition(Vector3Int position, Tile tileType)
    {
        UILayer.SetTile(position, tileType);
    }

    private void clearTileOnPosition(Vector3Int position)
    {
        UILayer.SetTile(position, null);
    }
}
