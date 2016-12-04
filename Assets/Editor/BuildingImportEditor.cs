#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

using System.Xml;
using System.IO;

using VoronoiNS;

public class BuildingImportEditor : MonoBehaviour {

    private enum MapFeatureTypes { Unknown, Building };
    private class MapFeature
    {
        public MapFeatureTypes type = MapFeatureTypes.Unknown;
        public List<long> ndreferences;
        public Dictionary<long, double[]> ndlookup;
        public string name;
        public int height;
        public MapFeature()
        {
            ndreferences = new List<long>();
        }
        public void PushReferenceID(long l)
        {
            ndreferences.Add(l);
        }
        public void AssociateLookup(Dictionary<long, double[]> Lookup)
        {
            ndlookup = Lookup;
        }
        public IEnumerator<double[]> GetCoordinateEnumerator()
        {
            foreach(long l in ndreferences)
                yield return ndlookup[l];
        }
        public int NodeCount()
        {
            return ndreferences.Count;
        }
        public double[] GetNodeCoordinates(int index)
        {
            return ndlookup[ndreferences[index]];
        }
    }

    [MenuItem("Skyline Import/Import OSM")]
    static void ImportOSM()
    {
        string path = EditorUtility.OpenFilePanel("Load OSM File", "", "osm");
        if (path.Length == 0)
        {
            EditorUtility.DisplayDialog("Failure", "Importing failed", "Okay");
            return;
        }

        List<MapFeature> mapFeatures = new List<MapFeature>();
        MapFeature currentFeature = null;

        Dictionary<long, double[]> ndLookup = new Dictionary<long, double[]>();
        List<long> ndCache = new List<long>();

        Stack<string> xmlPath = new Stack<string>();
        FileStream fs;
        XmlReader xr;

        /* START PASS ONE */
        fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        xr = XmlReader.Create(fs);
        
        //xr.MoveToContent();
        // Parse the file and display each of the nodes.
        while (xr.Read())
        {
            switch (xr.NodeType)
            {
                case XmlNodeType.Element:
                    //print(string.Format("<{0}>", xr.Name));

                    if (xr.Name.Equals("way"))
                    {
                        currentFeature = new MapFeature();
                    }
                    else if (xmlPath.Count > 0 && xmlPath.Peek().Equals("way"))
                    {
                        switch (xr.Name)
                        {
                            case "nd":
                                ndCache.Add(long.Parse(xr.GetAttribute("ref")));
                                break;
                            case "tag":
                                if (xr.GetAttribute("k").Equals("building"))
                                {
                                    currentFeature.type = MapFeatureTypes.Building;
                                }
                                if (xr.GetAttribute("k").Equals("building:levels"))
                                {
                                    currentFeature.height = int.Parse(xr.GetAttribute("v"));
                                }
                                else if (xr.GetAttribute("k").Equals("name"))
                                {
                                    currentFeature.name = xr.GetAttribute("v");
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (!xr.IsEmptyElement)
                        xmlPath.Push(xr.Name);

                    break;
                case XmlNodeType.EndElement:
                    //print(string.Format("</{0}>", xr.Name));
                    string name = xmlPath.Pop();

                    if (name == "way")
                    {
                        if (currentFeature != null && currentFeature.type == MapFeatureTypes.Building)
                        {
                            mapFeatures.Add(currentFeature);
                            currentFeature.AssociateLookup(ndLookup);
                            foreach (long l in ndCache)
                            {
                                currentFeature.PushReferenceID(l);
                                if (!ndLookup.ContainsKey(l))
                                    ndLookup.Add(l, null);
                            }
                        }
                        ndCache.Clear();
                        currentFeature = null;
                    }

                    break;
                    /*
                case XmlNodeType.Text:
                    print(xr.Value);
                    break;
                case XmlNodeType.CDATA:
                    print(string.Format("<![CDATA[{0}]]>", xr.Value));
                    break;
                case XmlNodeType.ProcessingInstruction:
                    print(string.Format("<?{0} {1}?>", xr.Name, xr.Value));
                    break;
                case XmlNodeType.Comment:
                    print(string.Format("<!--{0}-->", xr.Value));
                    break;
                case XmlNodeType.XmlDeclaration:
                    print("<?xml version='1.0'?>");
                    break;
                case XmlNodeType.Document:
                    break;
                case XmlNodeType.DocumentType:
                    print(string.Format("<!DOCTYPE {0} [{1}]", xr.Name, xr.Value));
                    break;
                case XmlNodeType.EntityReference:
                    print(xr.Name);
                    break;
                    */
            }
        }
        /* START PASS TWO */
        fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        xr = XmlReader.Create(fs);

        //xr.MoveToContent();
        // Parse the file and display each of the nodes.
        while (xr.Read())
        {
            switch (xr.NodeType)
            {
                case XmlNodeType.Element:
                    if (xr.Name.Equals("node"))
                    {
                        long id = long.Parse(xr.GetAttribute("id"));
                        if (ndLookup.ContainsKey(id))
                        {
                            //print(string.Format("{0} {1}", xr.GetAttribute("lat"), xr.GetAttribute("lon")));
                            ndLookup[id] =
                                new double[2]
                                {
                                    double.Parse(xr.GetAttribute("lat")),
                                    double.Parse(xr.GetAttribute("lon"))
                                };
                        }
                    }

                    if (!xr.IsEmptyElement)
                        xmlPath.Push(xr.Name);

                    break;
                case XmlNodeType.EndElement:
                    xmlPath.Pop();
                    break;
            }
        }
        

        /* START BUILDING BUILDINGS */

        Terrain[] terrains = GameObject.FindObjectsOfType<Terrain>();
        Terrain activeTerrain = null;
        CoordinateTranslation activeTranslation = null;
        //GameObject container = new GameObject();

        //container.name = "BuildingContainer";

        Material buildingMat = Resources.Load("Buildings/BuildingMat", typeof(Material)) as Material;

        int buildingsOOB = 0;

        foreach(MapFeature mf in mapFeatures)
        {
            double[] firstCoordinates = mf.GetNodeCoordinates(0);
            int firstNorthing, firstEasting;
            string firstZone;

            CoordinateTranslation.LatLongtoUTM(
                firstCoordinates[0],
                firstCoordinates[1],
                out firstNorthing, out firstEasting, out firstZone);

            /*
            activeTerrain = null;
            activeTranslation = null;
            */
            activeTerrain = GameObject.FindObjectOfType<Terrain>();
            activeTranslation = activeTerrain.GetComponent<CoordinateTranslation>();
            
            foreach (Terrain t in terrains)
            {
                if (t.GetComponent<CoordinateTranslation>().WithinExtent(firstNorthing, firstEasting))
                {
                    activeTerrain = t;
                    activeTranslation = t.GetComponent<CoordinateTranslation>();
                    break;
                }
            }
            if (activeTerrain == null || activeTranslation == null)
            {
                buildingsOOB++;
                continue;
            }

            // check if first and last reference are the same; if so, REMOVE THE LAST because it will BREAK TRIANGULATION
            if (mf.ndreferences[mf.ndreferences.Count - 1] == mf.ndreferences[0])
                mf.ndreferences.RemoveAt(mf.ndreferences.Count - 1);

            Vector2[] relativeNodes = new Vector2[mf.NodeCount()];
            int c = 0;

            foreach (long l in mf.ndreferences)
            {
                int northing, easting;
                string zone;

                CoordinateTranslation.LatLongtoUTM(ndLookup[l][0], ndLookup[l][1], out northing, out easting, out zone);

                relativeNodes[c++] = new Vector2((float)(easting - firstEasting), (float)(northing - firstNorthing));
            }
            GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
            building.transform.parent = activeTerrain.transform;

            if (string.IsNullOrEmpty(mf.name))
                building.name = "name unavailable";
            else
                building.name = mf.name;

            if (mf.height == 0)
                mf.height = 1;

            Mesh mesh = CreateMesh(relativeNodes, mf.height * 3.048f);
            building.GetComponent<MeshFilter>().mesh = mesh;
            building.GetComponent<Renderer>().material = buildingMat;

            Vector2 relative = activeTranslation.RelativePosition((int)firstNorthing, (int)firstEasting);

            building.transform.position = new Vector3(relative.x, activeTranslation.ElevationOffsetAtCoordinate((int)firstNorthing, (int)firstEasting), relative.y);
            // if this line throws an exception then you need to create the 'building' tag in the project
            building.tag = "building";
        }

        EditorUtility.DisplayDialog("Finished importing from OSM", "Importing was completed with " + (mapFeatures.Count - buildingsOOB) + "/" + mapFeatures.Count + " in bounds", "Okay");
    }

    [MenuItem("Skyline Import/NUKE IT")]
    public static void RemoveBuildings()
    {
        foreach(GameObject deleteme in GameObject.FindGameObjectsWithTag("building"))
            DestroyImmediate(deleteme);

    }

    public class BuildingGenerator : EditorWindow
    {
        string buildingName = "name";
        string coordinateString = "-1,0;1,0;0,1";

        // Add menu named "My Window" to the Window menu
        [MenuItem("Skyline Debug/Gen Sample Building")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            BuildingGenerator window = (BuildingGenerator)EditorWindow.GetWindow(typeof(BuildingGenerator));
            window.ShowUtility();
        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            buildingName = EditorGUILayout.TextField("Building name: ", buildingName);
            coordinateString = EditorGUILayout.TextField("Coordinate string: ", coordinateString);

            if (GUILayout.Button("Create"))
            {
                List<Vector2> vec = new List<Vector2>();
                foreach(string s_vec in coordinateString.Split(';'))
                {
                    string[] vecpart = s_vec.Split(',');
                    vec.Add(new Vector2(float.Parse(vecpart[0]), float.Parse(vecpart[1])));
                }
                GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Terrain t = GameObject.FindObjectOfType<Terrain>();
                CoordinateTranslation ct = t.GetComponent<CoordinateTranslation>();
                Material buildingMat = Resources.Load("Buildings/BuildingMat", typeof(Material)) as Material;
                Mesh mesh = CreateMesh(vec.ToArray(), 3.048f, true);
                building.GetComponent<MeshFilter>().mesh = mesh;
                building.GetComponent<Renderer>().material = buildingMat;
                building.transform.parent = t.transform;
                building.transform.position = new Vector3(vec[0].x + 1000, ct.ElevationOffsetAtCoordinate((int)(ct.UTMNorthing + vec[0].y), (int)(ct.UTMEasting + vec[0].x)), vec[0].y + 1000);
                // if this line throws an exception then you need to create the 'building' tag in the project
                building.tag = "building";
            }
        }
    }


    public static string[] IntListToStringArr(List<int> intl)
    {
        string[] result = new string[intl.Count];
        for (int i = 0; i < intl.Count; i++)
            result[i] = intl[i].ToString();
        return result;
    }
    public static string[] VecArrToStringArr(Vector2[] intl)
    {
        string[] result = new string[intl.Length];
        for (int i = 0; i < intl.Length; i++)
            result[i] = intl[i].ToString();
        return result;
    }
    public static Mesh CreateMesh(Vector2[] footprint, float height, bool debug=true, string debugname="")
    {
        //Create a new mesh
        Mesh mesh = new Mesh();

        // generate a new array of vertices that includes the footprint vertices and ceiling vertices
        Vector3[] vertices = new Vector3[footprint.Length * 2];
        for(int h = 0; h < 2; h++)
        {
            for (int i = 0; i < footprint.Length; i++)
                vertices[i + h * footprint.Length] = new Vector3(footprint[i].x, h * height, footprint[i].y);
        }

        // get a triangulator object; it will triangulate the floors
        Triangulator tr = new Triangulator(footprint);

        int[] baseTris = tr.Triangulate();
        //int[] baseTris = tr.TriangulatePolygon();

        // create a new array to hold floor triangles, ceiling triangles, and wall triangles
        // baseTris.Length * (1 floor + 1 ceiling) * (1 inside + 1 outside) +
        //     footprint.Length * (2 triangles per rectangle wall) * (3 points per triangle) * (1 inside + 1 outside)
        int[] allTris = new int[baseTris.Length * 2 * 2 + footprint.Length * 2 * 3 * 2];



        int f1, f2, f3, c1, c2, c3;
        int fi1, ci1, fi2, ci2;
        // copy baseTris for footprint, make triangles for top of building too
        for (int i = 0; i < baseTris.Length; i = i + 3)
        {
            // triangle 'pointers'
            f1 = baseTris[i];
            f2 = baseTris[i + 1];
            f3 = baseTris[i + 2];
            c1 = baseTris[i] + footprint.Length;
            c2 = baseTris[i + 1] + footprint.Length;
            c3 = baseTris[i + 2] + footprint.Length;

            // indices
            fi1 = i;
            ci1 = baseTris.Length * 1+ i;
            fi2 = baseTris.Length * 2 + i;
            ci2 = baseTris.Length * 3 + i;

            // forward copying
            // floor
            allTris[fi1] = f1;
            allTris[fi1 + 1] = f2;
            allTris[fi1 + 2] = f3;

            // ceiling
            allTris[ci1] = c1;
            allTris[ci1 + 1] = c2;
            allTris[ci1 + 2] = c3;
            // \

            // reverse copying
            // floor
            allTris[fi2] = f3;
            allTris[fi2 + 1] = f2;
            allTris[fi2 + 2] = f1;

            // ceiling
            allTris[ci2] = c3;
            allTris[ci2 + 1] = c2;
            allTris[ci2 + 2] = c1;
            // \
        }


        // create sides of building
        int w1, w2, w3, w4;
        int walli1, walli2;
        for (int i = 0; i < footprint.Length; i++)
        {
            // w3 _ w4
            // |  x  |
            // w1 _ w2

            // pointers
            w1 = i;
            w2 = (i + 1) % footprint.Length;
            w3 = w1 + footprint.Length;
            w4 = w2 + footprint.Length;

            // indices
            walli1 = baseTris.Length * 4 + i * 6;
            walli2 = walli1 + footprint.Length * 6;

            // generate triangles clockwise
            allTris[walli1] = w1;
            allTris[walli1 + 1] = w3;
            allTris[walli1 + 2] = w2;
            allTris[walli1 + 3] = w2;
            allTris[walli1 + 4] = w3;
            allTris[walli1 + 5] = w4;
            // generate triangles counter-clockwise
            allTris[walli2] = w1;
            allTris[walli2 + 1] = w2;
            allTris[walli2 + 2] = w3;
            allTris[walli2 + 3] = w2;
            allTris[walli2 + 4] = w4;
            allTris[walli2 + 5] = w3;
        }

        if (debug)
        {
            List<int> triList = new List<int>(allTris);
            List<int> floor = triList.GetRange(0, baseTris.Length * 2);
            List<int> ceiling = triList.GetRange(baseTris.Length * 2, baseTris.Length * 2);
            List<int> wall = triList.GetRange(baseTris.Length * 2 * 2, footprint.Length * 2 * 3 * 2);
            Debug.Log(string.Format("vert debug for {0}\nvertices ({1}): {2}\nfloor triangles: {3}\nceiling triangles: {4}\nwall triangles: {5}",
                debugname,
                footprint.Length * 2,
                string.Join("; ", VecArrToStringArr(footprint)),
                string.Join(", ", IntListToStringArr(floor)),
                string.Join(", ", IntListToStringArr(ceiling)),
                string.Join(", ", IntListToStringArr(wall))
                ));
        }
        //Assign data to mesh
        mesh.vertices = vertices;
        //mesh.uv = uvs;
        mesh.triangles = allTris;

        //Recalculations
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

        //Name the mesh
        mesh.name = "BuildingMesh";

        //Return the mesh
        return mesh;
    }
}
#endif