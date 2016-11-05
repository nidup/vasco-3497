using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Erase difference between tile layer, for instance, a forrest tile can have sand as neighbour but not water, sand can
 * have water as neighbour but but not deep water, etc
 */
public class SmoothTilesGenerator {

    public int[,] Generate(int[,] tiles, int width, int height)
    {
        for (var n = 0; n < GroundTiles.STACK.Length - 1; n++) {
            var tileCurrent = GroundTiles.STACK[n];
            var tileAbove = (n > 0) ? GroundTiles.STACK[n - 1] :  -1;
            var tileBelow =  GroundTiles.STACK[n + 1];

            for (var i = 0; i < width ; i++) {
                for (var j = 0; j < height; j++) {
                    if (tiles[i, j] == tileCurrent) {
                        var isLeftUp = i > 0 && j > 0 && tiles[i - 1, j - 1] != tileCurrent
                            && tiles[i - 1, j - 1] != tileAbove && tiles[i - 1, j - 1] != tileBelow;
                        if (isLeftUp) {
                            tiles[i - 1, j - 1] = tileBelow;
                        }
                        var isMidUp = j > 0 && tiles[i, j - 1] != tileCurrent && tiles[i, j - 1] != tileAbove
                            && tiles[i, j - 1] != tileBelow;
                        if (isMidUp) {
                            tiles[i, j - 1] = tileBelow;
                        }
                        var isRightUp = i < width - 1 && j > 0 && tiles[i + 1, j - 1] != tileCurrent
                            && tiles[i + 1, j - 1] != tileAbove && tiles[i + 1, j - 1] != tileBelow;
                        if (isRightUp) {
                            tiles[i + 1, j - 1] = tileBelow;
                        }
                        var isRightMid = i < width - 1 && tiles[i + 1, j] != tileCurrent
                            && tiles[i + 1, j] != tileAbove && tiles[i + 1, j] != tileBelow;
                        if (isRightMid) {
                            tiles[i + 1, j] = tileBelow;
                        }
                        var isRightDown = i < width - 1 && j < height - 1
                            && tiles[i + 1, j + 1] != tileCurrent && tiles[i + 1, j + 1] != tileAbove
                            && tiles[i + 1, j + 1] != tileBelow;
                        if (isRightDown) {
                            tiles[i + 1, j + 1] = tileBelow;
                        }
                        var isMidDown = j < height - 1 && tiles[i, j + 1] != tileCurrent
                            && tiles[i, j + 1] != tileAbove && tiles[i, j + 1] != tileBelow;
                        if (isMidDown) {
                            tiles[i, j + 1] = tileBelow;
                        }
                        var isLeftDown = i > 0 && j < height - 1 && tiles[i - 1, j + 1] != tileCurrent
                            && tiles[i - 1, j + 1] != tileAbove && tiles[i - 1, j + 1] != tileBelow;
                        if (isLeftDown) {
                            tiles[i - 1, j + 1] = tileBelow;
                        }
                        var isLeftMid = i > 0 && tiles[i - 1, j] != tileCurrent
                            && tiles[i - 1, j] != tileAbove && tiles[i - 1, j] != tileBelow;
                        if (isLeftMid) {
                            tiles[i - 1, j] = tileBelow;
                        }
                    }
                }
            }
        }

        return tiles;
    }
}

