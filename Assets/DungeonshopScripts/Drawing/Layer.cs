using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class Layer
    {
        public RenderTexture background;
        bool visible = false;
        public Layer(RenderTexture renderTexture)
        {
            visible = true;
            background = renderTexture;
        }

        public void makeVisible()
        {
            visible = true;
        }

        public void makeInvisible()
        {
            visible = false;
        }
    }
}

