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

        GenerateTiles();
    }

    public void Update()
    {
        AddTilesToWorld();
        //AddAllTilesToWorld();
    }

    private void GenerateTiles()
    {
        // TODO Fix seed for debug purpose
        Random.InitState (42);

        int width = (int) mapSize.x;
        int height = (int) mapSize.y;

        BaseTilesGenerator baseGenerator = new BaseTilesGenerator();
        int [,] tiles = baseGenerator.Generate(width, height);

        SmoothTilesGenerator smoothGenerator = new SmoothTilesGenerator();
        tiles = smoothGenerator.Generate(tiles, width, height);

        FinalTilesGenerator finalGenerator = new FinalTilesGenerator();
        tiles = finalGenerator.Generate(tiles, width, height);

        var index = 0;
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                index = tiles[x, y];
                _map[x, y] = new TileSprite(sprites[index]);
            }
        }
    }

    // TODO debug purpose
    private void AddAllTilesToWorld()
    {
        foreach (GameObject o in _tiles) {
            LeanPool.Despawn(o);
        }
        _tiles.Clear();
        LeanPool.Despawn(_tileContainer);
        _tileContainer = LeanPool.Spawn(tileContainerPrefab);

        var ratio = 3;
        var tileWith = .24f * ratio;
        var tileHeight = .28f * ratio;

        for (var x = 0; x < mapSize.x; x++) {
            for (var y = 0; y < mapSize.y; y++) {
                var t = LeanPool.Spawn(tilePrefab);
                // position
                var tX = x * tileWith;
                var tY = -(y * tileHeight); // TODO : reverse this?!
                t.transform.position = new Vector3(tX, tY, 0);
                // scale
                t.transform.localScale = new Vector3(tileWith, tileHeight, 1f);

                t.transform.SetParent(_tileContainer.transform);
                var renderer = t.GetComponent<SpriteRenderer>();
                renderer.sprite = _map[(int)x, (int)y].sprite;
                _tiles.Add(t);
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

        var ratio = 3;
        var tileWith = .24f * ratio;
        var tileHeight = .28f * ratio;

        var viewOffsetX = viewPortSize.x/2f;
        var viewOffsetY = viewPortSize.y/2f;
        for (var y = -viewOffsetY; y < viewOffsetY; y++) {
            for (var x = -viewOffsetX; x < viewOffsetX; x++) {

                // position
                var tX = x * tileWith;
                var tY = -(y * tileHeight); // TODO : reverse this?!

                var iX = x + currentPosition.x;
                var iY = y + currentPosition.y;

                if (iX < 0) continue;
                if (iY < 0) continue;
                if(iX > mapSize.x - 2) continue;
                if (iY > mapSize.y - 2) continue;

                var t = LeanPool.Spawn(tilePrefab);

                // position
                t.transform.position = new Vector3(tX, tY, 0);

                // scale
                t.transform.localScale = new Vector3(tileWith, tileHeight, 1f);

                t.transform.SetParent(_tileContainer.transform);
                var renderer = t.GetComponent<SpriteRenderer>();
                renderer.sprite = _map[(int)x + (int)currentPosition.x, (int)y + (int)currentPosition.y].sprite;
                _tiles.Add(t);
            }
        }
    }
}