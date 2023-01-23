using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class UndoStorage : MonoBehaviour
    {
        [HideInInspector] public static UndoStorage Instance;
        private List<CanvasManager> undoList();
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
