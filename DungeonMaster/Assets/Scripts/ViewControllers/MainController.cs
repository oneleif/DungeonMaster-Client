using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public GameObject charactersPanel;
    public GameObject mapPanel;
    public GameObject itemsPanel;
    public GameObject npcsPanel;
    public GameObject campaignsPanel;

    public GameObject currentPanel;

    public void CleanPanels() {
        charactersPanel.SetActive(false);
        mapPanel.SetActive(false);
        itemsPanel.SetActive(false);
        npcsPanel.SetActive(false);
        campaignsPanel.SetActive(false);
    }

    public void GoToCharacters() {
        GoToPanel(charactersPanel);
    }

    public void GoToMaps() {
        GoToPanel(mapPanel);
    }

    public void GoToItems() {
        GoToPanel(itemsPanel);
    }

    public void GoToNpcs() {
        GoToPanel(npcsPanel);
    }

    public void GoToCampaigns() {
        GoToPanel(campaignsPanel);
    }

    public void GoToPanel(GameObject go) {
        CleanPanels();
        go.SetActive(true);
        currentPanel = go;
    }
}
