using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayersViewController : MonoBehaviour
{
    MapLayersViewModel viewModel;
    GameObject mapLayerPrefab;
    //WorldMap worldMap;

    void Start()
    {
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
