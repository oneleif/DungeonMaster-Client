using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapHierarchyViewController : MonoBehaviour
{
    public GameObject mapHierachyScrollView;
    public GameObject mapHierarchyLabelPrefab;

    MapHierarchyViewModel viewModel;

    WorldMap worldMap;
    public WorldMap WorldMap {
        get {
            return worldMap;
        }
        set {
            worldMap = value;
            CurrentInstanceMap = worldMap.instance;
            viewModel = new MapHierarchyViewModel();
            viewModel.UpdateLayers(worldMap);
            ShowHierarchy();
        }
    }

    InstanceMap currentMap;
    InstanceMap CurrentInstanceMap {
        get {
            return currentMap;
        }
        set {
            currentMap = value;
        }
    }

    void Start()
    {
        
    }

    void ShowHierarchy()
    {
        List<string> layers = viewModel.GetHierarchy();
        foreach(string layer in layers)
        {
            GameObject label = Instantiate(mapHierarchyLabelPrefab, mapHierachyScrollView.transform);
            MapHeirarchyLabel hierarchyLabel = label.GetComponentInChildren<MapHeirarchyLabel>();
            hierarchyLabel.HierarchyItemText.text = layer;
            hierarchyLabel.AddChildButton.onClick.RemoveAllListeners();
            hierarchyLabel.AddChildButton.onClick.AddListener(delegate {
                AddChildButtonClicked();
            });
        }
    }

    void AddChildButtonClicked() {

    }

    #region BackgroundLayer setters
    public void SetBackgroundImage(Sprite sprite) {
        currentMap.map.backgroundLayer.image = sprite;
    }

    public void SetBackgroundColor(Color color) {
        currentMap.map.backgroundLayer.color = color;
    }

    public void SetRows(int rows) {
        currentMap.map.backgroundLayer.rows = rows;
    }

    public void SetColumns(int columns) {
        currentMap.map.backgroundLayer.columns = columns;
    }
    #endregion

    #region DrawLayer settings
    public void SetDrawLineWidth(float lineWidth) {
        currentMap.map.drawLayer.lineWidth = lineWidth;
    }

    public float GetDrawLineWidth() {
        return currentMap.map.drawLayer.lineWidth;
    }

    public void SetDrawTransparency(float transparency) {
        currentMap.map.drawLayer.transparency = transparency;
    }

    public float GetDrawTransparency() {
        return currentMap.map.drawLayer.transparency;
    }
    #endregion
}
