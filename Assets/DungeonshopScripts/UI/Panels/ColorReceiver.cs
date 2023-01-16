using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop.UI
{
    public abstract class ColorReceiver : MonoBehaviour
    {
        public static ColorReceiver Instance;
        public abstract void updateColor(Color color);
    }
}
