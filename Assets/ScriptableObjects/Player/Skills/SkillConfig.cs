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

    public class SkillConfig : ScriptableObject, ISkillConfig
    {
        protected virtual string[] ExcludedFieldNames { get; set; }

        public IEnumerable<string> GetConfigFieldNames()
            => GetType().GetFields(BindingFlags.Public).Select(f => f.ToString());

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

            object currentValue = fieldInfo.GetValue(fieldInfo);

            if (currentValue is int intValue)
            {
                fieldInfo.SetValue(GetType(), intValue + (int)value);
            }
            else if (currentValue is float floatValue)
            {
                fieldInfo.SetValue(GetType(), floatValue + value);
            }
            else
            {
                throw new ArgumentException($"Type of {fieldName} is not handled");
            }
        }
    }
}
