using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public static class MovementController
{
    //Moves the character along the created path
    public static void MoveAlongPath(CharacterInfo character, List<OverlayTile> path)
    {
        var step = character.speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        Vector2 offset = new Vector2(path[0].transform.position.x + character.xCenter, path[0].transform.position.y + character.yCenter);
        character.transform.position = Vector2.MoveTowards(character.transform.position, offset, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, offset) < 0.00001f)
        {
            PositionCharacterOnLine(character, path[0]);
            path.RemoveAt(0);
        }

    }

    //Positions the character directly center of the tile
    public static void PositionCharacterOnLine(CharacterInfo character, OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x + character.xCenter, tile.transform.position.y + character.yCenter, tile.transform.position.z - 0.1f);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile.isBlocked = false;
        character.activeTile = tile;
        character.activeTile.isBlocked = true;
    }

    //Raycasts from position and collects the tile below the position
    public static OverlayTile GetTileFromPos(Vector2 pos)
    {
        //CellToWorld() //WorldToCell()
        Vector3Int cellPosition = MapManager.Instance.tileMap.WorldToCell(pos);
        //Vector3 worldPosition = MapManager.Instance.tileMap.CellToWorld(cellPosition);
        //Debug.Log(cellPosition);
        //Debug.Log(worldPosition);
        //Debug.Log(MapManager.Instance.map[(Vector2Int)cellPosition]);
        //MapManager.Instance.map[(Vector2Int)cellPosition].ShowTile();
        return MapManager.Instance.map[(Vector2Int)cellPosition];
    }

    public static OverlayTile GetBestTile(OverlayTile tile, Transform characterPos){

        List<OverlayTile> neighbors = MovementController.GetCardinalNeighbors(tile);

        float prevDist = float.MaxValue;
        tile = neighbors[0];

        foreach(OverlayTile checkTile in neighbors){
            float distance = Vector3.Distance(tile.worldPosition, checkTile.worldPosition);
            checkTile.ShowTile();
            if(prevDist > distance){
                
                prevDist = distance;
                Debug.Log(checkTile.gridLocation);
                tile = checkTile;
            }
        }
        Debug.Log(tile);
        return tile;
    }

    public static List<OverlayTile> GetCardinalNeighbors(OverlayTile currentOverlayTile){
        var map = MapManager.Instance.map;

        List<OverlayTile> neighbors = new List<OverlayTile>();

        //Top
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);
        if (map.ContainsKey(locationToCheck) && !map[locationToCheck].isBlocked)
        {
            neighbors.Add(map[locationToCheck]);
        }        
        //Bottom
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);
        if (map.ContainsKey(locationToCheck) && !map[locationToCheck].isBlocked)
        {
            neighbors.Add(map[locationToCheck]);
        }
        //Right
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);
        if (map.ContainsKey(locationToCheck) && !map[locationToCheck].isBlocked)
        {
            neighbors.Add(map[locationToCheck]);
        }
        //Top
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);
        if (map.ContainsKey(locationToCheck) && !map[locationToCheck].isBlocked)
        {
            neighbors.Add(map[locationToCheck]);
        }

        return neighbors;
    }
}
