using ColossalFramework.UI;
using System;
using UnityEngine;

namespace ResizeIt
{
    class ExpandableScrollablePanel : MonoBehaviour
    {
        private bool _initialized;
        private bool _expanded;
        private bool _topped;

        private UITabContainer _tsContainer;
        
        private void Awake()
        {
            try
            {
                _expanded = true;

                if (_tsContainer == null)
                {
                    _tsContainer = GameObject.Find("TSContainer").GetComponent<UITabContainer>();
                }

            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:Awake -> Exception: " + e.Message);
            }
        }

        private void OnEnable()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:OnEnable -> Exception: " + e.Message);
            }
        }

        private void Start()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:Start -> Exception: " + e.Message);
            }
        }

        private void Update()
        {
            try
            {
                if (_tsContainer == null)
                {
                    return;
                }

                if (!_initialized || ModConfig.Instance.ConfigUpdated)
                {
                    if (_expanded)
                    {
                        Expand();
                    }
                    else
                    {
                        Compress();
                    }

                    _initialized = true;
                    ModConfig.Instance.ConfigUpdated = false;
                }

                if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Space))
                {
                    if (_topped)
                    {

                        _topped = false;
                    }
                    else
                    {

                        _topped = true;
                    }
                }

                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space))
                {
                    ToggleMode();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:Update -> Exception: " + e.Message);
            }
        }

        private void OnDisable()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:OnDisable -> Exception: " + e.Message);
            }
        }

        private void OnDestroy()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:OnDestroy -> Exception: " + e.Message);
            }
        }

        private void CreateExtenderPanel()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:CreateExtenderPanel -> Exception: " + e.Message);
            }
        }

        private void ToggleMode()
        {
            try
            {
                if (_expanded)
                {
                    Compress();
                    _expanded = false;
                }
                else
                {
                    Expand();
                    _expanded = true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:Toggle -> Exception: " + e.Message);
            }
        }

        private void Expand()
        {
            try
            {
                UpdateGUI(ModConfig.Instance.RowsExpanded > 0 ? ModConfig.Instance.RowsExpanded : 1, ModConfig.Instance.ColumnsExpanded > 0 ? ModConfig.Instance.ColumnsExpanded : 7);
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:Expand -> Exception: " + e.Message);
            }
        }

        private void Compress()
        {
            try
            {
                UpdateGUI(ModConfig.Instance.RowsCompressed > 0 ? ModConfig.Instance.RowsCompressed : 1, ModConfig.Instance.ColumnsCompressed > 0 ? ModConfig.Instance.ColumnsCompressed : 7);
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:Compress -> Exception: " + e.Message);
            }
        }

        private void UpdateGUI(int rows, int columns)
        {
            try
            {
                float scaling = ModConfig.Instance.Scaling > 0f ? ModConfig.Instance.Scaling : 1f;
                bool scrollVertically = ModConfig.Instance.ScrollDirection is "Vertically" ? true : false;
                bool alignmentCentered = ModConfig.Instance.Alignment is "Centered" ? true : false;
                float horizontalOffset = ModConfig.Instance.HorizontalOffset != 0f ? ModConfig.Instance.HorizontalOffset : 0f;
                float verticalOffset = ModConfig.Instance.VerticalOffset != 0f ? ModConfig.Instance.VerticalOffset : 0f;
                float opacity = ModConfig.Instance.Opacity > 0f ? ModConfig.Instance.Opacity : 1f;

                UITabContainer gtsContainer;
                UIScrollablePanel scrollablePanel;
                UIButton button;
                UIScrollbar scrollbar;

                _tsContainer.opacity = opacity;
                _tsContainer.height = Mathf.Round(109f * scaling * rows) + 1;
                _tsContainer.width = Mathf.Round(859f - 763f + 109f * scaling * columns) + 1;
                _tsContainer.relativePosition = new Vector3(alignmentCentered ? _tsContainer.parent.width / 2f - (_tsContainer.width / 2f) + horizontalOffset : 595.5f + horizontalOffset, 0 - (110f * scaling) - (109f * scaling * (rows - 1)) + verticalOffset);

                foreach (UIComponent toolPanel in _tsContainer.components)
                {
                    if (toolPanel is UIPanel)
                    {
                        gtsContainer = toolPanel.GetComponentInChildren<UITabContainer>();

                        if (gtsContainer != null)
                        {
                            foreach (UIComponent tabPanel in gtsContainer.components)
                            {
                                tabPanel.height = _tsContainer.height;
                                tabPanel.width = _tsContainer.width;

                                scrollablePanel = tabPanel.GetComponentInChildren<UIScrollablePanel>();

                                if (scrollablePanel != null)
                                {
                                    scrollablePanel.wrapLayout = true;
                                    scrollablePanel.autoLayout = true;
                                    scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;

                                    scrollablePanel.height = _tsContainer.height;
                                    scrollablePanel.width = Mathf.Round(109f * scaling * columns) + 1;

                                    foreach (UIComponent scrollableButton in scrollablePanel.components)
                                    {
                                        button = scrollableButton.GetComponentInChildren<UIButton>();

                                        if (button != null)
                                        {
                                            button.height = 100f * scaling;
                                            button.width = 109f * scaling;
                                            button.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
                                        }
                                    }

                                    scrollbar = scrollablePanel.verticalScrollbar ?? scrollablePanel.horizontalScrollbar;

                                    if (scrollbar != null)
                                    {
                                        if (scrollVertically)
                                        {
                                            scrollablePanel.autoLayoutDirection = LayoutDirection.Horizontal;
                                            scrollablePanel.scrollWheelDirection = UIOrientation.Vertical;

                                            scrollablePanel.horizontalScrollbar = null;
                                            scrollablePanel.verticalScrollbar = scrollbar;
                                        }
                                        else
                                        {
                                            scrollablePanel.autoLayoutDirection = LayoutDirection.Vertical;
                                            scrollablePanel.scrollWheelDirection = UIOrientation.Horizontal;

                                            scrollablePanel.horizontalScrollbar = scrollbar;
                                            scrollablePanel.verticalScrollbar = null;
                                        }

                                        scrollbar.height = _tsContainer.height;
                                        scrollbar.width = _tsContainer.width;

                                        if (scrollbar.decrementButton != null)
                                        {
                                            scrollbar.decrementButton.relativePosition = new Vector3(scrollbar.decrementButton.relativePosition.x, scrollbar.height / 2f - 16f);
                                            scrollbar.decrementButton.isInteractive = false;
                                        }

                                        if (scrollbar.incrementButton != null)
                                        {
                                            scrollbar.incrementButton.relativePosition = new Vector3(scrollbar.incrementButton.relativePosition.x, scrollbar.height / 2f - 16f);
                                            scrollbar.incrementButton.isInteractive = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ModUtils.IsModEnabled("ploppablerico"))
                {
                    PatchPloppableRICOMod(rows, columns, scaling, scrollVertically);
                }

                if (ModUtils.IsModEnabled("findit"))
                {
                    PatchFindItMod(rows, columns, scaling);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:UpdateGUI -> Exception: " + e.Message);
            }
        }

        private void PatchPloppableRICOMod(int rows, int columns, float scaling, bool scrollVertically)
        {
            try
            {
                UIComponent panel = GameObject.Find("PloppableBuildingPanel").GetComponent<UIComponent>();

                if (panel != null)
                {
                    UIScrollablePanel scrollablePanel;
                    UIButton button;

                    foreach (UIComponent component in panel.components)
                    {
                        if (component is UIScrollablePanel)
                        {
                            scrollablePanel = (UIScrollablePanel)component;

                            scrollablePanel.wrapLayout = true;
                            scrollablePanel.autoLayout = true;
                            scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;

                            scrollablePanel.height = _tsContainer.height;
                            scrollablePanel.width = Mathf.Round(109f * scaling * columns) + 1;

                            foreach (UIComponent scrollableButton in scrollablePanel.components)
                            {
                                button = scrollableButton.GetComponentInChildren<UIButton>();

                                if (button != null)
                                {
                                    button.height = 100f * scaling;
                                    button.width = 109f * scaling;
                                    button.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
                                }
                            }

                            if (scrollVertically)
                            {
                                scrollablePanel.autoLayoutDirection = LayoutDirection.Horizontal;
                                scrollablePanel.scrollWheelDirection = UIOrientation.Vertical;
                            }
                            else
                            {
                                scrollablePanel.autoLayoutDirection = LayoutDirection.Vertical;
                                scrollablePanel.scrollWheelDirection = UIOrientation.Horizontal;
                            }
                        }

                        if (component is UIButton)
                        {
                            if (component.position.x == 16f)
                            {
                                component.relativePosition = new Vector3(16f, _tsContainer.height / 2f - 16f);
                            }
                            else
                            {
                                component.relativePosition = new Vector3(component.parent.width - 47f, _tsContainer.height / 2f - 16f);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:PatchPloppableRICOMod -> Exception: " + e.Message);
            }
        }

        private void PatchFindItMod(int rows, int columns, float scaling)
        {
            try
            {
                UIComponent panel = GameObject.Find("FindItDefaultPanel").GetComponent<UIComponent>();

                if (panel != null)
                {
                    panel.height = _tsContainer.height;
                    panel.width = Mathf.Round(109f * scaling * columns) + 1;

                    UIComponent scrollPanel = panel.Find("ScrollablePanel").GetComponent<UIComponent>();

                    if (scrollPanel != null)
                    {
                        UIScrollablePanel scrollablePanel = (UIScrollablePanel)scrollPanel;

                        scrollablePanel.wrapLayout = true;
                        scrollablePanel.autoLayout = true;
                        scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;

                        UIComponent scrollbar = panel.Find("UIScrollbar").GetComponent<UIComponent>();

                        if (scrollbar != null)
                        {

                        }

                        UIComponent slicedSprite = panel.Find("UISlicedSprite").GetComponent<UIComponent>();

                        if (slicedSprite != null)
                        {

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:PatchFindItMod -> Exception: " + e.Message);
            }
        }
    }
}
