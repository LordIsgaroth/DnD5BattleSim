using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect
{
    private readonly HashSet<Vector3Int> affectedTiles = new HashSet<Vector3Int>();
    private readonly Hashtable affectedCharacters = new Hashtable();

    public AreaOfEffect(HashSet<Vector3Int> tiles, Hashtable characters)
    {
        this.affectedTiles = tiles;
        this.affectedCharacters = characters;
    }

    public HashSet<Vector3Int> AffectedTiles { get { return affectedTiles; } }
    public Hashtable AffectedCharacters { get { return affectedCharacters; } }
}
