using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class Layer
    {
        public RenderTexture background;
        public bool visible = false;
        public List<GameObject> objects;
        public Guid layerID;

        public Layer(RenderTexture renderTexture)
        {
            this.visible = true;
            this.background = renderTexture;
            this.objects = new List<GameObject>();
        }

        public void changeVisibility()
        {
            visible = !visible;
            BackgroundManager.Instance.uniteLayers();
        }
    }
}

