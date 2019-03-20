using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapHierarchyViewController : MonoBehaviour {
    public GameObject mapHierachyScrollView;
    public GameObject mapHierarchyLabelPrefab;
    public MapInspectorViewController mapInspector;

    MapHierarchyViewModel viewModel;

    WorldMap worldMap;

    List<MapHierarchyLabel> mapHierarchyLabels;

    public WorldMap WorldMap {
        get {
            return worldMap;
        }
        set {
            worldMap = value;
            viewModel = new MapHierarchyViewModel();

            CurrentInstanceMap = worldMap.instance;
        }
    }

    InstanceMap currentMap;
    InstanceMap CurrentInstanceMap {
        get {
            return currentMap;
        }
        set {
            currentMap = value;
            mapInspector.CurrentInstanceMap = currentMap;
            CleanHierarchy();
            ShowHierarchy();
            HighlightCurrentLabel();
        }
    }

    void HighlightCurrentLabel() {
        foreach (MapHierarchyLabel label in mapHierarchyLabels) {
            if (label.instanceMap == currentMap) {
                label.HierarchyItemText.fontStyle = FontStyle.Bold;
            } else {
                label.HierarchyItemText.fontStyle = FontStyle.Normal;
            }
        }
    }

    void CleanHierarchy() {
        foreach (Transform child in mapHierachyScrollView.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    void ShowHierarchy() {
        List<MapLayer> layers = viewModel.GetHierarchy(worldMap);
        mapHierarchyLabels = new List<MapHierarchyLabel>();
        foreach (MapLayer layer in layers) {
            GameObject label = Instantiate(mapHierarchyLabelPrefab, mapHierachyScrollView.transform);
            MapHierarchyLabel hierarchyLabel = label.GetComponent<MapHierarchyLabel>();
            hierarchyLabel.HierarchyItemText.text = new string('>', layer.level) + layer.instance.name;
            hierarchyLabel.instanceMap = layer.instance;
            hierarchyLabel.HierarchyItemButton.onClick.RemoveAllListeners();
            hierarchyLabel.HierarchyItemButton.onClick.AddListener(delegate {
                OnHierarchyItemSelected(hierarchyLabel);
            });

            hierarchyLabel.AddChildButton.onClick.RemoveAllListeners();
            hierarchyLabel.AddChildButton.onClick.AddListener(delegate {
                AddChildButtonClicked(hierarchyLabel);
            });

            mapHierarchyLabels.Add(hierarchyLabel);
        }
    }

    void OnHierarchyItemSelected(MapHierarchyLabel label) {
        CurrentInstanceMap = label.instanceMap;
    }

    void AddChildButtonClicked(MapHierarchyLabel label) {
        InstanceMap newInstance = new InstanceMap();
        label.instanceMap.regions.Add(newInstance);
        CurrentInstanceMap = newInstance;
    }
}
