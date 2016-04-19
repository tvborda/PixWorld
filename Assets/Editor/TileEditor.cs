using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

[CustomEditor(typeof(Tile))]
[CanEditMultipleObjects]
public class TileEditor : Editor {
    SerializedProperty TypeProp;
    SerializedProperty MatProp;
    SerializedProperty WidthProp;
    SerializedProperty HeightProp;
    SerializedProperty OffsetXProp;
    SerializedProperty OffsetYProp;
    SerializedProperty OffsetProp;
    SerializedProperty UnitProp;
    bool xFoldout = true;
    bool yFoldout = true;

    void OnEnable() {
        // Setup the SerializedProperties.
        TypeProp = serializedObject.FindProperty("Type");
        MatProp = serializedObject.FindProperty("Mat");
        WidthProp = serializedObject.FindProperty("Width");
        HeightProp = serializedObject.FindProperty("Height");
        OffsetXProp = serializedObject.FindProperty("OffsetX");
        OffsetYProp = serializedObject.FindProperty("OffsetY");
        OffsetProp = serializedObject.FindProperty("Offset");
        UnitProp = serializedObject.FindProperty("Unit");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        int type = TypeProp.intValue;
        Material mat = (Material)MatProp.objectReferenceValue;
        int width = WidthProp.intValue;
        int height = HeightProp.intValue;
        int offsetX = OffsetXProp.intValue;
        int offsetY = OffsetYProp.intValue;
        Vector2 offset = OffsetProp.vector2Value;
        Vector2 unit = UnitProp.vector2Value;

        EditorGUILayout.Space();
        bool isRendered = EditorGUILayout.Toggle("Is Rendered", (type & 1) == 1);
        bool hasCollider = EditorGUILayout.Toggle("Has Collider", (type & 2) == 2);
        TypeProp.intValue = (isRendered ? 1 : 0) + (hasCollider ? 2 : 0);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Texture", GUILayout.Width(90));
        MatProp.objectReferenceValue = EditorGUILayout.ObjectField(mat, typeof(Material), true);
        EditorGUILayout.EndHorizontal();

        if (mat && mat.mainTexture)
        {
            if (!WidthProp.hasMultipleDifferentValues && !HeightProp.hasMultipleDifferentValues) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Tile Size");
                WidthProp.intValue = HeightProp.intValue = (int)((TileSize)EditorGUILayout.EnumPopup((TileSize)width));
                EditorGUILayout.EndHorizontal();
            }

            if (!OffsetXProp.hasMultipleDifferentValues) {
                GUILayout.Space(10);
                List<string> xValues = new List<string>();
                int selectedOffsetX = offsetX / width;
                for (int x = 0; x < mat.mainTexture.width; x = x + width) {
                    xValues.Add((x / width).ToString());
                }
                xFoldout = EditorGUILayout.Foldout(xFoldout, "Offset Axis X");
                if (xFoldout) {
                    EditorGUILayout.LabelField("Offset X: " + offsetX);
                    selectedOffsetX = GUILayout.SelectionGrid(selectedOffsetX, xValues.ToArray(), 8);
                    OffsetXProp.intValue = selectedOffsetX * width;
                }
            } else {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Offset X: Multiple Values");
                if (GUILayout.Button("Reset Offset X To 0")) {
                    OffsetXProp.intValue = 0;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (!OffsetYProp.hasMultipleDifferentValues) {
                GUILayout.Space(10);
                List<string> yValues = new List<string>();
                int selectedOffsetY = offsetY / height;
                for (int y = 0; y < mat.mainTexture.height; y = y + height) {
                    yValues.Add((y / height).ToString());
                }
                yFoldout = EditorGUILayout.Foldout(yFoldout, "Offset Axis Y");
                if (yFoldout) {
                    EditorGUILayout.LabelField("Offset Y: " + offsetY);
                    selectedOffsetY = GUILayout.SelectionGrid(selectedOffsetY, yValues.ToArray(), 8);
                    OffsetYProp.intValue = selectedOffsetY * height;
                }
            } else {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Offset Y: Multiple Values");
                if (GUILayout.Button("Reset Offset Y To 0")) {
                    OffsetYProp.intValue = 0;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (!WidthProp.hasMultipleDifferentValues && !HeightProp.hasMultipleDifferentValues &&
                !OffsetXProp.hasMultipleDifferentValues && !OffsetYProp.hasMultipleDifferentValues) {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                EditorGUILayout.LabelField("Preview:");
                EditorGUILayout.EndHorizontal();

                if (width * height != 0) {
                    var rect = EditorGUILayout.GetControlRect();
                    rect.width = rect.height = 128;
                    float unitX = width / (float)mat.mainTexture.width;
                    float unitY = height / (float)mat.mainTexture.height;
                    float offX = offsetX / (float)mat.mainTexture.width;
                    float offY = offsetY / (float)mat.mainTexture.height;
                    GUI.DrawTextureWithTexCoords(rect, mat.mainTexture, new Rect(offX, offY, unitX, unitY));
                    GUILayout.Space(128);
                } else {
                    EditorGUILayout.HelpBox("Texture size must be larger than 0", MessageType.Warning);
                }
            }

            unit.x = width / (float)mat.mainTexture.width;
            unit.y = height / (float)mat.mainTexture.height;
            offset.x = offsetX / (float)mat.mainTexture.width;
            offset.y = offsetY / (float)mat.mainTexture.height;
            UnitProp.vector2Value = unit;
            OffsetProp.vector2Value = offset;
            

        }


        //if (GUI.changed)
        //{
        //    //Calculate
        //    if (script.Mat && script.Mat.mainTexture)
        //    {
        //        script.Width = (selectedTileSize + 1) * 16;
        //        script.Height = (selectedTileSize + 1) * 16;
        //        script.OffsetX = selectedOffsetX * script.Width;
        //        script.OffsetY = selectedOffsetY * script.Height;
        //        script.Unit.x = script.Width / (float)script.Mat.mainTexture.width;
        //        script.Unit.y = script.Height / (float)script.Mat.mainTexture.height;
        //        script.Offset.x = script.OffsetX / (float)script.Mat.mainTexture.width;
        //        script.Offset.y = script.OffsetY / (float)script.Mat.mainTexture.height;
        //    }
        //    EditorUtility.SetDirty(target);
        //}

        serializedObject.ApplyModifiedProperties();
    }
}
