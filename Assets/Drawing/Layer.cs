using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class Layer
    {
        public RenderTexture background;

        public Layer(RenderTexture renderTexture)
        {
            background = renderTexture;
        }
    }
}

