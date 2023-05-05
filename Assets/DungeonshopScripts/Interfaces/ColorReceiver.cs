using UnityEngine;

namespace Dungeonshop
{
    public abstract class ColorReceiver : MonoBehaviour
    {
        public static ColorReceiver Instance;
        public abstract void updateColor(Color color);
    }
}
