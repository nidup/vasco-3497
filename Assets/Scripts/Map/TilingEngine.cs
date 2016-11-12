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
    public GameObject player;
    public Vector2 viewPortSize;

    private MapChunk currentChunk;
    private Vector2 currentPosition;
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
        currentPosition = new Vector2(mapSize.x / 2, mapSize.y / 2);
        _map = new TileSprite[(int)mapSize.x, (int)mapSize.y];

        GenerateMapChunk();
        FulfilTileMap();
    }

    public void FixedUpdate()
    {
        int width = (int) mapSize.x;
        int height = (int) mapSize.y;
        int nbToCopy = 5;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Vector3 localVelocity = player.transform.InverseTransformDirection(rb.velocity);
        MapChunkGenerator generator = new MapChunkGenerator();

        // Debug.Log("x : " + localVelocity.x + " y : " + localVelocity.y);
        // Debug.Log("x/y : " + player.transform.position.x + "/" + player.transform.position.y);

        float borderTop = mapSize.y / 2;
        float borderBottom = - borderTop;
        float borderRight = mapSize.x / 2;
        float borderLeft = - borderRight;

        if (player.transform.position.y > borderTop && localVelocity.y > 0) {

            currentChunk = generator.generateTopOf(
                currentChunk,
                currentChunk.getPositionX(),
                currentChunk.getPositionY() - 1,
                width,
                height,
                nbToCopy
            );
            FulfilTileMap();

            Vector2 newPosition = new Vector2(player.transform.position.x, borderBottom);
            player.transform.position = newPosition;

        } else if (player.transform.position.y < borderBottom && localVelocity.y < 0) {

            currentChunk = generator.generateBottomOf(
                currentChunk,
                currentChunk.getPositionX(),
                currentChunk.getPositionY() + 1,
                width,
                height,
                nbToCopy
            );
            FulfilTileMap();

            Vector2 newPosition = new Vector2(player.transform.position.x, borderTop);
            player.transform.position = newPosition;

        } else if (player.transform.position.x < borderLeft && localVelocity.x < 0) {

            currentChunk = generator.generateLeftOf(
                currentChunk,
                currentChunk.getPositionX() - 1,
                currentChunk.getPositionY(),
                width,
                height,
                nbToCopy
            );
            FulfilTileMap();

            Vector2 newPosition = new Vector2(borderRight, player.transform.position.y);
            player.transform.position = newPosition;

        } else if (player.transform.position.x > borderRight && localVelocity.x > 0) {

            currentChunk = generator.generateRightOf(
                currentChunk,
                currentChunk.getPositionX() + 1,
                currentChunk.getPositionY(),
                width,
                height,
                nbToCopy
            );
            FulfilTileMap();

            Vector2 newPosition = new Vector2(borderLeft, player.transform.position.y);
            player.transform.position = newPosition;

        }
    }

    public void Update()
    {
        AddTilesToWorld();
        //AddAllTilesToWorld();
    }

    private void GenerateMapChunk()
    {
        int width = (int) mapSize.x;
        int height = (int) mapSize.y;
        MapChunkGenerator generator = new MapChunkGenerator();
        currentChunk = generator.generateInitial(width, height);
    }

    private void FulfilTileMap()
    {
        int width = (int) mapSize.x;
        int height = (int) mapSize.y;
        int [,] tiles = currentChunk.getFinalTiles();
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                _map[x, y] = new TileSprite(sprites[tiles[x, y]]);
            }
        }
    }

    // TODO debug purpose
    /*
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
    }*/

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


        //var playerPositionInTileX = (int) player.transform.position.x / tileWith;
        //var playerPositionInTileY = (int) player.transform.position.y / tileHeight;
        //Debug.Log("x/y : " + player.transform.position.x + "/" + player.transform.position.y);

        //Vector2 currentPosition = new Vector2(player.transform.position.x, -player.transform.position.y);


        var viewOffsetX = viewPortSize.x/2f;
        var viewOffsetY = viewPortSize.y/2f;

        /*
        var viewOffsetX = (int) player.transform.position.x/2f;
        var viewOffsetY = (int) player.transform.position.y/2f;
        */

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