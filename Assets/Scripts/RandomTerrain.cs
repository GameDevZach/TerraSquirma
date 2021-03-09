using UnityEngine;


/// <summary>
// Randomizer for Unity Terrains themselves. Based on Brackey's 2017 Procedural Terrain video
/// </summary>
public class RandomTerrain : MonoBehaviour
{
    public int height = 20;

    public int width = 256;
    public int length = 256;

    public float scale = 4f;
    public Vector2 offset = new Vector2(100f, 100f);

    public int circleRamp = 0;

    private void Start()
    {
        offset = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));
    }

    // Start is called before the first frame update
    void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain( terrain.terrainData );
    }

    TerrainData GenerateTerrain (TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, height, length);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float [,] GenerateHeights ()
    {
        float[,] heights = new float[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight (int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / length * scale;

        float circularDamp = CircularRamp(x, y);
        return Mathf.PerlinNoise(xCoord + offset.x, yCoord + offset.y) * circularDamp;
    }

    float CircularRamp ( int x, int y)
    {
        float dist = Vector2.Distance(new Vector2(width / 2f, length / 2f), new Vector2((float)x, (float)y));

        return (Mathf.SmoothStep(1f, 0f, dist / (width / 2f - circleRamp)));
    }
}
