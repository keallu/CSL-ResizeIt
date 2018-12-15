namespace ResizeIt
{
    [ConfigurationPath("ResizeItConfig.xml")]
    public class ModConfig
    {
        public bool ConfigUpdated { get; set; }
        public string DefaultMode { get; set; }
        public bool FastSwitchingEnabled { get; set; }
        public float ScalingExpanded { get; set; }
        public int RowsExpanded { get; set; }
        public int ColumnsExpanded { get; set; }
        public string ScrollDirectionExpanded { get; set; }
        public string AlignmentExpanded { get; set; }
        public float HorizontalOffsetExpanded { get; set; }
        public float VerticalOffsetExpanded { get; set; }
        public float OpacityExpanded { get; set; }
        public float ScalingCompressed { get; set; }
        public int RowsCompressed { get; set; }
        public int ColumnsCompressed { get; set; }
        public string ScrollDirectionCompressed { get; set; }
        public string AlignmentCompressed { get; set; }
        public float HorizontalOffsetCompressed { get; set; }
        public float VerticalOffsetCompressed { get; set; }
        public float OpacityCompressed { get; set; }

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