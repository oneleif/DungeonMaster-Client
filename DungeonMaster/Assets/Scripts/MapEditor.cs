using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour
{
    public GameObject backgroundTabButton;
    public GameObject drawTabButton;
    public GameObject placeTabButton;

    public GameObject backgroundPanel;
    public GameObject drawPanel;
    public GameObject placePanel;

    public GameObject mapPanel;
    public GameObject mapInfoPanel;

    public Image mapImage;
    public Image backgroundColorImage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //background layer functions
    public void ShowImages() {
        mapPanel.SetActive(false);
        mapInfoPanel.SetActive(true);

        LoadImages();
    }

    void LoadImages() {
        Sprite[] sprites = Resources.LoadAll<Sprite>("");
        Debug.Log("");
    }

    void SelectImage(Sprite sprite) {
        mapPanel.SetActive(true);
        mapInfoPanel.SetActive(false);

        mapImage.sprite = sprite;
    }
}
