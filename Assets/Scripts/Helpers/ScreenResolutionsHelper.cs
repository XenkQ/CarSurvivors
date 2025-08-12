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
            if(_availableResolutions is null)
            {
                var resolutions = Screen.resolutions;
                var currentRefreshRatio = Screen.currentResolution.refreshRateRatio;

                for (int i = resolutions.Length - 1; i >= 0; i--)
                {
                    resolutions[i].refreshRateRatio = currentRefreshRatio;
                }

                _availableResolutions = resolutions.Reverse();
            }

            return _availableResolutions;
        }
    }
}