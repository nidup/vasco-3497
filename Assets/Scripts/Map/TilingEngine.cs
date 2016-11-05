using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Lean;

/**
 * Tiling Engine inspired by the following tutorial https://gamedevacademy.org/how-to-script-a-2d-tile-map-in-unity3d/
 */
public class TilingEngine : MonoBehaviour
{
    public string tilesetSpriteName;
    public Vector2 mapSize;
    public GameObject tileContainerPrefab;
    public GameObject tilePrefab;
    public Vector2 currentPosition;
    public Vector2 viewPortSize;

    private TileSprite[,] _map;
    private GameObject _tileContainer;
    private List<GameObject> _tiles = new List<GameObject>();
    private Sprite[] sprites;

    public void Awake()
    {
        sprites = Resources.LoadAll<Sprite>(tilesetSpriteName);
    }

    public void Start()
    {
        _map = new TileSprite[(int)mapSize.x, (int)mapSize.y];

        SetTiles();
    }

    public void Update()
    {
        AddTilesToWorld();
    }

    private void SetTiles()
    {
        int width = (int) mapSize.x;
        int height = (int) mapSize.y;

        BaseTilesGenerator baseGenerator = new BaseTilesGenerator();
        int [,] tiles = baseGenerator.Generate(width, height);

        SmoothTilesGenerator smoothGenerator = new SmoothTilesGenerator();
        tiles = smoothGenerator.Generate(tiles, width, height);

        //FinalTilesGenerator finalGenerator = new FinalTilesGenerator();
        //tiles = finalGenerator.Generate(tiles, width, height);

        var index = 0;
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                index = tiles[x, y];
                _map[x, y] = new TileSprite(sprites[index]);
            }
        }
    }

    private void AddTilesToWorld()
    {
        foreach (GameObject o in _tiles) {
            LeanPool.Despawn(o);
        }
        _tiles.Clear();
        LeanPool.Despawn(_tileContainer);
        _tileContainer = LeanPool.Spawn(tileContainerPrefab);
        var tileSize = .64f;
        var viewOffsetX = viewPortSize.x/2f;
        var viewOffsetY = viewPortSize.y/2f;
        for (var y = -viewOffsetY; y < viewOffsetY; y++) {
            for (var x = -viewOffsetX; x < viewOffsetX; x++) {
                var tX = x*tileSize;
                var tY = y*tileSize;

                var iX = x + currentPosition.x;
                var iY = y + currentPosition.y;

                if (iX < 0) continue;
                if (iY < 0) continue;
                if(iX > mapSize.x - 2) continue;
                if (iY > mapSize.y - 2) continue;

                var t = LeanPool.Spawn(tilePrefab);
                t.transform.position = new Vector3(tX, tY, 0);
                t.transform.SetParent(_tileContainer.transform);
                var renderer = t.GetComponent<SpriteRenderer>();
                renderer.sprite = _map[(int)x + (int)currentPosition.x, (int)y + (int)currentPosition.y].sprite;
                _tiles.Add(t);
            }
        }
    }
}