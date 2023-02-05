using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeonshop.UI
{
    public class SelectionGroup : MonoBehaviour
    {
        public List<SelectionButton> buttons = new List<SelectionButton>();
        public Color tabIdle;
        public Color tabHover;
        public Color tabActive;
        public List<SelectionButton> selectedButtons = new List<SelectionButton>();
        public TMP_Text tabGroupName;
        public string defaultName;
        public Enum tabEnum;
        public Func<TabButton> tabButtonFunction;


        public void Subscribe(SelectionButton button)
        {
            buttons.Add(button);
            OnTabSelected(button);
        }

        public void OnTabEnter(SelectionButton button)
        {
            ResetTabs();
            defaultName = tabGroupName.text;
            tabGroupName.text = button.selectionName;
            button.background.color = tabHover;
        }

        public void OnTabExit(SelectionButton button)
        {
            ResetTabs();
            tabGroupName.text = defaultName;

        }

        public void OnTabSelected(SelectionButton button)
        {
            if (selectedButtons.Contains(button))
            {
                selectedButtons.Remove(button);
            }
            else
            {
                selectedButtons.Add(button);
            }
            ResetTabs();
            button.background.color = tabActive;
        }

        public void ResetTabs()
        {
            foreach (SelectionButton button in buttons)
            {
                if (selectedButtons.Contains(button))
                {
                    button.background.color = tabActive;
                }
                else
                {
                    button.background.color = tabIdle;
                }
            }
        }
    }
}
