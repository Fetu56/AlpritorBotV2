﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlpritorBotV2.Localization.Bot {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AlpritorBotV2.Localization.Bot.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to English.
        /// </summary>
        internal static string CurrentLocale {
            get {
                return ResourceManager.GetString("CurrentLocale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To unlock more features of bot, /mod me please :3.
        /// </summary>
        internal static string GiveMeModeMsg {
            get {
                return ResourceManager.GetString("GiveMeModeMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hello world!.
        /// </summary>
        internal static string HelloMsg {
            get {
                return ResourceManager.GetString("HelloMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stream uptime.
        /// </summary>
        internal static string Uptime {
            get {
                return ResourceManager.GetString("Uptime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stream offline.
        /// </summary>
        internal static string UptimeFail {
            get {
                return ResourceManager.GetString("UptimeFail", resourceCulture);
            }
        }
    }
}
