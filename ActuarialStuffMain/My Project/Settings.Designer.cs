namespace ActuarialStuff
{
    [global::System.Runtime.CompilerServices.CompilerGenerated()]
    [global::System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.2.0.0")]
    [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {
        private static Settings defaultInstance = (Settings)global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings());

        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public static Settings Default
        {
            get
            {

                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                return defaultInstance;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("0")]
        public double WindowLeft
        {
            get
            {
                return System.Convert.ToDouble(this["WindowLeft"]);
            }
            set
            {
                this["WindowLeft"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("0")]
        public double WindowTop
        {
            get
            {
                return System.Convert.ToDouble(this["WindowTop"]);
            }
            set
            {
                this["WindowTop"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("300")]
        public double WindowWidth
        {
            get
            {
                return System.Convert.ToDouble(this["WindowWidth"]);
            }
            set
            {
                this["WindowWidth"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("300")]
        public double WindowHeight
        {
            get
            {
                return System.Convert.ToDouble(this["WindowHeight"]);
            }
            set
            {
                this["WindowHeight"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("50")]
        public double ViewColumn0Width
        {
            get
            {
                return System.Convert.ToDouble(this["ViewColumn0Width"]);
            }
            set
            {
                this["ViewColumn0Width"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("50")]
        public double CompareColumn0Width
        {
            get
            {
                return System.Convert.ToDouble(this["CompareColumn0Width"]);
            }
            set
            {
                this["CompareColumn0Width"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("50")]
        public double FilterColumn0Width
        {
            get
            {
                return System.Convert.ToDouble(this["FilterColumn0Width"]);
            }
            set
            {
                this["FilterColumn0Width"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("50")]
        public double FilterColumn2Width
        {
            get
            {
                return System.Convert.ToDouble(this["FilterColumn2Width"]);
            }
            set
            {
                this["FilterColumn2Width"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("50")]
        public double FilterCol0Row0Height
        {
            get
            {
                return System.Convert.ToDouble(this["FilterCol0Row0Height"]);
            }
            set
            {
                this["FilterCol0Row0Height"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("50")]
        public double FilterCol0Row2Height
        {
            get
            {
                return System.Convert.ToDouble(this["FilterCol0Row2Height"]);
            }
            set
            {
                this["FilterCol0Row2Height"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("50")]
        public double FilterCol2Row0Height
        {
            get
            {
                return System.Convert.ToDouble(this["FilterCol2Row0Height"]);
            }
            set
            {
                this["FilterCol2Row0Height"] = value;
            }
        }

        [global::System.Configuration.UserScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Configuration.DefaultSettingValue("50")]
        public double FilterCol2Row2Height
        {
            get
            {
                return System.Convert.ToDouble(this["FilterCol2Row2Height"]);
            }
            set
            {
                this["FilterCol2Row2Height"] = value;
            }
        }
    }

    namespace My
    {
        [global::Microsoft.VisualBasic.HideModuleName()]
        [global::System.Diagnostics.DebuggerNonUserCode()]
        [global::System.Runtime.CompilerServices.CompilerGenerated()]
        internal static class MySettingsProperty
        {
            [global::System.ComponentModel.Design.HelpKeyword("My.Settings")]
            internal static global::ActuarialStuff.Settings Settings
            {
                get
                {
                    return global::ActuarialStuff.Settings.Default;
                }
            }
        }
    }
}
