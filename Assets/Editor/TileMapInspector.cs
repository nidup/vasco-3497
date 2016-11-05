using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(TilingEngine))]
public class TileMapInspector : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if(GUILayout.Button("Regenerate")) {
            TilingEngine engine = (TilingEngine) target;
            engine.Awake();
            engine.Start();
            engine.Update();
        }
    }
}
