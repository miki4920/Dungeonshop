using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonshop
{
    public class PanelView : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] GameObject viewControllerObject;
        PanelManagerViewController viewController;
        void Start()
        {
            viewController = viewControllerObject.GetComponent<PanelManagerViewController>();
        }

        public void changePanel()
        {
            viewController.changeActivePanel(panel);
        }
    }
}

