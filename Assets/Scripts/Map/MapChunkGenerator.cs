using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Generates MapChunk from neighbour to be able to get consistent junctions between chunks when passing from a chunk
 * to another (no transition from water to forest for instance)
 */
public class MapChunkGenerator {

    public MapChunk generateInitial(int width, int height)
    {
        BaseTilesGenerator baseGenerator = new BaseTilesGenerator();
        int [,] baseTiles = baseGenerator.Generate(width, height);

        SmoothTilesGenerator smoothGenerator = new SmoothTilesGenerator();
        int [,] smoothTiles = smoothGenerator.Generate(baseTiles, width, height);

        FinalTilesGenerator finalGenerator = new FinalTilesGenerator();
        int [,] finalTiles = finalGenerator.Generate(smoothTiles, width, height);

        return new MapChunk(baseTiles, smoothTiles, finalTiles, 0, 0, Random.state);
    }

    public MapChunk generateLeftOf(MapChunk chunk, int x, int y, int width, int height, int nbToCopy)
    {
        var copier = new NeighbourTilesCopier();
        int [,] initTiles = copier.copyLeftTiles(chunk.getSmoothTiles(), nbToCopy);

        return generateFromInitTiles(initTiles, x, y, width, height);
    }

    public MapChunk generateRightOf(MapChunk chunk, int x, int y, int width, int height, int nbToCopy) {
        var copier = new NeighbourTilesCopier();
        int [,] initTiles = copier.copyRightTiles(chunk.getSmoothTiles(), nbToCopy);

        return generateFromInitTiles(initTiles, x, y, width, height);
    }

    public MapChunk generateTopOf(MapChunk chunk, int x, int y, int width, int height, int nbToCopy) {
        var copier = new NeighbourTilesCopier();
        int [,] initTiles = copier.copyTopTiles(chunk.getSmoothTiles(), nbToCopy);

        return generateFromInitTiles(initTiles, x, y, width, height);
    }

    public MapChunk generateBottomOf(MapChunk chunk, int x, int y, int width, int height, int nbToCopy) {
        var copier = new NeighbourTilesCopier();
        int [,] initTiles = copier.copyBottomTiles(chunk.getSmoothTiles(), nbToCopy);

        return generateFromInitTiles(initTiles, x, y, width, height);
    }

    private MapChunk generateFromInitTiles (int [,] initTiles, int x, int y, int width, int height)
    {
        BaseTilesGenerator baseGenerator = new BaseTilesGenerator();
        int [,] baseTiles = baseGenerator.Generate(width, height, initTiles);

        SmoothTilesGenerator smoothGenerator = new SmoothTilesGenerator();
        int [,] smoothTiles = smoothGenerator.Generate(baseTiles, width, height);

        FinalTilesGenerator finalGenerator = new FinalTilesGenerator();
        int [,] finalTiles = finalGenerator.Generate(smoothTiles, width, height);

        return new MapChunk(baseTiles, smoothTiles, finalTiles, 0, 0, Random.state);
    }
}