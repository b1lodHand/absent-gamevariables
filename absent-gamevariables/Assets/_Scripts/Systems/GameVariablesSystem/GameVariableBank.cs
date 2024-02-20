using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.absence.gamevariables
{
    [CreateAssetMenu(menuName = "Game/Game Variable Bank")]
    public class GameVariableBank : ScriptableObject
    {
        public static readonly string Null = "null: null";

        [SerializedDictionary("Name", "Value")] public SerializedDictionary<string, int> Ints;
        [SerializedDictionary("Name", "Value")] public SerializedDictionary<string, float> Floats;
        [SerializedDictionary("Name", "Value")] public SerializedDictionary<string, string> Strings;
        [SerializedDictionary("Name", "Value")] public SerializedDictionary<string, bool> Booleans;

        public List<string> GetAllVariableNames()
        {
            var totalCount = Ints.Count + Floats.Count + Strings.Count + Booleans.Count;
            var result = new List<string>();
            if (totalCount == 0) return result;

            Ints.Keys.ToList().ForEach(k => result.Add(k));
            Floats.Keys.ToList().ForEach(k => result.Add(k));
            Strings.Keys.ToList().ForEach(k => result.Add(k));
            Booleans.Keys.ToList().ForEach(k => result.Add(k));

            return result;
        }

        public List<string> GetAllVariableNamesWithTypes()
        {
            var totalCount = Ints.Count + Floats.Count + Strings.Count + Booleans.Count;
            var result = new List<string>();
            if (totalCount == 0) return result;

            Ints.Keys.ToList().ForEach(k => result.Add($"int: {k}"));
            Floats.Keys.ToList().ForEach(k => result.Add($"float: {k}"));
            Strings.Keys.ToList().ForEach(k => result.Add($"string: {k}"));
            Booleans.Keys.ToList().ForEach(k => result.Add($"bool: {k}"));

            return result;
        }

        public int? GetInt(string variableName)
        {
            variableName = TrimVariableNameType(variableName);

            if (Ints.TryGetValue(variableName, out int result)) return result;
            else return null;
        }
        public float? GetFloat(string variableName)
        {
            variableName = TrimVariableNameType(variableName);

            if (Floats.TryGetValue(variableName, out float result)) return result;
            else return null;
        }
        public string GetString(string variableName)
        {
            variableName = TrimVariableNameType(variableName);

            if (Strings.TryGetValue(variableName, out string result)) return result;
            else return null;
        }
        public bool? GetBoolean(string variableName)
        {
            variableName = TrimVariableNameType(variableName);

            if (Booleans.TryGetValue(variableName, out bool result)) return result;
            else return null;
        }

        public bool SetInt(string variableName, int newValue)
        {
            variableName = TrimVariableNameType(variableName);

            if (!Ints.ContainsKey(variableName)) return false;
            Ints[variableName] = newValue;
            return true;
        }
        public bool SetFloat(string variableName, float newValue)
        {
            variableName = TrimVariableNameType(variableName);

            if (!Floats.ContainsKey(variableName)) return false;
            Floats[variableName] = newValue;
            return true;
        }
        public bool SetString(string variableName, string newValue)
        {
            variableName = TrimVariableNameType(variableName);

            if (Strings.ContainsKey(variableName)) return false;
            Strings[variableName] = newValue;
            return true;
        }
        public bool SetBoolean(string variableName, bool newValue)
        {
            variableName = TrimVariableNameType(variableName);

            if (!Booleans.ContainsKey(variableName)) return false;
            Booleans[variableName] = newValue;
            return true;
        }

        public bool HasInt(string variableName) => Ints.ContainsKey(TrimVariableNameType(variableName));
        public bool HasFloat(string variableName) => Floats.ContainsKey(TrimVariableNameType(variableName));
        public bool HasString(string variableName) => Strings.ContainsKey(TrimVariableNameType(variableName));
        public bool HasBoolean(string variableName) => Booleans.ContainsKey(TrimVariableNameType(variableName));
        public bool HasAny(string variableName)
        {
            variableName = TrimVariableNameType(variableName);

            return (this.HasInt(variableName) ||
                    this.HasFloat(variableName) ||
                    this.HasString(variableName) ||
                    this.HasBoolean(variableName));
        }

        public string TrimVariableNameType(string nameToTrim)
        {
            if (!nameToTrim.Contains(':')) return nameToTrim;
            return nameToTrim.Split(':')[1].Trim();
        }
        public string AddVariableNameType(string nameToAddType)
        {
            StringBuilder builder = new StringBuilder();
            if (this.HasInt(nameToAddType)) builder.Append("int: ");
            else if (this.HasFloat(nameToAddType)) builder.Append("float: ");
            else if (this.HasString(nameToAddType)) builder.Append("string: ");
            else if (this.HasBoolean(nameToAddType)) builder.Append("bool: ");

            builder.Append(" ");
            builder.Append(nameToAddType);

            return builder.ToString();
        }
    }

}