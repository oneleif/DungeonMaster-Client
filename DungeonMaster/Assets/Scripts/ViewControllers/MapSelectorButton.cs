using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectorButton : MonoBehaviour
{
    public Image mapImage;
    public Text mapName;
    public Button editButton;
    public Button deleteButton;

    WorldMap map;

    MapsViewController mapViewController;

    public WorldMap Map {
        get {
            return map;
        }
        set {
            map = value;
            UpdateUI();
        }
    }

    private void Start() {
        mapViewController = GetComponentInParent<MapsViewController>();
    }

    void UpdateUI() {
        mapImage.sprite = map.instance.map.backgroundLayer.image;
        mapName.text = map.instance.name;

        editButton.onClick.RemoveAllListeners();
        editButton.onClick.AddListener(delegate {
            Edit();
        });

        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(delegate {
            Delete();
        });
    }

    public void Edit() {
        mapViewController.CurrentWorldMap = map;
    }

    public void Delete() {
        mapViewController.DeleteMap(map);
    }
}
