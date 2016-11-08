using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Generates a map of clean tiles, with clean ground transitions, rounded borders, etc
 *
 * Huge Thx to Chmood who inspirates this generator https://github.com/Chmood/shmup/blob/gh-pages/src/js/game.js
 */
public class FinalTilesGenerator {

    /**
     * Detect ground differences and corners to replace base tiles by rounded tiles
     */
    public int[,] Generate(int[,] tiles, int width, int height)
    {
        var roundedTiles = new int[width, height];

        for (var n = 1; n < GroundTiles.STACK.Length; n++) {

            var currentLayer = GroundTiles.STACK[n];
            var upperLayer = GroundTiles.STACK[n - 1];

            for (var i = 0; i < width; i++) {
                for (var j = 0; j < height; j++) {

                    // copy last tiles
                    // TODO generate one more row and column and drop them at the end cause we can't smooth those???
                    if (i == width - 1 || j == height - 1) {
                        roundedTiles[i, j] = tiles[i, j];
                        continue;
                    }

                    string q = tiles[i, j] + "," + tiles[i + 1, j] + "," + tiles[i, j + 1] + "," + tiles[i + 1, j + 1];

                    // 4 corners
                    if (q == (upperLayer + "," + upperLayer + "," + upperLayer + "," + upperLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 6;

                    // 3 corners
                    } else if (q == (upperLayer + "," + upperLayer + "," + upperLayer + "," + currentLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 9;

                    } else if (q == (upperLayer + "," + upperLayer + "," + currentLayer + "," + upperLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 8;

                    } else if (q == (currentLayer + "," + upperLayer + "," + upperLayer + "," + upperLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 3;

                    } else if (q == (upperLayer + "," + currentLayer + "," + upperLayer + "," + upperLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 4;

                    // 2 corners
                    } else if (q == (upperLayer + "," + upperLayer + "," + currentLayer + "," + currentLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 11;

                    } else if (q == (currentLayer + "," + upperLayer + "," + currentLayer + "," + upperLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 5;

                    } else if (q == (currentLayer + "," + currentLayer + "," + upperLayer + "," + upperLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 1;

                    } else if (q == (upperLayer + "," + currentLayer + "," + upperLayer + "," + currentLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 7;

                    } else if (q == (currentLayer + "," + upperLayer + "," + upperLayer + "," + currentLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 14;

                    } else if (q == (upperLayer + "," + currentLayer + "," + currentLayer + "," + upperLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 13;

                    // 1 corner
                    } else if (q == (upperLayer + "," + currentLayer + "," + currentLayer + "," + currentLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 12;

                    } else if (q == (currentLayer + "," + upperLayer + "," + currentLayer + "," + currentLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 10;

                    } else if (q == (currentLayer + "," + currentLayer + "," + currentLayer + "," + upperLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 0;

                    } else if (q == (currentLayer + "," + currentLayer + "," + upperLayer + "," + currentLayer)) {
                        roundedTiles[i, j] = (n - 1) * 15 + 2;

                    // no corner
                    } else if (q == (currentLayer + "," + currentLayer + "," + currentLayer + "," + currentLayer)) {
                        roundedTiles[i, j] = n * 15 + 6;

                    }
                }
            }
        }

        return roundedTiles;
    }
}