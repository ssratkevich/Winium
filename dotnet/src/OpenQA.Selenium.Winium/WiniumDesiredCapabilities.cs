using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using OpenQA.Selenium.Remote;

namespace OpenQA.Selenium.Winium
{
    public class WiniumDesiredCapabilities : ICapabilities
    {
        private readonly Dictionary<string, object> capabilities;

        public WiniumDesiredCapabilities(Dictionary<string, object> capabilities) =>
            this.capabilities = capabilities;

        public string BrowserName
        {
            get
            {
                string empty = string.Empty;
                object capability = this.GetCapability(CapabilityType.BrowserName);
                if (capability != null)
                    empty = capability.ToString();
                return empty;
            }
        }

        public Platform Platform
        {
            get
            {
                if (!(this.GetCapability(CapabilityType.Platform) is Platform platform))
                    platform = new Platform(PlatformType.Any);
                return platform;
            }
        }

        public string Version
        {
            get
            {
                string empty = string.Empty;
                object capability = this.GetCapability(CapabilityType.Version);
                if (capability != null)
                    empty = capability.ToString();
                return empty;
            }
        }

        public bool AcceptInsecureCerts
        {
            get
            {
                bool flag = false;
                object capability = this.GetCapability(CapabilityType.AcceptInsecureCertificates);
                if (capability != null)
                    flag = (bool) capability;
                return flag;
            }
        }

        internal IDictionary<string, object> CapabilitiesDictionary => (IDictionary<string, object>) new ReadOnlyDictionary<string, object>((IDictionary<string, object>) this.capabilities);

        public object this[string capabilityName] => this.capabilities.ContainsKey(capabilityName) ? this.capabilities[capabilityName] : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The capability {0} is not present in this set of capabilities", (object) capabilityName));

        public bool HasCapability(string capability) => this.capabilities.ContainsKey(capability);

        public object GetCapability(string capability)
        {
            object obj = (object) null;
            if (this.capabilities.ContainsKey(capability))
            {
                obj = this.capabilities[capability];
                string str = obj as string;
                if (capability == CapabilityType.Platform && str != null)
                {
                    obj = (object) FromString(obj.ToString());
                }
            }
            return obj;
        }

        public IDictionary<string, object> ToDictionary() => (IDictionary<string, object>) new ReadOnlyDictionary<string, object>((IDictionary<string, object>) this.capabilities);

        public override int GetHashCode() => 31 * (31 * (this.BrowserName != null ? this.BrowserName.GetHashCode() : 0) + (this.Version != null ? this.Version.GetHashCode() : 0)) + (this.Platform != null ? this.Platform.GetHashCode() : 0);

        public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Capabilities [BrowserName={0}, Platform={1}, Version={2}]", (object) this.BrowserName, (object) this.Platform.PlatformType.ToString(), (object) this.Version);

        public override bool Equals(object obj) => this == obj || obj is WiniumDesiredCapabilities desiredCapabilities && !(this.BrowserName != null ? this.BrowserName != desiredCapabilities.BrowserName : desiredCapabilities.BrowserName != null) && this.Platform.IsPlatformType(desiredCapabilities.Platform.PlatformType) && !(this.Version != null ? this.Version != desiredCapabilities.Version : desiredCapabilities.Version != null);

        internal static Platform FromString(string platformName)
        {
            if (!Enum.TryParse<PlatformType>(platformName, out var typeValue))
            {
                typeValue = PlatformType.Any;
            }

            return new Platform(typeValue);
        }
    }
}
