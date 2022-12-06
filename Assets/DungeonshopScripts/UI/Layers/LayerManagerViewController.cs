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
        int layerCount;

        public void Start()
        {
            layerCount = 1;
        }

        public void addLayer()
        {
            Layer blankLayer = Dungeonshop.LayerManager.Instance.createNewLayer();
            GameObject layerContainerParent = Instantiate(this.layerContainerPrefab, this.layersContainer);
            layerContainerParent.GetComponent<LayerDragAndDrop>().viewController = this;
            GameObject layerContainer = layerContainerParent.transform.GetChild(0).gameObject;
            layerContainer.GetComponent<LayerView>().layer = blankLayer;
            layerContainer.GetComponent<LayerView>().viewController = this;

            layerContainer.transform.GetChild(1).GetComponent<RawImage>().texture = blankLayer.background;
            layerContainer.transform.GetChild(2).GetComponent<TMP_Text>().text = "Layer " + layerCount.ToString();
            layerCount += 1;
        }

        public void changeLayer(GameObject layerObject)
        {
            Layer layer = layerObject.GetComponent<LayerView>().layer;
            for (int i = 0; i < Dungeonshop.LayerManager.Instance.layers.Count; i++)
            {
                if (layer == Dungeonshop.LayerManager.Instance.layers[i])
                {
                    Dungeonshop.LayerManager.Instance.layer = i;
                }
            }
        }

        public void deleteLayer(GameObject layerObject)
        {
            Layer layer = layerObject.GetComponent<LayerView>().layer;
            for (int i = 0; i < Dungeonshop.LayerManager.Instance.layers.Count; i++)
            {
                if (layer == Dungeonshop.LayerManager.Instance.layers[i])
                {
                    Dungeonshop.LayerManager.Instance.deleteLayer(i);
                    Destroy(layerObject);
                }
            }
        }

        public void changeLayerPosition(int currentIndex, int newIndex)
        {
            Layer layer = Dungeonshop.LayerManager.Instance.layers[currentIndex];
            Dungeonshop.LayerManager.Instance.layers.RemoveAt(currentIndex);
            if (newIndex > currentIndex)
            {
                newIndex -= 1;
            }
            Dungeonshop.LayerManager.Instance.layers.Insert(newIndex, layer);
        }
    }
}

