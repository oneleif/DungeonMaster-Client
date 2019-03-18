﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorViewController : MonoBehaviour
{
    public GameObject backgroundTabButton;
    public GameObject drawTabButton;
    public GameObject placeTabButton;

    public GameObject backgroundPanel;
    public GameObject drawPanel;
    public GameObject placePanel;

    public GameObject mapPanel;
    public GameObject mapInfoPanel;
    public GameObject mapLayersPanel;

    public Image mapImage;
    public Image backgroundColorImage;

    public GameObject imageButtonPrefab;

    WorldMap worldMap;

    public WorldMap WorldMap {
        get {
            return worldMap;
        }
        set {
            worldMap = value;
            mapImage.sprite = worldMap.image;
        }
    }

    //background layer functions
    public void ShowImages() {
        ToggleInfoPanel();
        LoadImages();
    }

    public void LoadImages()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("WorldMaps");
        foreach (Sprite sprite in sprites)
        {
            GameObject imageObject = Instantiate(imageButtonPrefab, mapInfoPanel.transform);
            Image image = imageObject.GetComponent<Image>();
            image.sprite = sprite;
            Button button = imageObject.GetComponent<Button>();
            button.onClick.AddListener(delegate
            {
                OnImageClicked(imageObject);
            });
        }
    }

    public void OnImageClicked(GameObject imageObject)
    {
        Sprite sprite = imageObject.GetComponent<Image>().sprite;
        worldMap.image = sprite;
        mapImage.sprite = sprite;
        mapInfoPanel.SetActive(false);
    }

    public void ToggleInfoPanel()
    {
        mapPanel.SetActive(!mapPanel.activeInHierarchy);
        mapInfoPanel.SetActive(!mapInfoPanel.activeInHierarchy);
    }
}
