using UnityEngine;
using UnityEditor;
public enum TileType
{
    TowerPlatform,
    GroundPlatform
}

[System.Serializable]
public class TileMap
{
    public TileType tileType;
    public GameObject tileMapReference;
}

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Texture2D map;
    [SerializeField] private ColorToPrefab[] tiles;
    [SerializeField] private TileMap[] tileMaps;

    public void GenerateLevel()
    {
        ClearLevel();
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    public void ClearLevel()
    {
        for (int i = 0; i < tileMaps.Length; i++)
        {
            while (tileMaps[i].tileMapReference.transform.childCount > 0)
            {
                foreach (Transform child in tileMaps[i].tileMapReference.transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        Color32 pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            return;
        }

        foreach (ColorToPrefab tile in tiles)
        {
            if (tile.colorKey.Equals(pixelColor))
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject tileSpawned = Instantiate(tile.prefab, position, Quaternion.identity);
                tileSpawned.transform.SetParent(ParentBasedOnType(tile).transform);
                tileSpawned.transform.localPosition = new Vector3(tileSpawned.transform.localPosition.x + tileSpawned.transform.parent.transform.parent.localPosition.x,
                0.5f, tileSpawned.transform.localPosition.z + tileSpawned.transform.parent.transform.parent.localPosition.z);
            }
        }
    }

    private GameObject ParentBasedOnType(ColorToPrefab tile)
    {
        for (int i = 0; i < tileMaps.Length; i++)
        {
            if (tile.assignedTilemap == tileMaps[i].tileType)
            {
                return tileMaps[i].tileMapReference;
            }
        }
        return null;
    }
}


