using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Dungeonshop.UI
{
    public class TextureView : MonoBehaviour, IPointerClickHandler
    {
        Texture2D texture;
        void Start()
        {
            texture = (Texture2D) gameObject.GetComponent<RawImage>().texture;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Dungeonshop.UI.BrushSelectorManager.Instance.updateTexture(texture);
        }
    }

}
