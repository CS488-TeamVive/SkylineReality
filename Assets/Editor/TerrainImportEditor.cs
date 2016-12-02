#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;

using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class TerrainSceneCreator : EditorWindow {

    void OnGUI()
    {
        
    }

    public static Terrain getSelectedTerrain()
    {
        Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>(); 
        if (terrain == null)
        {
            EditorUtility.DisplayDialog("TerrainCreate Error", "Select a Terrain before using this tool", "Okay");
            return null;
        }
        return terrain;
    }
    [MenuItem("Skyline Import/Import from ASC")]
    static void CreateFromFile()
    {
        //Terrain terrain = getSelectedTerrain();
        // if (terrain == null) return;

        string path = EditorUtility.OpenFilePanel("Load Terrain File", "", "asc");
        if (path.Length == 0)
            return;



        Dictionary<string, float> config = new Dictionary<string, float>();
        float[,] heightMap;
        float minHeightActual = float.MaxValue;
        float maxHeightActual = float.MinValue;
        float heightSpread;
        float heightScale;

        using (StreamReader sr = new StreamReader(path))
        {
            string line;
            Regex paramPattern = new Regex(@"^([\w_]+)\s+(\-?\d+(\.\d+)?)$", RegexOptions.IgnoreCase);
            while (true)
            {
                line = sr.ReadLine();
                //Debug.Log(string.Format("Reading line {0}", line));
                if (paramPattern.IsMatch(line))
                {
                    Match match = paramPattern.Match(line);
                    config[match.Groups[1].Value] = float.Parse(match.Groups[2].Value);
                    //Debug.Log(string.Format("Parsed value: {0}", config[match.Groups[1].Value]));
                }
                else
                    break;
            }

            heightMap = new float[(int)config["ncols"], (int)config["nrows"]];

            string nextValue = string.Empty;
            int row = 0;
            int col = 0;
            foreach(char c in line)
            {
                if (c.Equals(' ') && !nextValue.Equals(string.Empty))
                {
                    float commitValue = float.Parse(nextValue);
                    heightMap[row, col++] = commitValue;
                    nextValue = string.Empty;
                    if (commitValue > maxHeightActual)
                        maxHeightActual = commitValue;
                    if (commitValue < minHeightActual)
                        minHeightActual = commitValue;
                }
            }
            row++;

            char[] nextChar = new char[1];
            do
            {
                sr.Read(nextChar, 0, 1);
                if (nextChar[0].Equals(' ') && !nextValue.Equals(string.Empty))
                {
                    float commitValue = float.Parse(nextValue);
                    heightMap[row, col++] = commitValue;
                    nextValue = string.Empty;
                    if (commitValue > maxHeightActual)
                        maxHeightActual = commitValue;
                    if (commitValue < minHeightActual)
                        minHeightActual = commitValue;
                }
                else if (nextChar[0].Equals('\r'))
                {
                    continue;
                }
                else if (nextChar[0].Equals('\n'))
                {
                    row++;
                    col = 0;
                }
                else
                {
                    nextValue += nextChar[0];
                }
            }
            while (!sr.EndOfStream);
        }

        heightSpread = maxHeightActual - minHeightActual;
        heightScale = 1 / heightSpread;

        EditorUtility.DisplayDialog("Import stats", string.Format("Total values: {0:D} ({1:D}x{2:D})\nMin:{3:D}   Max:{4:D}", heightMap.Length, heightMap.GetLength(0), heightMap.GetLength(1), (int)minHeightActual, (int)maxHeightActual), "Okay");

        for (int x = 0; x < heightMap.GetLength(0); x++)
            for (int y = 0; y < heightMap.GetLength(1); y++)
                heightMap[x, y] = (heightMap[x, y] - minHeightActual) / heightSpread;



        //terrain.terrainData.heightmapResolution = NextBestResolution((int)Mathf.Max(config["nrows"], config["ncols"]));
        // terrain.terrainData.SetHeights(1, 1, heightMap);

        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = 2049;
        terrainData.SetHeights(0, 0, heightMap);
        terrainData.size = new Vector3(heightMap.GetLength(0) * config["cellsize"], heightSpread * config["cellsize"] * 0.01f, heightMap.GetLength(1) * config["cellsize"]);
        

        GameObject terrain = Terrain.CreateTerrainGameObject(terrainData);

        terrain.transform.name = path.Substring(path.LastIndexOf("/") + 1);

        CoordinateTranslation coord = terrain.gameObject.AddComponent<CoordinateTranslation>();
        coord.SetCenter((int)config["yllcorner"], (int)config["xllcorner"], "Unknown");
        coord.MetersPerPixel = config["cellsize"];
        coord.MinHeightActual = (int)minHeightActual;
        coord.HeightScale = heightScale;
        coord.HeightmapNSLength = (int)config["nrows"];
        coord.HeightmapEWLength = (int)config["ncols"];

        EditorUtility.DisplayDialog("Job Complete", "The terrain has finished importing", "Okay");
    }

    [MenuItem("Skyline Import/Set Random Heightmap")]
    static void SetRandomHeightmap()
    {
        Terrain terrain = getSelectedTerrain();
        if (terrain == null) return;

        TerrainData terrainData = terrain.terrainData;

        float[,] heightmap = new float[terrainData.heightmapWidth, terrainData.heightmapWidth];

        float multx = Random.Range(0.1f, 0.2f);
        float multy = Random.Range(0.1f, 0.2f);

        for (int x = 0; x < terrainData.heightmapWidth; x++)
            for (int y = 0; y < terrainData.heightmapWidth; y++)
                heightmap[x, y] = ((Mathf.Sin(x * multx) + Mathf.Sin(y * multy)) + 2) / 50;

        terrainData.SetHeights(0, 0, heightmap);
                
    }

    [MenuItem("Skyline Import/Terrain Stats")]
    static void TerrainStats()
    {
        Terrain terrain = getSelectedTerrain();
        if (terrain == null) return;

        TerrainData terrainData = terrain.terrainData;

        Vector2 resolution = MetersPerPixel(terrainData);
        EditorUtility.DisplayDialog("Terrain Stats", string.Format("Meters per pixel: ({0:F}, {1:F})\nResolution: ({2:D}, {2:D})", resolution.x, resolution.y, terrainData.heightmapResolution), "Okay");
    }

    static Vector2 MetersPerPixel(TerrainData terrainData)
    {
        return new Vector2(
            terrainData.size.x / terrainData.heightmapWidth,
            terrainData.size.y / terrainData.heightmapHeight);
    }

    static int NextBestResolution(int resolution)
    {
        return 1 + 2 ^((int)Mathf.Log(resolution - 2, 2) + 1);
    }
}
#endif