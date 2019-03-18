using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsViewController : MonoBehaviour
{
    public GameObject mapEditorPanel;
    public MapEditorViewController mapEditor;
    public GameObject mapSelectorPanel;

    WorldMap currentWorldMap;

    public WorldMap CurrentWorldMap {
        get {
            return currentWorldMap;
        }
        set {
            currentWorldMap = value;
            GoToMapEditor();
        }
    }

    private void OnEnable() {
        mapSelectorPanel.SetActive(true);
        mapEditorPanel.SetActive(false);
    }

    public void GoToMapEditor()
    {
        mapSelectorPanel.SetActive(false);
        mapEditorPanel.SetActive(true);
        mapEditor.WorldMap = currentWorldMap;
    }

    public void DeleteMap(WorldMap map) {
        //TODO: Send request to delete map
    }
}
