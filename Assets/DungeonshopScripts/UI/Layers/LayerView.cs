using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Dungeonshop;
using UnityEngine.UI;

namespace Dungeonshop
{
    public class LayerView : MonoBehaviour, IPointerClickHandler
    {
        public Layer layer;
        public LayerManagerViewController viewController;

        public void Update()
        {
            transform.GetChild(1).GetComponent<RawImage>().texture = layer.background;
        }

        public void changeVisibility()
        {
            layer.changeVisibility();
        }

        public void delete()
        {
            viewController.deleteLayer(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
           viewController.changeLayer(gameObject);
        }
    }
}

