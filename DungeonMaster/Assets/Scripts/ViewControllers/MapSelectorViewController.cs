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

    void Start() {
        mapViewController = GetComponentInParent<MapsViewController>();

        createMapButton.onClick.RemoveAllListeners();
        createMapButton.onClick.AddListener(delegate {
            OnCreateMapClicked();
        });

        maps = GetMaps();
        LoadMapButtons();
    }

    void OnCreateMapClicked() {
        WorldMap map = new WorldMap("placeholder name");
        mapViewController.CurrentWorldMap = map;
    }

    //put in the view model
    //TODO: load maps from server
    WorldMap[] GetMaps() {
        WorldMap worldMap = new WorldMap("worldMap1");
        InstanceMap instance1 = new InstanceMap("instance1");
        instance1.regions.Add(new InstanceMap("instance11"));
        instance1.regions.Add(new InstanceMap("instance12"));

        InstanceMap instance2 = new InstanceMap("instance2");
        InstanceMap instance21 = new InstanceMap("instance21");
        instance21.regions.Add(new InstanceMap("instance211"));
        instance2.regions.Add(instance21);

        InstanceMap instance3 = new InstanceMap("instance3");

        worldMap.instance.regions.Add(instance1);
        worldMap.instance.regions.Add(instance2);
        worldMap.instance.regions.Add(instance3);
        WorldMap[] worldMaps = new WorldMap[] {
            worldMap,
            new WorldMap("map2"),
            new WorldMap("map3")
        };

        return worldMaps;
    }

    void LoadMapButtons() {
        foreach (WorldMap map in maps) {
            GameObject button = Instantiate(mapSelectorButtonPrefab, mapSelectorScrollView.transform);
            MapSelectorButton mapSelectorButton = button.GetComponent<MapSelectorButton>();
            mapSelectorButton.Map = map;
        }
    }
}
