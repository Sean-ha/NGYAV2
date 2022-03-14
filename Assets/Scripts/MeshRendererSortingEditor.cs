using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

/// This just exposes the Sorting Layer / Order in MeshRenderer since it's there
/// but not displayed in the inspector. Getting MeshRenderer to render in front
/// of a SpriteRenderer is pretty hard without this.
/// Adapted from https://gist.github.com/sinbad/bd0c49bc462289fa1a018ffd70d806e3
/// With changes from https://forum.unity.com/threads/extending-mesh-renderer-component-with-a-custom-editor.949176/
/// to preserve the Unity MeshRenderer GUI. 
[CustomEditor(typeof(MeshRenderer))]
[CanEditMultipleObjects]
public class MeshRendererSortingEditor : Editor
{
   private Editor defaultEditor;
   private MeshRenderer meshRenderer;
   private static bool showSorting = true;
   private string header = "2D Sorting";

   private SerializedProperty sortingLayerIdProperty;
   private SerializedProperty sortingOrderProperty;

   [InitializeOnLoadMethod]
   private static void OnLoad()
   {
      showSorting = EditorPrefs.GetBool("MeshRendererSortingEditor.showSorting");
   }

   private void OnEnable()
   {
      defaultEditor = CreateEditor(targets, Type.GetType("UnityEditor.MeshRendererEditor, UnityEditor"));
      meshRenderer = target as MeshRenderer;

      sortingLayerIdProperty = serializedObject.FindProperty("m_SortingLayerID");
      sortingOrderProperty = serializedObject.FindProperty("m_SortingOrder");
   }

   private void OnDisable()
   {
      //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
      //Also, make sure to call any required methods like OnDisable
      var disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
      if (disableMethod != null)
         disableMethod.Invoke(defaultEditor, null);
      DestroyImmediate(defaultEditor);
   }

   public override void OnInspectorGUI()
   {
      defaultEditor.OnInspectorGUI();

      serializedObject.Update();

      EditorGUI.BeginChangeCheck();
      showSorting = EditorGUILayout.BeginFoldoutHeaderGroup(showSorting, header);
      if (EditorGUI.EndChangeCheck())
         EditorPrefs.SetBool("MeshRendererSortingEditor.showSorting", showSorting);

      if (showSorting)
      {
         EditorGUI.indentLevel++;

         var rect = EditorGUILayout.GetControlRect();
         EditorGUI.BeginProperty(rect, new GUIContent("Sorting Layer"), sortingLayerIdProperty);
         EditorGUI.BeginChangeCheck();
         var newId = DrawSortingLayersPopup(rect, meshRenderer.sortingLayerID);
         if (EditorGUI.EndChangeCheck())
            sortingLayerIdProperty.intValue = newId;
         EditorGUI.EndProperty();

         EditorGUILayout.PropertyField(sortingOrderProperty);

         EditorGUI.indentLevel--;
      }

      serializedObject.ApplyModifiedProperties();
   }

   private static int DrawSortingLayersPopup(Rect rect, int layerID)
   {
      var layers = SortingLayer.layers;
      var names = layers.Select(l => l.name).ToArray();

      if (!SortingLayer.IsValid(layerID))
         layerID = SortingLayer.NameToID("Default");

      var index = 0;
      for (int i = 0; i < layers.Length; i++) //No IndexOf in LINQ unfortunately
         if (layers[i].id == layerID)
            index = i;

      index = EditorGUI.Popup(rect, "Sorting Layer", index, names);

      return layers[index].id;
   }
}