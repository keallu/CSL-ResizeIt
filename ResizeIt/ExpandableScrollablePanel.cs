using ColossalFramework.UI;
using System;
using UnityEngine;

namespace ResizeIt
{
    class ExpandableScrollablePanel : MonoBehaviour
    {
        private bool _initialized;
        private bool _expanded;

        private UITabContainer _tsContainer;
        private UITextureAtlas _textureAtlas;
        private UIPanel _controlPanel;
        private UILabel _topLabel;
        private UISprite _resizeSprite;
        private UISprite _leftSprite;
        private UISprite _rightSprite;
        private UISprite _upSprite;
        private UISprite _downSprite;
        private UILabel _bottomLabel;

        private void Awake()
        {
            try
            {
                if (_tsContainer == null)
                {
                    _tsContainer = GameObject.Find("TSContainer").GetComponent<UITabContainer>();
                }

                _textureAtlas = LoadResources();

                CreateControlPanel();
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
                    _expanded = ModConfig.Instance.DefaultMode is "Expanded mode" ? true : false;

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

                if (ModConfig.Instance.FastSwitchingEnabled)
                {
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Space))
                    {
                        ToggleMode();
                    }
                }

                if (ModConfig.Instance.ControlPanelFastSwitchingEnabled)
                {
                    if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Space))
                    {
                        ToggleControlPanel();
                    }
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
                if (_controlPanel == null)
                {
                    return;
                }

