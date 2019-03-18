using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayersViewController : MonoBehaviour
{
    MapLayersViewModel viewModel;
    GameObject mapLayerPrefab;

    void Start()
    {
        viewModel = new MapLayersViewModel();
        LoadLayers();
    }

    void LoadLayers()
    {
        List<string> layers = viewModel.GetLayers();
        foreach(string s in layers)
        {
            Instantiate(mapLayerPrefab, gameObject.transform);
        }
    }

    void Update()
    {
        
    }
}
