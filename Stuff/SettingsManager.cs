using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.Exceptions;

namespace Stuff
{
    public class SettingsManager
    {
        private readonly Dictionary<string, Dictionary<string, string[]>> settings;

        internal SettingsManager()
        {
            settings = new Dictionary<string, Dictionary<string, string[]>>();
        }

        internal SettingsManager(Dictionary<string, Dictionary<string, string[]>> settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Adds the specified category to the SettingsManager.
        /// </summary>
        /// <param name="category">The name of the new category.</param>
        /// <returns>Whether the SettingsManager already contained the category.</returns>
        public bool AddCategory(string category)
        {
            bool result = settings.ContainsKey(category);
            settings.Add(category, new Dictionary<string, string[]>());
            return result;
        }

        /// <summary>
        /// Adds a setting to a category with a specified key and values.
        /// </summary>
        /// <param name="category">The category to place the setting in.</param>
        /// <param name="key">The key to the setting.</param>
        /// <param name="values">The value to be tied to the key.</param>
        /// <returns>Whether the category already contained a key with the same name.</returns>
        public bool AddSetting(string category, string key, params string[] values)
        {
            bool result = settings[category].ContainsKey(key);
            settings[category].Add(key, values);
            return result;
        }

        public void SetSetting(string category, string key, int index, string value)
        {
            settings[category][key][index] = value;
        }
        
        public void SetSetting(string category, string key, string value)
        {
            settings[category][key][0] = value;
        }

        public IEnumerable<KeyValuePair<string, string[]>> SettingsInCategory(string category)
        {
            foreach (string key in settings[category].Keys)
                yield return new KeyValuePair<string, string[]>(key, settings[category][key]);
        }

        public int IndexesInSetting(string category, string key)
        {
            return settings[category][key].Length;
        }

        public IEnumerable<string> Categories()
        {
            return settings.Keys;
        }

        public string GetString(string category, string key, int index = 0)
        {
            if (!settings.ContainsKey(category))
                throw new CategoryNotFoundException(category, key);
            if (!settings[category].ContainsKey(key))
                throw new SettingsKeyNotFoundException(category, key);
            if (settings[category][key].Length <= index)
                throw new IndexNotFoundException(category, key, index);
            return settings[category][key][index];
        }

        public IEnumerable<string> GetStrings(string category, string key)
        {
            if (!settings.ContainsKey(category))
                throw new CategoryNotFoundException(category, key);
            if (!settings[category].ContainsKey(key))
                throw new SettingsKeyNotFoundException(category, key);
            return settings[category][key];
        }

        public int GetInt(string category, string key, int index = 0)
        {
            if (int.TryParse(GetString(category, key, index), out int result))
                return result;
            else
                throw new InvalidSettingFormat(category, key, index);
        }

        public IEnumerable<int> GetInts(string category, string key)
        {
            for (int i = 0; i < settings[category][key].Length; ++i)
                yield return GetInt(category, key, i);
        }

        public long GetLong(string category, string key, int index = 0)
        {
            if (long.TryParse(GetString(category, key, index), out long result))
                return result;
            else
                throw new InvalidSettingFormat(category, key, index);
        }

        public IEnumerable<long> GetLongs(string category, string key)
        {
            for (int i = 0; i < settings[category][key].Length; ++i)
                yield return GetLong(category, key, i);
        }

        public float GetFloat(string category, string key, int index = 0)
        {
            if (float.TryParse(GetString(category, key, index), out float result))
                return result;
            else
                throw new InvalidSettingFormat(category, key, index);
        }

        public IEnumerable<float> GetFloats(string category, string key)
        {
            for (int i = 0; i < settings[category][key].Length; ++i)
                yield return GetFloat(category, key, i);
        }

        public double GetDouble(string category, string key, int index = 0)
        {
            if (double.TryParse(GetString(category, key, index), out double result))
                return result;
            else
                throw new InvalidSettingFormat(category, key, index);
        }

        public IEnumerable<double> GetDoubles(string category, string key)
        {
            for (int i = 0; i < settings[category][key].Length; ++i)
                yield return GetDouble(category, key, i);
        }

        public bool GetBool(string category, string key, int index = 0)
        {
            if (bool.TryParse(GetString(category, key, index), out bool result))
                return result;
            else
                throw new InvalidSettingFormat(category, key, index);
        }

        public IEnumerable<bool> GetBools(string category, string key)
        {
            for (int i = 0; i < settings[category][key].Length; ++i)
                yield return GetBool(category, key, i);
        }
    }
}
