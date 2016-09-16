using UnityEngine;
using System.Collections;

public class TerrainEditorScript : MonoBehaviour {
    public void randomcolors()
    {
        TerrainData td = GetComponent<Terrain>().terrainData;
        float[,,] alphamaps = td.GetAlphamaps(0, 0, td.alphamapWidth, td.alphamapHeight);
        for (int x = 0; x < alphamaps.GetLength(0); x++)
            for (int y = 0; y < alphamaps.GetLength(1); y++)
                alphamaps[x, y, 0] = Random.Range(0f, 1f);
        td.SetAlphamaps(0, 0, alphamaps);
    }
}
