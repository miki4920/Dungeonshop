using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop.UI
{
    public class Light : ColorReceiver
    {
        [SerializeField] Color color;
        public override void updateColor(Color color)
        {
            this.color = color;
        }
    }
}

