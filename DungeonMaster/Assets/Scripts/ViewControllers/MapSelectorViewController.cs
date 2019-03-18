using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectorViewController : MonoBehaviour {
    public GameObject mapSelectorScrollView;
    public GameObject mapSelectorButtonPrefab;
    public Button createMapButton;

    WorldMap[] maps;

    MapsViewController mapViewController;

    void Start()
    {
        mapViewController = GetComponentInParent<MapsViewController>();

        createMapButton.onClick.RemoveAllListeners();
        createMapButton.onClick.AddListener(delegate
        {
            OnCreateMapClicked();
        });

        maps = GetMaps();
        LoadMapButtons();
    }

    void OnCreateMapClicked() {
        WorldMap map = new WorldMap("placeholder name");
        mapViewController.CurrentWorldMap = map;
    }

    //TODO: load maps
    WorldMap[] GetMaps() {
        WorldMap[] worldMaps = new WorldMap[] {
            new WorldMap("map1"),
            new WorldMap("map2"),
            new WorldMap("map3")
        };

        return worldMaps;
    }

    void LoadMapButtons() {
        foreach(WorldMap map in maps) {
            GameObject button = Instantiate(mapSelectorButtonPrefab, mapSelectorScrollView.transform);
            MapSelectorButton mapSelectorButton = button.GetComponent<MapSelectorButton>();
            mapSelectorButton.Map = map;
        }
    }
}
