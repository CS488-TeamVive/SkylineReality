using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TerrainEditorScript))]
public class TerrainEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainEditorScript myScript = (TerrainEditorScript)target;
        if (GUILayout.Button("Random colors"))
            myScript.randomcolors();
    }
}
