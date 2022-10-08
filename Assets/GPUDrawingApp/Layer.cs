using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public struct Layer
    {
        public RenderTexture background;

        public Layer(RenderTexture renderTexture)
        {
            background = renderTexture;
        }
    }
}

