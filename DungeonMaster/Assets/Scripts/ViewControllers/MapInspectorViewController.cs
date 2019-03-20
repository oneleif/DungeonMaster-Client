using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInspectorViewController : MonoBehaviour {
    [Header("Inspector Tabs")]
    public Button backgroundTabButton;
    public Button drawTabButton;
    public Button placeTabButton;
    [Space(10)]
    public GameObject backgroundPanel;
    public GameObject drawPanel;
    public GameObject placePanel;

    [Header("Map editor center pane")]
    public GameObject mapImageViewer;
    public GameObject mapImageSelector;

    #region BackgroundLayer variables
    [Header("BackgroundLayer")]
    public Image currentMapImage;
    public Image currentBackgroundColorImage;
    public InputField rowsInput;
    public InputField columnsInput;
    #endregion

    #region DrawLayer variables
    [Header("DrawLayer")]
    public Image currentDrawColorImage;
    public Slider lineWidthSlider;
    public InputField lineWidthValueInput;
    public Slider transparencySlider;
    public InputField transparencyValueInput;
    #endregion

    #region PlaceLayer variables
    [Header("PlaceLayer")]
    public Button playersButton;
    public Button npcsButton;
    public Button monstersButton;
    public Button itemsButton;
    #endregion

    [Space(20)]
    public GameObject imageButtonPrefab;
    public MapHierarchyViewController mapHierarchy;
    public MapInspectorViewModel viewModel;

    InstanceMap currentInstanceMap;
    public InstanceMap CurrentInstanceMap {
        get {
            return currentInstanceMap;
        }
        set {
            currentInstanceMap = value;
            UpdateLayers();
        }
    }

    MapLayers currentMapLayer {
        get {
            return currentMapLayer;
        }
        set {
            CleanLayerPanels();
            if (value == MapLayers.Background) {
                SetupBackgroundLayer();
            } else if (value == MapLayers.Draw) {
                SetupDrawLayer();
            } else if (value == MapLayers.Place) {
                SetupPlaceLayer();
            }
        }
    }

    const string COLOR_URI = "Colors/";
    const string MAPS_URI = "WorldMaps/";

    enum MapLayers {
        Background, Draw, Place
    }

    private void OnEnable() {
        currentMapLayer = MapLayers.Background;

        SetupButtons();

        viewModel = new MapInspectorViewModel();
        //TODO update draw panel for view model color/linewidth/transparency
    }

    void UpdateLayers() {
        UpdateBackgroundLayer();
        UpdateDrawLayer();
        UpdatePlaceLayer();
    }

    void UpdateBackgroundLayer() {
        currentMapImage.sprite = CurrentInstanceMap.map.backgroundLayer.image;
        currentBackgroundColorImage.sprite = CurrentInstanceMap.map.backgroundLayer.color;
        rowsInput.text = CurrentInstanceMap.map.backgroundLayer.rows.ToString();
        columnsInput.text = CurrentInstanceMap.map.backgroundLayer.columns.ToString();
    }

    void UpdateDrawLayer() {
        //update draw image
    }

    void UpdatePlaceLayer() {

    }

    void SetupButtons() {
        backgroundTabButton.onClick.RemoveAllListeners();
        backgroundTabButton.onClick.AddListener(delegate {
            OnLayerTabClicked(MapLayers.Background);
        });

        drawTabButton.onClick.RemoveAllListeners();
        drawTabButton.onClick.AddListener(delegate {
            OnLayerTabClicked(MapLayers.Draw);
        });

        placeTabButton.onClick.RemoveAllListeners();
        placeTabButton.onClick.AddListener(delegate {
            OnLayerTabClicked(MapLayers.Place);
        });
    }

    void OnLayerTabClicked(MapLayers mapLayer) {
        currentMapLayer = mapLayer;
    }

    void CleanLayerPanels() {
        backgroundPanel.SetActive(false);
        drawPanel.SetActive(false);
        placePanel.SetActive(false);
    }

    #region BackgroundLayer
    void SetupBackgroundLayer() {
        backgroundPanel.SetActive(true);

        Button mapImageButton = currentMapImage.gameObject.GetComponent<Button>();
        mapImageButton.onClick.RemoveAllListeners();
        mapImageButton.onClick.AddListener(delegate {
            ShowImages();
        });

        Button colorImageButton = currentBackgroundColorImage.gameObject.GetComponent<Button>();
        colorImageButton.onClick.RemoveAllListeners();
        colorImageButton.onClick.AddListener(delegate {
            ShowMapBackgroundColors();
        });

        rowsInput.onEndEdit.AddListener(delegate {
            SetMapRows();
        });

        columnsInput.onEndEdit.AddListener(delegate {
            SetMapColumns();
        });
    }


    void ShowImages() {
        ToggleInfoPanel();
        CleanInfoPanel();
        LoadImages();
    }

    //TODO: load map images from server
    void LoadImages() {
        Sprite[] sprites = Resources.LoadAll<Sprite>(MAPS_URI);
        foreach (Sprite sprite in sprites) {
            GameObject imageObject = Instantiate(imageButtonPrefab, mapImageSelector.transform);
            Image image = imageObject.GetComponent<Image>();
            image.sprite = sprite;
            Button button = imageObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate {
                OnBackgroundImageSelected(imageObject);
            });
        }
    }

    public void OnBackgroundImageSelected(GameObject image) {
        Sprite sprite = image.GetComponent<Image>().sprite;
        CurrentInstanceMap.map.backgroundLayer.image = sprite;
        currentMapImage.sprite = sprite;
        mapImageSelector.SetActive(false);
    }


    void ShowMapBackgroundColors() {
        ToggleInfoPanel();
        CleanInfoPanel();
        LoadBackgroundColors();
    }

    //TODO: get local preset color images for the user to choose from
    void LoadBackgroundColors() {
        Sprite[] sprites = Resources.LoadAll<Sprite>(COLOR_URI);
        foreach (Sprite sprite in sprites) {
            GameObject imageObject = Instantiate(imageButtonPrefab, mapImageSelector.transform);
            Image image = imageObject.GetComponent<Image>();
            image.sprite = sprite;
            Button button = imageObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate {
                OnBackgroundColorSelected(imageObject);
            });
        }
    }

    void OnBackgroundColorSelected(GameObject colorImage) {
        Sprite sprite = colorImage.GetComponent<Image>().sprite;
        CurrentInstanceMap.map.backgroundLayer.color = sprite;
        currentBackgroundColorImage.sprite = sprite;
        mapImageSelector.SetActive(false);
    }

    void SetMapRows() {
        int rows = -1;
        if (int.TryParse(rowsInput.text, out rows)) {
            CurrentInstanceMap.map.backgroundLayer.rows = rows;
        } else {
            rowsInput.text = "";
        }
    }

    void SetMapColumns() {
        int columns = -1;
        if (int.TryParse(columnsInput.text, out columns)) {
            CurrentInstanceMap.map.backgroundLayer.columns = columns;
        } else {
            columnsInput.text = "";
        }
    }

    public void ToggleInfoPanel() {
        mapImageViewer.SetActive(!mapImageViewer.activeInHierarchy);
        mapImageSelector.SetActive(!mapImageSelector.activeInHierarchy);
    }

    void CleanInfoPanel() {
        foreach (Transform child in mapImageSelector.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
    #endregion

    #region DrawLayer
    void SetupDrawLayer() {
        drawPanel.SetActive(true);
        Button colorImageButton = currentDrawColorImage.gameObject.GetComponent<Button>();
        colorImageButton.onClick.RemoveAllListeners();
        colorImageButton.onClick.AddListener(delegate {
            ShowDrawColors();
        });

        lineWidthSlider.onValueChanged.AddListener(delegate {
            OnLineWidthChanged(lineWidthSlider.value.ToString());
        });

        lineWidthValueInput.onValueChanged.AddListener(delegate {
            OnLineWidthChanged(lineWidthValueInput.text);
        });

        transparencySlider.onValueChanged.AddListener(delegate {
            OnTransparencyChanged(transparencySlider.value.ToString());
        });

        transparencyValueInput.onValueChanged.AddListener(delegate {
            OnTransparencyChanged(transparencyValueInput.text);
        });
    }

    void OnLineWidthChanged(string newValue) {
        float lineWidth = -1;
        if (float.TryParse(newValue, out lineWidth)) {
            lineWidthSlider.value = lineWidth;
            lineWidthValueInput.text = lineWidth.ToString();
            viewModel.SetDrawLineWidth(lineWidth);
        } else {
            lineWidthSlider.value = viewModel.GetDrawLineWidth();
            lineWidthValueInput.text = viewModel.GetDrawLineWidth().ToString();
        }
    }

    void OnTransparencyChanged(string newValue) {
        float transparency = -1;
        if (float.TryParse(newValue, out transparency)) {
            transparencySlider.value = transparency;
            transparencyValueInput.text = transparency.ToString();
            viewModel.SetDrawTransparency(transparency);
        } else {
            transparencySlider.value = viewModel.GetDrawTransparency();
            transparencyValueInput.text = viewModel.GetDrawTransparency().ToString();
        }
    }

    void ShowDrawColors() {
        ToggleInfoPanel();
        CleanInfoPanel();
        LoadDrawColors();
    }

    void LoadDrawColors() {
        Sprite[] sprites = Resources.LoadAll<Sprite>(COLOR_URI);
        foreach (Sprite sprite in sprites) {
            GameObject imageObject = Instantiate(imageButtonPrefab, mapImageSelector.transform);
            Image image = imageObject.GetComponent<Image>();
            image.sprite = sprite;
            Button button = imageObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate {
                OnColorClicked(imageObject);
            });
        }
    }

    void OnColorClicked(GameObject colorImage) {
        Sprite sprite = colorImage.GetComponent<Image>().sprite;
        viewModel.SetDrawColor(sprite.texture.GetPixel(0, 0));
        currentDrawColorImage.sprite = sprite;
        mapImageSelector.SetActive(false);
    }

    #endregion

    #region PlaceLayer
    void SetupPlaceLayer() {
        placePanel.SetActive(true);
    }
    #endregion
}
