using UnityEngine;
using UnityEngine.Tilemaps;

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
