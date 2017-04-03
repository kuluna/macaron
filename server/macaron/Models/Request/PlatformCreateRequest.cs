using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace macaron.Models.Request
{
    /// <summary>
    /// Request body(Platform create)
    /// </summary>
    public class PlatformCreateRequest
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Convert to platform
        /// </summary>
        /// <returns>Generate new model</returns>
        public Platform ToPlatform()
        {
            return new Platform()
            {
                Name = Name
            };
        }

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="platform">model</param>
        public void Update(ref Platform platform)
        {
            platform.Name = Name;
        }
    }
}