                UnityEngine.Object.Destroy(_controlPanel.gameObject);
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:OnDestroy -> Exception: " + e.Message);
            }
        }

        private UITextureAtlas LoadResources()
        {
            try
            {
                if (_textureAtlas == null)
                {
                    string[] spriteNames = new string[]
                    {
                        "buttondown",
                        "buttonleft",
                        "buttonresize",
                        "buttonright",
                        "buttonup"
                    };

                    _textureAtlas = ResourceLoader.CreateTextureAtlas("ResizeItAtlas", spriteNames, "ResizeIt.Icons.");
                }

                return _textureAtlas;
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:LoadResources -> Exception: " + e.Message);
                return null;
            }
        }

        private void CreateControlPanel()
        {
            try
            {
                _controlPanel = UIUtils.CreatePanel("ResizeItControlPanel");
                _controlPanel.width = 109f;
                _controlPanel.height = 109f;
                _controlPanel.backgroundSprite = "SubcategoriesPanel";
                _controlPanel.isVisible = false;

                _topLabel = UIUtils.CreateLabel(_controlPanel, "ResizeItControlPanelTopLabel", "0 items");
                _topLabel.textAlignment = UIHorizontalAlignment.Center;
                _topLabel.verticalAlignment = UIVerticalAlignment.Middle;
                _topLabel.textColor = new Color32(185, 221, 254, 255);
                _topLabel.textScale = 0.6f;

                _resizeSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelResizeSprite");
                _resizeSprite.atlas = _textureAtlas;
                _resizeSprite.spriteName = "buttonresize";
                _resizeSprite.autoSize = false;
                _resizeSprite.size = new Vector2(24f, 24f);
                _resizeSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _resizeSprite.width / 2f, _controlPanel.height / 2f - _resizeSprite.height / 2f);
                _resizeSprite.eventClick += (component, eventParam) =>
                {
                    ToggleMode();
                };

                _leftSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelLeftSprite");
                _leftSprite.atlas = _textureAtlas;
                _leftSprite.spriteName = "buttonleft";
                _leftSprite.autoSize = false;
                _leftSprite.size = new Vector2(15f, 15f);
                _leftSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _leftSprite.width / 2f - 25f, _controlPanel.height / 2f - _leftSprite.height / 2f);
                _leftSprite.eventClick += (component, eventParam) =>
                {
                    AddOrRemoveRowsOrColumns(0, -1);
                };

                _rightSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelRightSprite");
                _rightSprite.atlas = _textureAtlas;
                _rightSprite.spriteName = "buttonright";
                _rightSprite.autoSize = false;
                _rightSprite.size = new Vector2(15f, 15f);
                _rightSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _rightSprite.width / 2f + 25f, _controlPanel.height / 2f - _rightSprite.height / 2f);
                _rightSprite.eventClick += (component, eventParam) =>
                {
                    AddOrRemoveRowsOrColumns(0, 1);
                };

                _upSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelUpSprite");
                _upSprite.atlas = _textureAtlas;
                _upSprite.spriteName = "buttonup";
                _upSprite.autoSize = false;
                _upSprite.size = new Vector2(15f, 15f);
                _upSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _upSprite.width / 2f, _controlPanel.height / 2f - _upSprite.height / 2f - 25f);
                _upSprite.eventClick += (component, eventParam) =>
                {
                    AddOrRemoveRowsOrColumns(1, 0);
                };

                _downSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelDownSprite");
                _downSprite.atlas = _textureAtlas;
                _downSprite.spriteName = "buttondown";
                _downSprite.autoSize = false;
                _downSprite.size = new Vector2(15f, 15f);
                _downSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _downSprite.width / 2f, _controlPanel.height / 2f - _downSprite.height / 2f + 25f);
                _downSprite.eventClick += (component, eventParam) =>
                {
                    AddOrRemoveRowsOrColumns(-1, 0);
                };

                _bottomLabel = UIUtils.CreateLabel(_controlPanel, "ResizeItControlPanelBottomLabel", "1 x 7 @ 100%");
                _bottomLabel.textAlignment = UIHorizontalAlignment.Center;
                _bottomLabel.verticalAlignment = UIVerticalAlignment.Middle;
                _bottomLabel.textColor = new Color32(185, 221, 254, 255);
                _bottomLabel.textScale = 0.6f;

                _tsContainer.eventSelectedIndexChanged += (component, value) =>
                {
                    if (ModConfig.Instance.ControlPanelEnabled && value > -1)
                    {
                        UpdateControlPanel();

                        _controlPanel.isVisible = true;
                    }
                    else
                    {
                        _controlPanel.isVisible = false;
                    }
                };
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:CreateControlPanel -> Exception: " + e.Message);
            }
        }

        private void AddOrRemoveRowsOrColumns(int rowsAlteration, int columnsAlteration)
        {
            try
            {
                if (_expanded)
                {
                    ModConfig.Instance.RowsExpanded = Mathf.Clamp(ModConfig.Instance.RowsExpanded + rowsAlteration, 1, 5);
                    ModConfig.Instance.ColumnsExpanded = Mathf.Clamp(ModConfig.Instance.ColumnsExpanded + columnsAlteration, 5, 30);
                    Expand();
                }
                else
                {
                    ModConfig.Instance.RowsCompressed = Mathf.Clamp(ModConfig.Instance.RowsCompressed + rowsAlteration, 1, 5);
                    ModConfig.Instance.ColumnsCompressed = Mathf.Clamp(ModConfig.Instance.ColumnsCompressed + columnsAlteration, 5, 30);
                    Compress();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:AddOrRemoveRowsOrColumns -> Exception: " + e.Message);
            }
        }

        private void ToggleControlPanel()
        {
            try
            {
                if (ModConfig.Instance.ControlPanelEnabled)
                {
                    _controlPanel.isVisible = false;
                    ModConfig.Instance.ControlPanelEnabled = false;
                }
                else
                {
                    _controlPanel.isVisible = true;
                    ModConfig.Instance.ControlPanelEnabled = true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:ToggleControlPanel -> Exception: " + e.Message);
            }
        }

        private void UpdateControlPanel()
        {
            try
            {
                int rows = _expanded ? ModConfig.Instance.RowsExpanded : ModConfig.Instance.RowsCompressed;
                int columns = _expanded ? ModConfig.Instance.ColumnsExpanded : ModConfig.Instance.ColumnsCompressed;
                float scaling = _expanded ? ModConfig.Instance.ScalingExpanded : ModConfig.Instance.ScalingCompressed;
                _bottomLabel.text = string.Format("{0} x {1} @ {2}%", rows.ToString(), columns.ToString(), (scaling * 100).ToString());
                _bottomLabel.relativePosition = new Vector3(_controlPanel.width / 2f - _bottomLabel.width / 2f, _controlPanel.height - _bottomLabel.height - 5f);

                if (ModConfig.Instance.ControlPanelAlignment is "Left")
                {
                    _controlPanel.absolutePosition = new Vector3(_tsContainer.absolutePosition.x - _controlPanel.width - 3f, _tsContainer.absolutePosition.y);
                }
                else
                {
                    _controlPanel.absolutePosition = new Vector3(_tsContainer.absolutePosition.x + _tsContainer.width + 3f, _tsContainer.absolutePosition.y);
                }

                _controlPanel.opacity = ModConfig.Instance.ControlPanelOpacity;
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:UpdateControlPanel -> Exception: " + e.Message);
            }
        }

        private void UpdateNumberOfItems(int numberOfItems)
        {
            try
            {
                _topLabel.text = numberOfItems + " items";
                _topLabel.relativePosition = new Vector3(_controlPanel.width / 2f - _topLabel.width / 2f, 10f);
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:UpdateNumberOfItems -> Exception: " + e.Message);
            }
        }

        private void ToggleMode()
        {
            try
            {
                if (_expanded)
                {
                    Compress();
                }
                else
                {
                    Expand();
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
                float scaling = ModConfig.Instance.ScalingExpanded;
                int rows = ModConfig.Instance.RowsExpanded;
                int columns = ModConfig.Instance.ColumnsExpanded;
                bool scrollVertically = ModConfig.Instance.ScrollDirectionExpanded is "Vertically" ? true : false;
                bool alignmentCentered = ModConfig.Instance.AlignmentExpanded is "Centered" ? true : false;
                float horizontalOffset = ModConfig.Instance.HorizontalOffsetExpanded;
                float verticalOffset = ModConfig.Instance.VerticalOffsetExpanded;
                float opacity = ModConfig.Instance.OpacityExpanded;

                UpdateGUI(scaling, rows, columns, scrollVertically, alignmentCentered, horizontalOffset, verticalOffset, opacity);

                _expanded = true;

                UpdateControlPanel();
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
                float scaling = ModConfig.Instance.ScalingCompressed;
                int rows = ModConfig.Instance.RowsCompressed;
                int columns = ModConfig.Instance.ColumnsCompressed;
                bool scrollVertically = ModConfig.Instance.ScrollDirectionCompressed is "Vertically" ? true : false;
                bool alignmentCentered = ModConfig.Instance.AlignmentCompressed is "Centered" ? true : false;
                float horizontalOffset = ModConfig.Instance.HorizontalOffsetCompressed;
                float verticalOffset = ModConfig.Instance.VerticalOffsetCompressed;
                float opacity = ModConfig.Instance.OpacityCompressed;

                UpdateGUI(scaling, rows, columns, scrollVertically, alignmentCentered, horizontalOffset, verticalOffset, opacity);

                _expanded = false;

                UpdateControlPanel();
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ExpandableScrollablePanel:Compress -> Exception: " + e.Message);
            }
        }

        private void UpdateGUI(float scaling, int rows, int columns, bool scrollVertically, bool alignmentCentered, float horizontalOffset, float verticalOffset, float opacity)
        {
            try
            {
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

                                    scrollablePanel.eventVisibilityChanged += (component, value) =>
                                    {
                                        if (value)
                                        {
                                            UpdateNumberOfItems(component.childCount);
                                        }
                                    };

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

                    foreach (UIComponent comp in panel.components)
                    {
                        if (comp is UIScrollablePanel)
                        {
                            scrollablePanel = (UIScrollablePanel)comp;

                            scrollablePanel.wrapLayout = true;
                            scrollablePanel.autoLayout = true;
                            scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;

                            scrollablePanel.height = _tsContainer.height;
                            scrollablePanel.width = Mathf.Round(109f * scaling * columns) + 1;

                            scrollablePanel.eventVisibilityChanged += (component, value) =>
                            {
                                if (value)
                                {
                                    UpdateNumberOfItems(component.childCount);
                                }
                            };

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

                        if (comp is UIButton)
                        {
                            if (comp.position.x == 16f)
                            {
                                comp.relativePosition = new Vector3(16f, _tsContainer.height / 2f - 16f);
                            }
                            else
                            {
                                comp.relativePosition = new Vector3(comp.parent.width - 47f, _tsContainer.height / 2f - 16f);
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
