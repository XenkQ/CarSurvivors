using Assets.Scripts.Storage;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.ScoreBoard
{
    public class StoredScoreBoard : IAppStorageValue<List<uint>>
    {
        public List<uint> DefaultValue => new();
        public byte MAX_SAVED_SCORES_COUNT = 6;

        public string GetKey()
        {
            return "ScoreBoard";
        }

        public List<uint> GetValueOrStoredDefault()
        {
            if (AppStorage.TryGetValue<List<uint>>(GetKey(), out var value))
            {
                return value;
            }

            return DefaultValue;
        }

        public void SaveValue(List<uint> value)
        {
            if (value.Count > MAX_SAVED_SCORES_COUNT)
            {
                throw new ArgumentException($"StoredScoreBoard can't have more then {MAX_SAVED_SCORES_COUNT} scores.");
            }

            AppStorage.SetValue(GetKey(), value);
        }
    }
}
