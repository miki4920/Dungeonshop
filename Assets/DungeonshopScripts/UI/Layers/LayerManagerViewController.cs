using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dungeonshop;
using TMPro;

namespace Dungeonshop.UI
{
    public class LayerManagerViewController : MonoBehaviour
    {
        [SerializeField] Transform layersContainer;
        [SerializeField] GameObject layerContainerPrefab;
        List<GameObject> layerContainers = new List<GameObject>();
        int layerCount;

        public void Start()
        {
            layerCount = 1;
        }

        public void addLayer()
        {
            Layer blankLayer = Dungeonshop.LayerManager.Instance.createNewLayer();
            GameObject layerContainer = Instantiate(this.layerContainerPrefab, this.layersContainer);
            layerContainer.GetComponent<LayerView>().layer = blankLayer;
            layerContainer.GetComponent<LayerView>().viewController = this;
            
            layerContainer.transform.GetChild(1).GetComponent<RawImage>().texture = blankLayer.background;
            layerContainer.transform.GetChild(2).GetComponent<TMP_Text>().text = "Layer " + layerCount.ToString();
            layerCount += 1;
            this.layerContainers.Insert(Dungeonshop.LayerManager.Instance.layer, layerContainer);
        }

        public void changeLayer(Layer layer)
        {
            for (int i = 0; i < Dungeonshop.LayerManager.Instance.layers.Count; i++)
            {
                if (layer == Dungeonshop.LayerManager.Instance.layers[i]) {
                    Dungeonshop.LayerManager.Instance.layer = i;
                }
            }
        }

        public void deleteLayer(Layer layer)
        {
            for (int i = 0; i < Dungeonshop.LayerManager.Instance.layers.Count; i++)
            {
                if (layer == Dungeonshop.LayerManager.Instance.layers[i])
                {
                    Dungeonshop.LayerManager.Instance.deleteLayer(i);
                    Destroy(layerContainers[i]);
                    layerContainers.RemoveAt(i);
                }
            }
        }
    }
}

