﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetControlClient.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.1.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
  <string>DESKTOP-DNLCJ2N</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection Servers {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["Servers"]));
            }
            set {
                this["Servers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int RefreshPeriod {
            get {
                return ((int)(this["RefreshPeriod"]));
            }
            set {
                this["RefreshPeriod"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1920,1080")]
        public string ScreenshotSize {
            get {
                return ((string)(this["ScreenshotSize"]));
            }
            set {
                this["ScreenshotSize"] = value;
            }
        }
    }
}
