using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dungeonshop
{
    public class LayerManagerViewController : MonoBehaviour
    {
        [SerializeField] Transform layersContainer;
        [SerializeField] GameObject layerContainerPrefab;
        [SerializeField] Color notSelected;
        [SerializeField] Color selected;
        int layerCount;

        public void Start()
        {
            layerCount = 1;
            addLayer();
        }

        public void addLayer()
        {
            Layer blankLayer = CanvasManager.Instance.createNewLayer();
            GameObject layerContainerParent = Instantiate(this.layerContainerPrefab, this.layersContainer);
            layerContainerParent.GetComponent<LayerDragAndDrop>().viewController = this;
            GameObject layerContainer = layerContainerParent.transform.GetChild(0).gameObject;
            layerContainer.GetComponent<LayerView>().layer = blankLayer;
            layerContainer.GetComponent<LayerView>().viewController = this;

            layerContainer.transform.GetChild(1).GetComponent<RawImage>().texture = blankLayer.background;
            layerContainer.transform.GetChild(2).GetComponent<TMP_Text>().text = "Layer " + layerCount.ToString();
            layerCount += 1;
            changeLayer(layerContainer);
        }

        public void changeLayer(GameObject layerObject)
        {
            Layer layer = layerObject.GetComponent<LayerView>().layer;
            for (int i = 0; i < CanvasManager.Instance.layers.Count; i++)
            {
                layersContainer.GetChild(i).GetComponent<Image>().color = notSelected;
                if (layer == CanvasManager.Instance.layers[i])
                {
                    CanvasManager.Instance.layer = i;
                    layersContainer.GetChild(i).GetComponent<Image>().color = selected;
                }
            }
        }

        public void deleteLayer(GameObject layerObject)
        {
            Layer layer = layerObject.GetComponent<LayerView>().layer;
            for (int i = 0; i < CanvasManager.Instance.layers.Count; i++)
            {
                if (layer == CanvasManager.Instance.layers[i])
                {
                    CanvasManager.Instance.deleteLayer(i);
                    Destroy(layerObject);
                }
            }
        }

        public void changeLayerPosition(int currentIndex, int newIndex)
        {
            Layer layer = CanvasManager.Instance.layers[currentIndex];
            CanvasManager.Instance.layers.RemoveAt(currentIndex);
            if (newIndex > currentIndex)
            {
                newIndex -= 1;
            }
            CanvasManager.Instance.layers.Insert(newIndex, layer);
        }
    }
}

