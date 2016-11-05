using System;
using UnityEngine;
using System.Collections;

// thx to https://gamedevacademy.org/how-to-script-a-2d-tile-map-in-unity3d/

[Serializable]
public class TileSprite {

    public Sprite sprite;

    public TileSprite()
    {
        sprite = new Sprite();
    }

    public TileSprite(Sprite mySprite)
    {
        sprite = mySprite;
    }
}
