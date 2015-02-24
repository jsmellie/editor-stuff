using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ResourceRefChecker : EditorWindow
{
  #region -= INTERNAL CLASSES =-

  public class ResourceReference
  {
    #region -= FIELDS AND PROPERTIES

    public Sprite sprite = null;

    public bool showSceneObjects = false;
    public List<GameObject> sceneObjets = new List<GameObject>();

    public bool showPrefabObjects = false;
    public List<GameObject> prefabObjects = new List<GameObject>();

    #endregion
  }

  #endregion

  #region -= FIELDS AND PROPERTIES =-

  /// <summary>
  /// Bool used to force the script to recompile the list game objects
  /// </summary>
  bool _shouldRefresh = false;

  List<ResourceReference> _refs = null;

  #endregion

  #region -= METHODS =-

  public ResourceRefChecker()
  {
    _shouldRefresh = true;
  }

  void Refresh()
  {
    if (_refs != null)
    {
      _refs.Clear();
    }
    else
    {
      _refs = new List<ResourceReference>();
    }

    GameObject[] gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

    List<GameObject> prefabObjects = new List<GameObject>();
    List<GameObject> sceneObjects = new List<GameObject>();

    GameObject go = null;

    for (int counter = 0; counter < gameObjects.Length; ++counter)
    {
      go = gameObjects[counter];

      //Object is the prefab object in the editor and therefor isn't valid in the scene
      if (PrefabUtility.GetPrefabParent(go) == null && PrefabUtility.GetPrefabObject(go) != null)
      {
        prefabObjects.Add(go);
      }
      //Hidden Editor only object that get spawned (SceneLights and SceneCameras)
      else if(go.GetInstanceID() < 0 && go.hideFlags == HideFlags.HideAndDontSave)
      {
        //TODO: Some internal objects have HideFlags and have a negative instance ID (meaning they aren't dynamically spawned)
        continue;
      }
      //If the object has a path then it isn't valid and is loaded from the library folder
      else if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go)))
      {
        continue;
      }
        //Have a valid scene object
      else
      {
        sceneObjects.Add(go);
      }
    }

    SpriteRenderer srenderer = null;
    Image image = null;
    //RawImage rawImage = null;

    for (int sceneCounter = 0; sceneCounter < sceneObjects.Count; ++sceneCounter)
    {
      go = sceneObjects[sceneCounter];

      srenderer = go.GetComponent<SpriteRenderer>();
      image = go.GetComponent<Image>();
      //rawImage = go.GetComponent<RawImage>();

      if (srenderer != null)
      {
        if (srenderer.sprite != null)
        {
          UpdateReferences(srenderer.sprite, go, true);
        }
      }
      if (image != null)
      {
        if (image.sprite != null)
        {
          UpdateReferences(srenderer.sprite, go, true);
        }
      }
    }
  }


  #region -= HELPER METHODS

  ResourceReference UpdateReferences(Sprite sprite, GameObject go, bool isSceneObject)
  {
    for (int i = 0; i < _refs.Count; ++i)
    {
      if (_refs[i].sprite == sprite)
      {
        if (isSceneObject)
        {
          _refs[i].sceneObjets.Add(go);
        }
        else
        {
          _refs[i].prefabObjects.Add(go);
        }

        return _refs[i];
      }
    }

    ResourceReference newRef = new ResourceReference();

    newRef.sprite = sprite;

    if (isSceneObject)
    {
      newRef.sceneObjets.Add(go);
    }
    else
    {
      newRef.prefabObjects.Add(go);
    }

    _refs.Add(newRef);

    return newRef;
  }

  #endregion

  #endregion



  // Add menu named "My Window" to the Window menu
  [MenuItem ("Window/Resource Ref Checker")]
  static void Init () 
  {
    // Get existing open window or if none, make a new one:
    ResourceRefChecker window = (ResourceRefChecker)EditorWindow.GetWindow (typeof (ResourceRefChecker));
  }
  
  void OnGUI ()
  {
    //Button used to refresh all the current numbers on the window
    if (GUILayout.Button("Refresh"))
    {
      _shouldRefresh = true;
    }

    if (_shouldRefresh)
    {
      Refresh();
    }

    _shouldRefresh = false;

    if (_refs == null)
    {
      return;
    }

    //Render all the resource references we have
    for (int i = 0; i < _refs.Count; ++i)
    {
      EditorGUILayout.BeginHorizontal();
      {
        EditorGUILayout.LabelField(_refs[i].sprite.name);

        EditorGUILayout.BeginVertical();
        {
          _refs[i].showSceneObjects = EditorGUILayout.Foldout(_refs[i].showSceneObjects, "Scene Objects");

          if (_refs[i].showSceneObjects)
          {
            for (int sceneCounter = 0; sceneCounter < _refs[i].sceneObjets.Count; ++sceneCounter)
            {
              if (GUILayout.Button(_refs[i].sceneObjets[sceneCounter].name))
              {
                EditorGUIUtility.PingObject(_refs[i].sceneObjets[sceneCounter]);
              }
            }
          }
        }
        EditorGUILayout.EndVertical();
      }
      EditorGUILayout.EndHorizontal();
    }
  }
}
