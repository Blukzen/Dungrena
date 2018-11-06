using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New RoomSet", menuName = "DungeonGen/RoomSet", order = 3)]
public class RoomSet : ScriptableObject {

    public Texture2D[] maps;
    public ColorMap colorMap;
    private Texture2D currentMap;

    public string RandomizeCurrentMap()
    {
        currentMap = maps[Random.Range(0, maps.Length - 1)];
        return currentMap.name;
    }

    public List<TileBase[]> GetRandomMap()
    {
        List<TileBase[]> map = new List<TileBase[]>();
        RandomizeCurrentMap();

        // Floor layer
        var floor = GetFloorMap();
        map.Insert(0, floor);

        // Object layer
        var objects = GetObjectMap();
        map.Insert(1, objects);

        return map;
    }

    public TileBase[] GetObjectMap()
    {
        int index = 0;
        var map = currentMap;
        TileBase[] tiles = new TileBase[map.width * map.height/2];

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height/2; y++)
            {
                Color pixelColor = map.GetPixel(x, y);

                if (pixelColor.a == 0)
                    tiles[index] = null;
                else
                    tiles[index] = colorMap.ColorToTile(pixelColor);

                index++;
            }
        }

        return tiles;
    }

    public TileBase[] GetFloorMap()
    {
        int index = 0;
        var map = currentMap;
        TileBase[] tiles = new TileBase[map.width * map.height/2];

        for (int x = 0; x < map.width; x++)
        {
            for (int y = map.height/2; y < map.height; y++)
            {
                Color pixelColor = map.GetPixel(x, y);

                if (pixelColor.a == 0)
                    tiles[index] = null;
                else
                    tiles[index] = colorMap.ColorToTile(pixelColor);

                index++;
            }
        }

        return tiles;
    }
}