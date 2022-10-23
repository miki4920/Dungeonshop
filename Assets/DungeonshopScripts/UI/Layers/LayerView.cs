using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dungeonshop;

namespace Dungeonshop.UI
{
    public class LayerView : MonoBehaviour, IPointerClickHandler
    {
        public Layer layer;
        public LayerManagerViewController viewController;

        public void changeVisibility()
        {
            layer.changeVisibility();
        }

        public void delete()
        {
            viewController.deleteLayer(layer);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
           viewController.changeLayer(layer);
        }
    }
}

