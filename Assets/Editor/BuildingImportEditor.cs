#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

using System.Xml;
using System.IO;

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
                                if (xr.GetAttribute("k").Equals("building") && xr.GetAttribute("v").Contains("yes"))
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

        int buildingsOOB = 0;

        foreach(MapFeature mf in mapFeatures)
        {
            double[] firstCoordinates = mf.GetNodeCoordinates(0);
            double firstNorthing, firstEasting;
            string firstZone;

            CoordinateTranslation.LatLongtoUTM(
                firstCoordinates[0],
                firstCoordinates[1],
                out firstNorthing, out firstEasting, out firstZone);


            activeTerrain = null;
            activeTranslation = null;
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


            Vector2[] relativeNodes = new Vector2[mf.NodeCount()];
            int c = 0;
            foreach(long l in mf.ndreferences)
            {
                double northing, easting;
                string zone;

                CoordinateTranslation.LatLongtoUTM(ndLookup[l][0], ndLookup[l][1], out northing, out easting, out zone);

                relativeNodes[c++] = new Vector2((float)(northing - firstNorthing), (float)(easting - firstEasting));
            }
            GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
            building.name = mf.name;
            building.transform.parent = activeTerrain.transform;

            Mesh mesh = CreateMesh(relativeNodes, mf.height * 3.048f);
            building.GetComponent<MeshFilter>().mesh = mesh;

            Vector2 relative = activeTranslation.RelativePosition((int)firstNorthing, (int)firstEasting);

            building.transform.position = new Vector3(relative.x, 0, relative.y);

        }

        EditorUtility.DisplayDialog("Finished importing from OSM", "Importing was completed with " + (mapFeatures.Count - buildingsOOB) + "/" + mapFeatures.Count + " in bounds", "Okay");

    }

    public static Mesh CreateMesh(Vector2[] footprint, float height)
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