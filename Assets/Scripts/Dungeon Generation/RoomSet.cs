using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New RoomSet", menuName = "DungeonGen/RoomSet", order = 3)]
public class RoomSet : ScriptableObject {

    public Texture2D[] maps;
    public ColorMap colorMap;
    private Texture2D currentMap;
    private int xIndex;
    private int yIndex;

    public void RandomizeCurrentMap()
    {
        currentMap = maps[Random.Range(0, maps.Length - 1)];
        xIndex = 0;
        yIndex = 0;
    }

    public TileBase GetTile(int x, int y)
    {
        Color pixelColor = currentMap.GetPixel(x, y);
        return colorMap.ColorToTile(pixelColor);
    }

    public IEnumerable<TileBase> GetRandomMap()
    {
        var map = maps[Random.Range(0, maps.Length - 1)];

        Debug.Log(map.width + " " + map.height);

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                Color pixelColor = map.GetPixel(x, y);

                if (pixelColor.a == 0)
                    yield return null;
                else
                    yield return colorMap.ColorToTile(pixelColor);
            }
        }
    }

    [CreateAssetMenu(fileName = "New ColorMap", menuName = "DungeonGen/ColorMap", order = 3)]
    public class ColorMap : ScriptableObject
    {
        public ColorToTile[] colorMap;

        public TileBase ColorToTile(Color color)
        {
            foreach (ColorToTile colorTile in colorMap)
            {
                if (colorTile.color.ToString() == color.ToString())
                {
                    return colorTile.tile;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class ColorToTile
    {
        public Color color;
        public TileBase tile;
    }
}
