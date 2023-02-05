using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Dungeonshop.UI
{
    [RequireComponent(typeof(Image))]
    public class SelectionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public SelectionGroup selectionGroup;
        public string selectionName;
        public Image background;

        public void OnPointerClick(PointerEventData eventData)
        {
            selectionGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            selectionGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            selectionGroup.OnTabExit(this);
        }

        void Start()
        {
            background = GetComponent<Image>();
            selectionGroup.Subscribe(this);
        }
    }

}
