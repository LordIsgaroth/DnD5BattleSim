using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect
{
    private HashSet<Vector3Int> tiles = new HashSet<Vector3Int>();
    private List<Character> characters = new List<Character>();

    public AreaOfEffect(HashSet<Vector3Int> tiles, List<Character> characters)
    {
        this.tiles = tiles;
        this.characters = characters;
    }

    public HashSet<Vector3Int> Tiles { get { return tiles; } }
    public List<Character> Characters { get { return characters; } }
}
