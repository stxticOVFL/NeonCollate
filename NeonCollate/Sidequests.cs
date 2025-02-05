using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NeonCollate
{
    internal class Sidequests
    {
        private static GameObject _mainMenu;
        private static GameObject _redSidequests;
        private static GameObject _violetSidequests;
        private static GameObject _yellowSidequests;

        public static void Initialize()
        {
            if (_redSidequests)
                return;

            _mainMenu = GameObject.Find("Main Menu");
            GameObject missionButton = GameObject.Find("Main Menu/Canvas/Main Menu/Panel/Missions Panel/Scroll View/Viewport/Buttons Holder/Mission Button Holder");

            if (!_mainMenu || !missionButton)
                return;

            List<GameObject> newButtons = new List<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                GameObject _newButton = GameObject.Instantiate(missionButton, missionButton.transform.parent);

                Button _button = _newButton.GetComponentInChildren<Button>();
                ColorBlock _buttonColors = _button.colors;

                _buttonColors.normalColor = new Color(1f, 1f, 1f, 1f);
                _buttonColors.highlightedColor = new Color(0.5074f, 0.9782f, 1f, 1f);
                _buttonColors.pressedColor = new Color(0.0077f, 0.5221f, 0.4999f, 1f);
                _buttonColors.selectedColor = new Color(0.5074f, 0.9782f, 1f, 1f);
                _buttonColors.disabledColor = new Color(0.7843f, 0.7843f, 0.7843f, 0.502f);
                _button.colors = _buttonColors;

                _newButton.transform.Find("Button").GetComponentInChildren<AxKLocalizedText>().Init();
                _newButton.transform.Find("Button").GetComponentInChildren<AxKLocalizedText>().enabled = false;

                _newButton.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 1f);
                _newButton.transform.Find("Button").GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, 1f);
                _newButton.transform.Find("Button").transform.Find("CounterText").GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.502f, 0.502f, 0.502f, 1f);
                _newButton.transform.Find("Button").transform.Find("Lock Image").gameObject.SetActive(false);

                newButtons.Add(_newButton);
            }

            _redSidequests = newButtons[0];
            _redSidequests.name = "RedSQ Mission Button";
            _redSidequests.transform.Find("Button").GetComponentInChildren<TextMeshProUGUI>().SetText("Red Sidequests");
            _redSidequests.transform.Find("Button").transform.Find("CounterText").GetComponentInChildren<TextMeshProUGUI>().SetText("R");

            _violetSidequests = newButtons[1];
            _violetSidequests.name = "VioletSQ Mission Button";
            _violetSidequests.transform.Find("Button").GetComponentInChildren<TextMeshProUGUI>().SetText("Violet Sidequests");
            _violetSidequests.transform.Find("Button").transform.Find("CounterText").GetComponentInChildren<TextMeshProUGUI>().SetText("V");

            _yellowSidequests = newButtons[2];
            _yellowSidequests.name = "YellowSQ Mission Button";
            _yellowSidequests.transform.Find("Button").GetComponentInChildren<TextMeshProUGUI>().SetText("Yellow Sidequests");
            _yellowSidequests.transform.Find("Button").transform.Find("CounterText").GetComponentInChildren<TextMeshProUGUI>().SetText("Y");

            _redSidequests.GetComponentInChildren<Button>().onClick.AddListener(() => Click("M_SIDEQUESTS_RED"));
            _violetSidequests.GetComponentInChildren<Button>().onClick.AddListener(() => Click("M_SIDEQUESTS_VIOLET"));
            _yellowSidequests.GetComponentInChildren<Button>().onClick.AddListener(() => Click("M_SIDEQUESTS_YELLOW"));
        }

        public static void Click(string missionID)
        {
            _mainMenu.GetComponent<MainMenu>().SelectMission(missionID, true, false);
        }

        public static void PushDown()
        {
            if (_redSidequests)
            {
                foreach (GameObject btn in new List<GameObject> { _redSidequests, _violetSidequests, _yellowSidequests })
                {
                    btn.transform.SetAsLastSibling();
                    Animator animator = btn.GetComponent<Animator>();
                    animator.SetBool("LoadIn", true);
                    animator.SetBool("LoadOut", false);
                    animator.SetBool("Selected", false);
                    animator.SetBool("MouseEnter", false);
                }
            }
        }

        [HarmonyPatch]
        public class MainMenuSelectCampaignPatch
        {
            [HarmonyPatch(typeof(MainMenu), "SelectCampaign")]
            [HarmonyPostfix]
            public static void PostfixSelectCampaign(string campaignID)
            {
                if (campaignID == "C_MAINQUEST") // lazy
                    Sidequests.PushDown();
            }

            [HarmonyPatch(typeof(MainMenu), "OnPressBackButton")]
            [HarmonyPostfix]
            public static void PostfixBackButton(string ____lastCampaignID)
            {
                if (____lastCampaignID == "C_MAINQUEST") // lazy
                    Sidequests.PushDown();
            }
        }
    }
}
