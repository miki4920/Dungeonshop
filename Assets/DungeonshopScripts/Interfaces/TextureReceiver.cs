using UnityEngine;

namespace Dungeonshop
{
    public interface TextureReceiver
    {
        public abstract void updateTexture(Texture2D texture);
    }
}