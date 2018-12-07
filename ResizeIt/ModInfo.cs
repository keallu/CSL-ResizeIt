using ICities;
using System;

namespace ResizeIt
{
    public class ModInfo : IUserMod
    {
        public string Name => "Resize It!";
        public string Description => "Allows to resize the scrollable panels in-game to match your style of play.";

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
            UIHelperBase group = helper.AddGroup(Name);

            int selectedIndex;
            float selectedValue;
            float result;

            selectedValue = ModConfig.Instance.Scaling;

            group.AddSlider("Scaling", 0.5f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.Scaling = sel;
                ModConfig.Instance.Save();
            });

            group.AddSpace(20);

            selectedValue = ModConfig.Instance.RowsExpanded;

            group.AddSlider("Rows (expanded)", 1f, 5f, 1f, selectedValue, sel =>
            {
                ModConfig.Instance.RowsExpanded = (int)sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.ColumnsExpanded;

            group.AddSlider("Columns (expanded)", 5f, 30f, 1f, selectedValue, sel =>
            {
                ModConfig.Instance.ColumnsExpanded = (int)sel;
                ModConfig.Instance.Save();
            });

            group.AddSpace(20);

            selectedValue = ModConfig.Instance.RowsCompressed;

            group.AddSlider("Rows (compressed)", 1f, 5f, 1f, selectedValue, sel =>
            {
                ModConfig.Instance.RowsCompressed = (int)sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.ColumnsCompressed;

            group.AddSlider("Columns (compressed)", 5f, 30f, 1f, selectedValue, sel =>
            {
                ModConfig.Instance.ColumnsCompressed = (int)sel;
                ModConfig.Instance.Save();
            });

            group.AddSpace(20);

            selectedIndex = GetSelectedOptionIndex(ScrollDirectionValues, ModConfig.Instance.ScrollDirection);

            group.AddDropdown("Scroll Direction", ScrollDirectionLabels, selectedIndex, sel =>
            {
                ModConfig.Instance.ScrollDirection = ScrollDirectionValues[sel];
                ModConfig.Instance.Save();
            });

            selectedIndex = GetSelectedOptionIndex(AlignmentValues, ModConfig.Instance.Alignment);

            group.AddDropdown("Alignment", AlignmentLabels, selectedIndex, sel =>
            {
                ModConfig.Instance.Alignment = AlignmentValues[sel];
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.HorizontalOffset;

            group.AddTextfield("Horizontal offset", selectedValue.ToString(), sel =>
            {
                float.TryParse(sel, out result);
                ModConfig.Instance.HorizontalOffset = result;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.VerticalOffset;

            group.AddTextfield("Vertical offset", selectedValue.ToString(), sel =>
            {
                float.TryParse(sel, out result);
                ModConfig.Instance.VerticalOffset = result;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.Opacity;

            group.AddSlider("Opacity", 0.05f, 1f, 0.05f, selectedValue, sel =>
            {
                ModConfig.Instance.Opacity = sel;
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