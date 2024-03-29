using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Dungeonshop
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public TabGroup tabGroup;
        public string tabName;
        public Image background;
        public GameObject associatedObject;
        public TabGroup child;

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit();
        }

        void Start()
        {
            background = GetComponent<Image>();
            tabGroup.Subscribe(this);
        }
    }

}
