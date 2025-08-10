using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class ScreenResolutionsHelper
    {
        private static IEnumerable<Resolution> _availableResolutions;

        public static IEnumerable<Resolution> GetAvailableResolutions()
        {
            _availableResolutions ??= Screen.resolutions.Reverse();

            return _availableResolutions;
        }
    }
}