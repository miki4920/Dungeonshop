using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Dungeonshop
{
    public class TextureView : MonoBehaviour, IPointerClickHandler, TextureReceiver
    {
        public Texture2D texture;
        public TextureReceiver textureReceiver;
        public RawImage rawImage;

        public void updateTexture(Texture2D texture)
        {
            this.texture = texture;
            rawImage.texture = texture;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            textureReceiver.updateTexture(texture);
        }
    }

}
