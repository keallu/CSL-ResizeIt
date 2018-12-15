using ICities;
using System;

namespace ResizeIt
{
    public class ModInfo : IUserMod
    {
        public string Name => "Resize It!";
        public string Description => "Allows to resize the scrollable panels in-game to match your style of play.";

        private static readonly string[] DefaultModeLabels =
        {
            "Expanded mode",
            "Compressed mode"
        };

        private static readonly string[] DefaultModeValues =
        {
            "Expanded mode",
            "Compressed mode"
        };

        private static readonly string[] ScrollDirectionLabels =
        {
            "Horizontally",
            "Vertically"
        };

        private static readonly string[] ScrollDirectionValues =
        {
            "Horizontally",
            "Vertically"
        };

        private static readonly string[] AlignmentLabels =
        {
            "Fixed",
            "Centered"
        };

        private static readonly string[] AlignmentValues =
        {
            "Fixed",
            "Centered"
        };

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group;
            bool selected;
            int selectedIndex;
            float selectedValue;
            float result;

            group = helper.AddGroup(Name);

            selectedIndex = GetSelectedOptionIndex(DefaultModeValues, ModConfig.Instance.DefaultMode);

            group.AddDropdown("Default mode", DefaultModeLabels, selectedIndex, sel =>
            {
                ModConfig.Instance.DefaultMode = DefaultModeValues[sel];
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.FastSwitchingEnabled;

            group.AddCheckbox("Fast switching enabled (LEFT CTRL + SPACE)", selected, sel =>
            {
                ModConfig.Instance.FastSwitchingEnabled = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Expanded mode");

            selectedValue = ModConfig.Instance.ScalingExpanded;

            group.AddSlider("Scaling", 0.5f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.ScalingExpanded = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.RowsExpanded;

            group.AddSlider("Rows", 1f, 5f, 1f, selectedValue, sel =>
            {
                ModConfig.Instance.RowsExpanded = (int)sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.ColumnsExpanded;

            group.AddSlider("Columns", 5f, 30f, 1f, selectedValue, sel =>
            {
                ModConfig.Instance.ColumnsExpanded = (int)sel;
                ModConfig.Instance.Save();
            });

            selectedIndex = GetSelectedOptionIndex(ScrollDirectionValues, ModConfig.Instance.ScrollDirectionExpanded);

            group.AddDropdown("Scroll Direction", ScrollDirectionLabels, selectedIndex, sel =>
            {
                ModConfig.Instance.ScrollDirectionExpanded = ScrollDirectionValues[sel];
                ModConfig.Instance.Save();
            });

            selectedIndex = GetSelectedOptionIndex(AlignmentValues, ModConfig.Instance.AlignmentExpanded);

            group.AddDropdown("Alignment", AlignmentLabels, selectedIndex, sel =>
            {
                ModConfig.Instance.AlignmentExpanded = AlignmentValues[sel];
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.HorizontalOffsetExpanded;

            group.AddTextfield("Horizontal offset", selectedValue.ToString(), sel =>
            {
                float.TryParse(sel, out result);
                ModConfig.Instance.HorizontalOffsetExpanded = result;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.VerticalOffsetExpanded;

            group.AddTextfield("Vertical offset", selectedValue.ToString(), sel =>
            {
                float.TryParse(sel, out result);
                ModConfig.Instance.VerticalOffsetExpanded = result;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.OpacityExpanded;

            group.AddSlider("Opacity", 0.05f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.OpacityExpanded = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Compressed mode");

            selectedValue = ModConfig.Instance.ScalingCompressed;

            group.AddSlider("Scaling", 0.5f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.ScalingCompressed = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.RowsCompressed;

            group.AddSlider("Rows", 1f, 5f, 1f, selectedValue, sel =>
            {
                ModConfig.Instance.RowsCompressed = (int)sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.ColumnsCompressed;

            group.AddSlider("Columns", 5f, 30f, 1f, selectedValue, sel =>
            {
                ModConfig.Instance.ColumnsCompressed = (int)sel;
                ModConfig.Instance.Save();
            });

            selectedIndex = GetSelectedOptionIndex(ScrollDirectionValues, ModConfig.Instance.ScrollDirectionCompressed);

            group.AddDropdown("Scroll Direction", ScrollDirectionLabels, selectedIndex, sel =>
            {
                ModConfig.Instance.ScrollDirectionCompressed = ScrollDirectionValues[sel];
                ModConfig.Instance.Save();
            });

            selectedIndex = GetSelectedOptionIndex(AlignmentValues, ModConfig.Instance.AlignmentCompressed);

            group.AddDropdown("Alignment", AlignmentLabels, selectedIndex, sel =>
            {
                ModConfig.Instance.AlignmentCompressed = AlignmentValues[sel];
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.HorizontalOffsetCompressed;

            group.AddTextfield("Horizontal offset", selectedValue.ToString(), sel =>
            {
                float.TryParse(sel, out result);
                ModConfig.Instance.HorizontalOffsetCompressed = result;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.VerticalOffsetCompressed;

            group.AddTextfield("Vertical offset", selectedValue.ToString(), sel =>
            {
                float.TryParse(sel, out result);
                ModConfig.Instance.VerticalOffsetCompressed = result;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.OpacityCompressed;

            group.AddSlider("Opacity", 0.05f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.OpacityCompressed = sel;
                ModConfig.Instance.Save();
            });
        }

        private int GetSelectedOptionIndex(string[] option, string value)
        {
            int index = Array.IndexOf(option, value);
            if (index < 0) index = 0;

            return index;
        }
    }
}