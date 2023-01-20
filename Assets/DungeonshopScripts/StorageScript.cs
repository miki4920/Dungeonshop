using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop.UI
{

    public enum UndoElement
    {
        Layer,
        Light
    }
    public class StorageScript : MonoBehaviour
    {
        public static StorageScript Instance;
        private List<UndoElement> undoElements;
        private List<List<Layer>> layerUndo;
        private List<List<GameObject>> lightsUndo;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }

}
