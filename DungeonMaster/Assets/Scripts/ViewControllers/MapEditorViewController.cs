using System.Collections;
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

    const string COLOR_URI = "Colors";
    const string MAPS_URI = "WorldMaps";

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

    Map currentMap;
    public Map CurrentMap {
        get {
            return currentMap;
        }
        set {
            currentMap = value;
        }
    }

    private void OnEnable() {
        Button mapImageButton = mapImage.gameObject.GetComponent<Button>();
        mapImageButton.onClick.RemoveAllListeners();
        mapImageButton.onClick.AddListener(delegate
        {
            ShowImages();
        });

        Button colorImageButton = backgroundColorImage.gameObject.GetComponent<Button>();
        colorImageButton.onClick.RemoveAllListeners();
        colorImageButton.onClick.AddListener(delegate 
        {
            ShowColors();
        });
    }

    void ShowImages() {
        ToggleInfoPanel();
        CleanInfoPanel();
        LoadImages();
    }

    void LoadImages()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(MAPS_URI);
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

    void ShowColors() {
        ToggleInfoPanel();
        CleanInfoPanel();
        LoadColors();
    }

    void LoadColors() {
        Sprite[] sprites = Resources.LoadAll<Sprite>(COLOR_URI);
        foreach (Sprite sprite in sprites) {
            GameObject imageObject = Instantiate(imageButtonPrefab, mapInfoPanel.transform);
            Image image = imageObject.GetComponent<Image>();
            image.sprite = sprite;
            Button button = imageObject.GetComponent<Button>();
            button.onClick.AddListener(delegate {
                OnImageClicked(imageObject);
            });
        }
    }

    void OnColorClicked(GameObject colorImage) {
        Sprite sprite = colorImage.GetComponent<Image>().sprite;
        worldMap.backgroundColor = sprite.texture.GetPixel(0,0);
        mapImage.sprite = sprite;
        mapInfoPanel.SetActive(false);
    }

    public void OnImageClicked(GameObject image)
    {
        Sprite sprite = image.GetComponent<Image>().sprite;
        worldMap.image = sprite;
        mapImage.sprite = sprite;
        mapInfoPanel.SetActive(false);
    }

    public void ToggleInfoPanel()
    {
        mapPanel.SetActive(!mapPanel.activeInHierarchy);
        mapInfoPanel.SetActive(!mapInfoPanel.activeInHierarchy);
    }

    void CleanInfoPanel() {
        foreach (Transform child in mapInfoPanel.transform) {
            GameObject.Destroy(child);
        }
    }
}
