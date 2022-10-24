using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop.UI
{
    public class PanelView : MonoBehaviour
    {
        [SerializeField] DrawingMode drawingMode;

        public void changePanel()
        {
            Dungeonshop.UI.BrushSelectorManager.Instance.changeDrawingMode(drawingMode);
        }
    }
}

