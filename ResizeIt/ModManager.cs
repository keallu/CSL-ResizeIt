using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResizeIt
{
    public class ModManager : MonoBehaviour
    {
        private bool _initialized;
        private bool _expanded;

        private List<string> _controlPanelValidToolbarButtonNames;
        private List<int> _controlPanelValidToolbarButtonIndex;

        private UITabContainer _tsContainer;
        private UITextureAtlas _textureAtlas;

        private bool isPloppableRICOEnabled;
        private bool isFindItEnabled;

        private UIPanel _controlPanel;
        private UILabel _topLabel;
        private UISprite _hoverSprite;
        private UISprite _resizeSprite;
        private UISprite _leftSprite;
        private UISprite _rightSprite;
        private UISprite _upSprite;
        private UISprite _downSprite;
        private UILabel _bottomLabel;

        public void Awake()
        {
            try
            {
                if (_tsContainer == null)
                {
                    _tsContainer = GameObject.Find("TSContainer").GetComponent<UITabContainer>();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:Awake -> Exception: " + e.Message);
            }
        }

        public void Start()
        {
            try
            {
                _textureAtlas = LoadResources();

                _controlPanelValidToolbarButtonNames = new List<string>() { "RoadsPanel", "ZoningPanel", "DistrictPanel", "ElectricityPanel", "WaterAndSewagePanel", "GarbagePanel", "HealthcarePanel", "FireDepartmentPanel", "PolicePanel", "EducationPanel", "PublicTransportPanel", "BeautificationPanel", "MonumentsPanel", "WondersPanel", "LandscapingPanel", "ResourcePanel", "WaterPanel", "SurfacePanel", "PloppableBuildingPanel", "FindItGroupPanel" };

                ModConfig.Instance.RowsExpanded = ModConfig.Instance.RowsExpanded > 0 ? ModConfig.Instance.RowsExpanded : 3;
                ModConfig.Instance.ColumnsExpanded = ModConfig.Instance.ColumnsExpanded > 0 ? ModConfig.Instance.ColumnsExpanded : 7;
                ModConfig.Instance.OpacityExpanded = ModConfig.Instance.OpacityExpanded > 0f ? ModConfig.Instance.OpacityExpanded : 1f;

                ModConfig.Instance.RowsCompressed = ModConfig.Instance.RowsCompressed > 0 ? ModConfig.Instance.RowsCompressed : 1;
                ModConfig.Instance.ColumnsCompressed = ModConfig.Instance.ColumnsCompressed > 0 ? ModConfig.Instance.ColumnsCompressed : 7;
                ModConfig.Instance.OpacityCompressed = ModConfig.Instance.OpacityCompressed > 0f ? ModConfig.Instance.OpacityCompressed : 1f;

                if (ModUtils.IsModEnabled("ploppablerico"))
                {
                    isPloppableRICOEnabled = true;
                }

                if (ModUtils.IsModEnabled("findit"))
                {
                    isFindItEnabled = true;
                }

                CreateControlPanel();
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:Start -> Exception: " + e.Message);
            }
        }

        public void Update()
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
                Debug.Log("[Resize It!] ModManager:Update -> Exception: " + e.Message);
            }
        }

        public void OnDestroy()
        {
            try
            {
                if (_bottomLabel != null)
                {
                    Destroy(_bottomLabel);
                }
                if (_downSprite != null)
                {
                    Destroy(_downSprite);
                }
                if (_upSprite != null)
                {
                    Destroy(_upSprite);
                }
                if (_rightSprite != null)
                {
                    Destroy(_rightSprite);
                }
                if (_leftSprite != null)
                {
                    Destroy(_leftSprite);
                }
                if (_resizeSprite != null)
                {
                    Destroy(_resizeSprite);
                }
                if (_hoverSprite != null)
                {
                    Destroy(_hoverSprite);
                }
                if (_topLabel != null)
                {
                    Destroy(_topLabel);
                }
                if (_controlPanel != null)
                {
                    Destroy(_controlPanel);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:OnDestroy -> Exception: " + e.Message);
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
                        "buttonresizecompressed",
                        "buttonresizeexpanded",
                        "buttonright",
                        "buttonup",
                        "hover"
                    };

                    _textureAtlas = ResourceLoader.CreateTextureAtlas("ResizeItAtlas", spriteNames, "ResizeIt.Icons.");
                }

                return _textureAtlas;
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:LoadResources -> Exception: " + e.Message);
                return null;
            }
        }

        private void CreateControlPanel()
        {
            try
            {
                _controlPanel = UIUtils.CreatePanel("ResizeItControlPanel");
                _controlPanel.zOrder = 25;
                _controlPanel.width = 109f;
                _controlPanel.height = 109f;
                _controlPanel.backgroundSprite = "SubcategoriesPanel";
                _controlPanel.isVisible = false;

                _topLabel = UIUtils.CreateLabel(_controlPanel, "ResizeItControlPanelTopLabel", "0 items");
                _topLabel.textAlignment = UIHorizontalAlignment.Center;
                _topLabel.verticalAlignment = UIVerticalAlignment.Middle;
                _topLabel.textColor = new Color32(185, 221, 254, 255);
                _topLabel.textScale = 0.6f;

                _hoverSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelHoverSprite");
                _hoverSprite.atlas = _textureAtlas;
                _hoverSprite.spriteName = "hover";
                _hoverSprite.autoSize = false;
                _hoverSprite.isVisible = false;

                _resizeSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelResizeSprite");
                _resizeSprite.atlas = _textureAtlas;
                _resizeSprite.spriteName = "buttonresizecompressed";
                _resizeSprite.autoSize = false;
                _resizeSprite.size = new Vector2(24f, 24f);
                _resizeSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _resizeSprite.width / 2f, _controlPanel.height / 2f - _resizeSprite.height / 2f);
                _resizeSprite.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        ToggleMode();

                        eventParam.Use();
                    }
                };
                _resizeSprite.eventMouseEnter += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonEnter(component, eventParam);

                        eventParam.Use();
                    }
                };
                _resizeSprite.eventMouseLeave += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonLeave(component, eventParam);

                        eventParam.Use();
                    }
                };

                _leftSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelLeftSprite");
                _leftSprite.atlas = _textureAtlas;
                _leftSprite.spriteName = "buttonleft";
                _leftSprite.autoSize = false;
                _leftSprite.size = new Vector2(15f, 15f);
                _leftSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _leftSprite.width / 2f - 25f, _controlPanel.height / 2f - _leftSprite.height / 2f);
                _leftSprite.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        AddOrRemoveRowsOrColumns(0, -1);
                        ModConfig.Instance.Save();

                        eventParam.Use();
                    }
                };
                _leftSprite.eventMouseEnter += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonEnter(component, eventParam);

                        eventParam.Use();
                    }
                };
                _leftSprite.eventMouseLeave += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonLeave(component, eventParam);

                        eventParam.Use();
                    }
                };

                _rightSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelRightSprite");
                _rightSprite.atlas = _textureAtlas;
                _rightSprite.spriteName = "buttonright";
                _rightSprite.autoSize = false;
                _rightSprite.size = new Vector2(15f, 15f);
                _rightSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _rightSprite.width / 2f + 25f, _controlPanel.height / 2f - _rightSprite.height / 2f);
                _rightSprite.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        AddOrRemoveRowsOrColumns(0, 1);
                        ModConfig.Instance.Save();

                        eventParam.Use();
                    }
                };
                _rightSprite.eventMouseEnter += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonEnter(component, eventParam);

                        eventParam.Use();
                    }
                };
                _rightSprite.eventMouseLeave += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonLeave(component, eventParam);

                        eventParam.Use();
                    }
                };

                _upSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelUpSprite");
                _upSprite.atlas = _textureAtlas;
                _upSprite.spriteName = "buttonup";
                _upSprite.autoSize = false;
                _upSprite.size = new Vector2(15f, 15f);
                _upSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _upSprite.width / 2f, _controlPanel.height / 2f - _upSprite.height / 2f - 25f);
                _upSprite.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        AddOrRemoveRowsOrColumns(1, 0);
                        ModConfig.Instance.Save();

                        eventParam.Use();
                    }
                };
                _upSprite.eventMouseEnter += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonEnter(component, eventParam);

                        eventParam.Use();
                    }
                };
                _upSprite.eventMouseLeave += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonLeave(component, eventParam);

                        eventParam.Use();
                    }
                };

                _downSprite = UIUtils.CreateSprite(_controlPanel, "ResizeItControlPanelDownSprite");
                _downSprite.atlas = _textureAtlas;
                _downSprite.spriteName = "buttondown";
                _downSprite.autoSize = false;
                _downSprite.size = new Vector2(15f, 15f);
                _downSprite.relativePosition = new Vector3(_controlPanel.width / 2f - _downSprite.width / 2f, _controlPanel.height / 2f - _downSprite.height / 2f + 25f);
                _downSprite.eventClick += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        AddOrRemoveRowsOrColumns(-1, 0);
                        ModConfig.Instance.Save();

                        eventParam.Use();
                    }
                };
                _downSprite.eventMouseEnter += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonEnter(component, eventParam);

                        eventParam.Use();
                    }
                };
                _downSprite.eventMouseLeave += (component, eventParam) =>
                {
                    if (!eventParam.used)
                    {
                        OnButtonLeave(component, eventParam);

                        eventParam.Use();
                    }
                };

                _bottomLabel = UIUtils.CreateLabel(_controlPanel, "ResizeItControlPanelBottomLabel", "0 x 0 @ 0%");
                _bottomLabel.textAlignment = UIHorizontalAlignment.Center;
                _bottomLabel.verticalAlignment = UIVerticalAlignment.Middle;
                _bottomLabel.textColor = new Color32(185, 221, 254, 255);
                _bottomLabel.textScale = 0.6f;

                _tsContainer.eventSelectedIndexChanged += (component, value) =>
                {
                    if (_controlPanelValidToolbarButtonIndex == null)
                    {
                        _controlPanelValidToolbarButtonIndex = new List<int>();

                        int index = 0;

                        foreach (UIComponent comp in _tsContainer.components)
                        {
                            if (_controlPanelValidToolbarButtonNames.Contains(comp.name))
                            {
                                _controlPanelValidToolbarButtonIndex.Add(index);
                            }

                            index++;
                        }
                    }

                    if (ModConfig.Instance.ControlPanelEnabled && _controlPanelValidToolbarButtonIndex.Contains(value))
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
                Debug.Log("[Resize It!] ModManager:CreateControlPanel -> Exception: " + e.Message);
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
                Debug.Log("[Resize It!] ModManager:AddOrRemoveRowsOrColumns -> Exception: " + e.Message);
            }
        }

        private void OnButtonEnter(UIComponent component, UIMouseEventParameter eventParam)
        {
            float diameter = component.name is "ResizeItControlPanelResizeSprite" ? 36f : 26.25f;

            _hoverSprite.size = new Vector3(diameter, diameter);
            _hoverSprite.AlignTo(component, UIAlignAnchor.TopLeft);
            _hoverSprite.relativePosition = new Vector3(0 - ((diameter - component.width) / 2f), 0 - ((diameter - component.height) / 2f));
            _hoverSprite.isVisible = true;
        }

        private void OnButtonLeave(UIComponent component, UIMouseEventParameter eventParam)
        {
            _hoverSprite.isVisible = false;
        }

        private void ToggleControlPanel()
        {
            try
            {
                if (ModConfig.Instance.ControlPanelEnabled)
                {
                    ModConfig.Instance.ControlPanelEnabled = false;
                    _controlPanel.isVisible = false;
                }
                else
                {
                    ModConfig.Instance.ControlPanelEnabled = true;

                    if (_controlPanelValidToolbarButtonIndex.Contains(_tsContainer.selectedIndex))
                    {
                        _controlPanel.isVisible = true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:ToggleControlPanel -> Exception: " + e.Message);
            }
        }

        private void UpdateControlPanel()
        {
            try
            {
                int rows = _expanded ? ModConfig.Instance.RowsExpanded : ModConfig.Instance.RowsCompressed;
                int columns = _expanded ? ModConfig.Instance.ColumnsExpanded : ModConfig.Instance.ColumnsCompressed;
                string spriteName = _expanded ? "buttonresizeexpanded" : "buttonresizecompressed";

                _resizeSprite.spriteName = spriteName;
                _bottomLabel.text = string.Format("{0} x {1}", rows.ToString(), columns.ToString());
                _bottomLabel.relativePosition = new Vector3(_controlPanel.width / 2f - _bottomLabel.width / 2f, _controlPanel.height - _bottomLabel.height - 5f);

                _controlPanel.absolutePosition = new Vector3(_tsContainer.absolutePosition.x + _tsContainer.width + 3f, _tsContainer.absolutePosition.y);
                _controlPanel.opacity = ModConfig.Instance.ControlPanelOpacity;
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:UpdateControlPanel -> Exception: " + e.Message);
            }
        }

        private void UpdateNumberOfItemsText(string text)
        {
            try
            {
                _topLabel.text = text;
                _topLabel.relativePosition = new Vector3(_controlPanel.width / 2f - _topLabel.width / 2f, 10f);
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:UpdateNumberOfItemsText -> Exception: " + e.Message);
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
                Debug.Log("[Resize It!] ModManager:Toggle -> Exception: " + e.Message);
            }
        }

        private void Expand()
        {
            try
            {
                int rows = ModConfig.Instance.RowsExpanded;
                int columns = ModConfig.Instance.ColumnsExpanded;
                bool scrollVertically = ModConfig.Instance.ScrollDirectionExpanded is "Vertically" ? true : false;
                bool alignmentCentered = ModConfig.Instance.AlignmentExpanded is "Centered" ? true : false;
                float horizontalOffset = ModConfig.Instance.HorizontalOffsetExpanded;
                float verticalOffset = ModConfig.Instance.VerticalOffsetExpanded;
                float opacity = ModConfig.Instance.OpacityExpanded;

                UpdateGUI(rows, columns, scrollVertically, alignmentCentered, horizontalOffset, verticalOffset, opacity);

                _expanded = true;

                UpdateControlPanel();
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:Expand -> Exception: " + e.Message);
            }
        }

        private void Compress()
        {
            try
            {
                int rows = ModConfig.Instance.RowsCompressed;
                int columns = ModConfig.Instance.ColumnsCompressed;
                bool scrollVertically = ModConfig.Instance.ScrollDirectionCompressed is "Vertically" ? true : false;
                bool alignmentCentered = ModConfig.Instance.AlignmentCompressed is "Centered" ? true : false;
                float horizontalOffset = ModConfig.Instance.HorizontalOffsetCompressed;
                float verticalOffset = ModConfig.Instance.VerticalOffsetCompressed;
                float opacity = ModConfig.Instance.OpacityCompressed;

                UpdateGUI(rows, columns, scrollVertically, alignmentCentered, horizontalOffset, verticalOffset, opacity);

                _expanded = false;

                UpdateControlPanel();
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:Compress -> Exception: " + e.Message);
            }
        }

        private void UpdateGUI(int rows, int columns, bool scrollVertically, bool alignmentCentered, float horizontalOffset, float verticalOffset, float opacity)
        {
            try
            {
                UITabContainer gtsContainer;
                UIScrollablePanel scrollablePanel;
                UIButton button;
                UIScrollbar scrollbar;

                _tsContainer.opacity = opacity;
                _tsContainer.height = Mathf.Round(109f * rows) + 1;
                _tsContainer.width = Mathf.Round(859f - 763f + 109f * columns) + 1;
                _tsContainer.relativePosition = new Vector3(alignmentCentered ? _tsContainer.parent.width / 2f - (_tsContainer.width / 2f) + horizontalOffset : 595.5f + horizontalOffset, 0 - 110f - (109f * (rows - 1)) + verticalOffset);

                foreach (UIComponent toolPanel in _tsContainer.components)
                {
                    if (ModConfig.Instance.SafeModeEnabled && !_controlPanelValidToolbarButtonIndex.Contains(_tsContainer.components.IndexOf(toolPanel)))
                    {
                        continue;
                    }

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
                                    scrollablePanel.width = Mathf.Round(109f * columns) + 1;

                                    scrollablePanel.eventVisibilityChanged += (component, value) =>
                                    {
                                        if (value)
                                        {
                                            UpdateNumberOfItemsText(component.childCount + " items");
                                        }
                                    };

                                    foreach (UIComponent scrollableButton in scrollablePanel.components)
                                    {
                                        button = scrollableButton.GetComponentInChildren<UIButton>();

                                        if (button != null)
                                        {
                                            button.height = 100f;
                                            button.width = 109f;
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
                                        }

                                        if (scrollbar.incrementButton != null)
                                        {
                                            scrollbar.incrementButton.relativePosition = new Vector3(scrollbar.incrementButton.relativePosition.x, scrollbar.height / 2f - 16f);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (isPloppableRICOEnabled)
                {
                    PatchPloppableRICOMod(rows, columns);
                }

                if (isFindItEnabled)
                {
                    PatchFindItMod();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:UpdateGUI -> Exception: " + e.Message);
            }
        }

        private void PatchPloppableRICOMod(int rows, int columns)
        {
            try
            {
                GameObject ploppableBuildingGameObject = GameObject.Find("PloppableBuildingPanel");

                if (ploppableBuildingGameObject != null)
                {
                    UIComponent ploppableBuildingPanel = ploppableBuildingGameObject.GetComponent<UIComponent>();

                    if (ploppableBuildingPanel != null)
                    {
                        UIScrollablePanel scrollablePanel;
                        UIButton button;

                        foreach (UIComponent comp in ploppableBuildingPanel.components)
                        {
                            if (comp is UIScrollablePanel)
                            {
                                UIPanel panel = (UIPanel)ploppableBuildingPanel;
                                scrollablePanel = (UIScrollablePanel)comp;

                                scrollablePanel.wrapLayout = true;
                                scrollablePanel.autoLayout = true;
                                scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;

                                panel.height = _tsContainer.height;
                                panel.width = Mathf.Round(859f - 763f + 109f * columns) + 1;

                                scrollablePanel.height = _tsContainer.height;
                                scrollablePanel.width = Mathf.Round(109f * columns) + 1;

                                scrollablePanel.eventVisibilityChanged += (component, value) =>
                                {
                                    if (value)
                                    {
                                        UpdateNumberOfItemsText(component.childCount + " items");
                                    }
                                };

                                foreach (UIComponent scrollableButton in scrollablePanel.components)
                                {
                                    button = scrollableButton.GetComponentInChildren<UIButton>();

                                    if (button != null)
                                    {
                                        button.height = 100f;
                                        button.width = 109f;
                                        button.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
                                    }
                                }

                                if (rows > 1)
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
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:PatchPloppableRICOMod -> Exception: " + e.Message);
            }
        }

        private void PatchFindItMod()
        {
            try
            {
                GameObject findItGameObject = GameObject.Find("FindItDefaultPanel");

                if (findItGameObject != null)
                {
                    UIComponent findItDefaultPanel = findItGameObject.GetComponent<UIComponent>();

                    if (findItDefaultPanel != null)
                    {
                        UIComponent findItScrollablePanel = findItDefaultPanel.Find("ScrollablePanel").GetComponent<UIComponent>();

                        if (findItScrollablePanel != null)
                        {
                            UIScrollablePanel scrollablePanel = (UIScrollablePanel)findItScrollablePanel;

                            scrollablePanel.wrapLayout = true;
                            scrollablePanel.autoLayout = true;
                            scrollablePanel.autoLayoutStart = LayoutStart.TopLeft;

                            scrollablePanel.eventVisibilityChanged += (component, value) =>
                            {
                                if (value)
                                {
                                    UpdateNumberOfItemsText("Find It!");
                                }
                            };
                        }

                        UIComponent findItScrollbar = findItDefaultPanel.Find("UIScrollbar").GetComponent<UIComponent>();

                        if (findItScrollbar != null)
                        {
                            UIScrollbar scrollbar = (UIScrollbar)findItScrollbar;

                            UIComponent findItSlicedSpriteTrack = findItScrollbar.Find("UISlicedSprite").GetComponent<UIComponent>();

                            if (findItSlicedSpriteTrack != null)
                            {
                                UISlicedSprite slicedSpriteTrack = (UISlicedSprite)findItSlicedSpriteTrack;

                                slicedSpriteTrack.height = _tsContainer.height;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] ModManager:PatchFindItMod -> Exception: " + e.Message);
            }
        }
    }
}