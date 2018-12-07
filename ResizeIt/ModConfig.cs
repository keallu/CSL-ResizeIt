namespace ResizeIt
{
    [ConfigurationPath("ResizeItConfig.xml")]
    public class ModConfig
    {
        public bool ConfigUpdated { get; set; }
        public float Scaling { get; set; }
        public int RowsExpanded { get; set; }
        public int ColumnsExpanded { get; set; }
        public int RowsCompressed { get; set; }
        public int ColumnsCompressed { get; set; }
        public string ScrollDirection { get; set; }
        public string Alignment { get; set; }
        public float HorizontalOffset { get; set; }
        public float VerticalOffset { get; set; }
        public float Opacity { get; set; }

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