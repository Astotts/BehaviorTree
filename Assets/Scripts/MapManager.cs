using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class MapManager : MonoBehaviour
{
    //Singleton
    private static MapManager _instance;
    public static MapManager Instance{get { return _instance; }}

    public int otCount = 1;

    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;
    public GameObject floorMap;
    public GameObject objectMap;

    public Dictionary<Vector2Int, OverlayTile> map;

    public Tilemap tileMap;
    public Tilemap blockingMap;
    public BoundsInt bounds;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }

        tileMap = floorMap.GetComponent<Tilemap>();
        blockingMap = objectMap.GetComponent<Tilemap>();
        map = new Dictionary<Vector2Int, OverlayTile>();
        bounds = tileMap.cellBounds;

        for (int z = bounds.max.z; z > bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);

                    if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey) && !blockingMap.HasTile(tileLocation))
                    {
                        var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        //Names the OverlayTiles
                        overlayTile.name = "OverlayTile-" + otCount;
                        otCount++;

                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = floorMap.GetComponent<TilemapRenderer>().sortingOrder;

                        overlayTile.gridLocation = tileLocation;
                        overlayTile.worldPosition = cellWorldPosition;

                        map.Add(tileKey, overlayTile);
                    }
                }
            }
        }
    }
}
