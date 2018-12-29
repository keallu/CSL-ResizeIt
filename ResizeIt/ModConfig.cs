namespace ResizeIt
{
    [ConfigurationPath("ResizeItConfig.xml")]
    public class ModConfig
    {
        public bool ConfigUpdated { get; set; } = false;
        public string DefaultMode { get; set; } = "Expanded mode";
        public bool FastSwitchingEnabled { get; set; } = true;
        public bool ControlPanelEnabled { get; set; } = true;
        public string ControlPanelAlignment { get; set; } = "Left";
        public float ControlPanelOpacity { get; set; } = 1f;
        public bool ControlPanelFastSwitchingEnabled { get; set; } = true;
        public float ScalingExpanded { get; set; } = 1f;
        public int RowsExpanded { get; set; } = 3;
        public int ColumnsExpanded { get; set; } = 7;
        public string ScrollDirectionExpanded { get; set; } = "Vertically";
        public string AlignmentExpanded { get; set; } = "Fixed";
        public float HorizontalOffsetExpanded { get; set; } = 0f;
        public float VerticalOffsetExpanded { get; set; } = 0f;
        public float OpacityExpanded { get; set; } = 1f;
        public float ScalingCompressed { get; set; } = 1f;
        public int RowsCompressed { get; set; } = 1;
        public int ColumnsCompressed { get; set; } = 7;
        public string ScrollDirectionCompressed { get; set; } = "Horizontally";
        public string AlignmentCompressed { get; set; } = "Fixed";
        public float HorizontalOffsetCompressed { get; set; } = 0f;
        public float VerticalOffsetCompressed { get; set; } = 0f;
        public float OpacityCompressed { get; set; } = 1f;

        private static ModConfig instance;

        public static ModConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Configuration<ModConfig>.Load();
                }

                return instance;
            }
        }

        public void Save()
        {
            Configuration<ModConfig>.Save();
            ConfigUpdated = true;
        }
    }
}