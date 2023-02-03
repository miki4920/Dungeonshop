using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class Layer
    {
        public RenderTexture background;
        public bool visible = false;
        public List<GameObject> lights;

        public Layer(RenderTexture renderTexture)
        {
            this.visible = true;
            this.background = renderTexture;
            this.lights = new List<GameObject>();
        }

        public void changeVisibility()
        {
            visible = !visible;
        }
    }
}

