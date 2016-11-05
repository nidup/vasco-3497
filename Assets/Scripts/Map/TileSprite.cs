using System;
using UnityEngine;
using System.Collections;

// thx to https://gamedevacademy.org/how-to-script-a-2d-tile-map-in-unity3d/

[Serializable]
public class TileSprite {

    public string Name;
    public Sprite TileImage;
    public Tiles TileType;

    public TileSprite()
    {
        Name = "Unset";
        TileImage = new Sprite();
        TileType = Tiles.Unset;
    }

    public TileSprite(string name, Sprite image, Tiles tile)
    {
        Name = name;
        TileImage = image;
        TileType = tile;
    }
}
