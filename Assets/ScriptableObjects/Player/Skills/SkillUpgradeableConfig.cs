using Assets.Scripts.CustomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.ScriptableObjects.Player.Skills
{
    public interface ISkillConfig
    {
        IEnumerable<string> GetConfigFieldNames();

        void UpdateConfigStatByFieldName(string fieldName, float value);
    }

    public class SkillUpgradeableConfig : ScriptableObject, ISkillConfig
    {
        protected virtual string[] ExcludedFieldNames { get; set; }

        public IEnumerable<string> GetConfigFieldNames()
            => GetType().GetFields(BindingFlags.Public).Select(f => f.ToString());

        public void UpdateConfigStatByFieldName(string fieldName, ValueRange<float> valueRange)
        {
            UpdateConfigStatByFieldName(fieldName, valueRange.GetRandomValueInRange());
        }

        public void UpdateConfigStatByFieldName(string fieldName, float value)
        {
            if (ExcludedFieldNames.Contains(fieldName))
            {
                return;
            }

            FieldInfo fieldInfo = GetType().GetField(fieldName, BindingFlags.Public);

            if (fieldInfo == null)
            {
                return;
            }

            object valueObject = fieldInfo.GetValue(fieldInfo);

            HandleFieldUpgrade(valueObject, fieldInfo, value);
        }

        protected virtual void HandleFieldUpgrade(object valueObject, FieldInfo fieldInfo, float value)
        {
            if (valueObject is int intValue)
            {
                fieldInfo.SetValue(GetType(), intValue + (int)value);
            }
            else if (valueObject is float floatValue)
            {
                fieldInfo.SetValue(GetType(), floatValue + value);
            }
            else
            {
                throw new ArgumentException($"Type of {fieldInfo.Name} is not handled");
            }
        }
    }
}
