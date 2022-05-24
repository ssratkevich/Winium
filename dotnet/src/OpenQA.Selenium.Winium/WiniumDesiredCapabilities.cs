using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using OpenQA.Selenium.Remote;

namespace OpenQA.Selenium.Winium
{
    /// <summary>
    /// Capabilities created from given dictionary.
    /// </summary>
    public class WiniumDesiredCapabilities : ICapabilities
    {
        private readonly Dictionary<string, object> capabilities;

        /// <summary>
        /// Creates new instance of a Capabilities.
        /// </summary>
        /// <param name="capabilities">Capability dictionary.</param>
        public WiniumDesiredCapabilities(Dictionary<string, object> capabilities) =>
            this.capabilities = capabilities;

        /// <summary>
        /// Browser name.
        /// </summary>
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

        /// <summary>
        /// Platform.
        /// </summary>
        public Platform Platform
        {
            get
            {
                if (!(this.GetCapability(CapabilityType.Platform) is Platform platform))
                    platform = new Platform(PlatformType.Any);
                return platform;
            }
        }

        /// <summary>
        /// Version.
        /// </summary>
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

        /// <summary>
        /// Indicates whether the browser accepts SSL certificates on W3C Endpoints.
        /// </summary>
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

        /// <inheritdoc/>
        public object this[string capabilityName] => this.capabilities.ContainsKey(capabilityName) ? this.capabilities[capabilityName] : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The capability {0} is not present in this set of capabilities", (object) capabilityName));

        /// <inheritdoc/>
        public bool HasCapability(string capability) => this.capabilities.ContainsKey(capability);

        /// <inheritdoc/>
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

        /// <summary>
        /// Get capabilities dictionary.
        /// </summary>
        /// <returns>Capabilities dictionary.</returns>
        public IDictionary<string, object> ToDictionary() => (IDictionary<string, object>) new ReadOnlyDictionary<string, object>((IDictionary<string, object>) this.capabilities);

        /// <inheritdoc/>
        public override int GetHashCode() => 31 * (31 * (this.BrowserName != null ? this.BrowserName.GetHashCode() : 0) + (this.Version != null ? this.Version.GetHashCode() : 0)) + (this.Platform != null ? this.Platform.GetHashCode() : 0);

        /// <inheritdoc/>
        public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Capabilities [BrowserName={0}, Platform={1}, Version={2}]", (object) this.BrowserName, (object) this.Platform.PlatformType.ToString(), (object) this.Version);

        /// <inheritdoc/>
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
