using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dungeonshop;

namespace Dungeonshop.UI
{
    public class LayerManagerViewController : MonoBehaviour
    {
        [SerializeField] Transform layersContainer;
        [SerializeField] GameObject layerContainerPrefab;
        List<GameObject> layerContainers = new List<GameObject>();
        
        public void addLayer()
        {
            Dungeonshop.LayerManager.Instance.createNewLayer();
            GameObject layerContainer = Instantiate(this.layerContainerPrefab, this.layersContainer);
            this.layerContainers.Insert(Dungeonshop.LayerManager.Instance.layer, layerContainer);
        }

        public void removeLayer()
        {
            Destroy(layerContainers[Dungeonshop.LayerManager.Instance.layer]);
            layerContainers.RemoveAt(Dungeonshop.LayerManager.Instance.layer);
            Dungeonshop.LayerManager.Instance.deleteCurrentLayer();
        }

        public void Update()
        {
            
        }
    }
}

