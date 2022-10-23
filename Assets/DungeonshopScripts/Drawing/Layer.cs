using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class Layer
    {
        public RenderTexture background;
        public bool visible = false;

        public Layer(RenderTexture renderTexture)
        {
            this.visible = true;
            this.background = renderTexture;
        }

        public void changeVisibility()
        {
            visible = !visible;
        }
    }
}

