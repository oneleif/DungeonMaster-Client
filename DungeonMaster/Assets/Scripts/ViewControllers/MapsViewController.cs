using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsViewController : MonoBehaviour
{
    public GameObject mapEditorPanel;
    public GameObject mapSelectorPanel;

    WorldMap worldMap;

    void Start()
    {
        mapSelectorPanel.SetActive(true);
        mapEditorPanel.SetActive(false);
    }

    public void SelectMap(WorldMap worldMap)
    {
        this.worldMap = worldMap;
        mapSelectorPanel.SetActive(false);
        mapEditorPanel.SetActive(true);
    }
}
