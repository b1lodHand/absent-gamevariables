using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.absence.gamevariables
{
    public class GameVariableBank : ScriptableObject
    {
        public static readonly string Null = "null: null";

        [SerializeField] protected List<GameVariable_Integer> Ints = new();
        [SerializeField] protected List<GameVariable_Float> Floats = new();
        [SerializeField] protected List<GameVariable_String> Strings = new();
        [SerializeField] protected List<GameVariable_Boolean> Booleans = new();

        public List<string> GetAllVariableNames()
        {
            var totalCount = Ints.Count + Floats.Count + Strings.Count + Booleans.Count;
            var result = new List<string>();
            if (totalCount == 0) return result;

            Ints.ConvertAll(v => v.Name).ToList().ForEach(k => result.Add(k));
            Floats.ConvertAll(v => v.Name).ToList().ForEach(k => result.Add(k));
            Strings.ConvertAll(v => v.Name).ToList().ForEach(k => result.Add(k));
            Booleans.ConvertAll(v => v.Name).ToList().ForEach(k => result.Add(k));

            return result;
        }

        public List<string> GetAllVariableNamesWithTypes()
        {
            var totalCount = Ints.Count + Floats.Count + Strings.Count + Booleans.Count;
            var result = new List<string>();
            if (totalCount == 0) return result;

            Ints.ConvertAll(v => v.Name).ToList().ForEach(k => result.Add($"int: {k}"));
            Floats.ConvertAll(v => v.Name).ToList().ForEach(k => result.Add($"float: {k}"));
            Strings.ConvertAll(v => v.Name).ToList().ForEach(k => result.Add($"string: {k}"));
            Booleans.ConvertAll(v => v.Name).ToList().ForEach(k => result.Add($"bool: {k}"));

            return result;
        }

        public GameVariable_Integer GetInt(string variableName)
        {
            variableName = TrimVariableNameType(variableName);

            var check = Ints.Where(v => v.Name == variableName).ToList();
            if (check.Count > 0) return check.FirstOrDefault();
            else return null;
        }
        public GameVariable_Float GetFloat(string variableName)
        {
            variableName = TrimVariableNameType(variableName);

            var check = Floats.Where(v => v.Name == variableName).ToList();
            if (check.Count > 0) return check.FirstOrDefault();
            else return null;
        }
        public GameVariable_String GetString(string variableName)
        {
            variableName = TrimVariableNameType(variableName);

            var check = Strings.Where(v => v.Name == variableName).ToList();
            if (check.Count > 0) return check.FirstOrDefault();
            else return null;
        }
        public GameVariable_Boolean GetBoolean(string variableName)
        {
            variableName = TrimVariableNameType(variableName);

            var check = Booleans.Where(v => v.Name == variableName).ToList();
            if (check.Count > 0) return check.FirstOrDefault();
            else return null;
        }

        public bool SetInt(string variableName, int newValue)
        {
            variableName = TrimVariableNameType(variableName);

            var found = Ints.Where(v => v.Name == variableName).FirstOrDefault();
            if(found == null) return false;

            found.Value = newValue;
            return true;
        }
        public bool SetFloat(string variableName, float newValue)
        {
            variableName = TrimVariableNameType(variableName);

            var found = Floats.Where(v => v.Name == variableName).FirstOrDefault();
            if (found == null) return false;

            found.Value = newValue;
            return true;
        }
        public bool SetString(string variableName, string newValue)
        {
            variableName = TrimVariableNameType(variableName);

            var found = Strings.Where(v => v.Name == variableName).FirstOrDefault();
            if (found == null) return false;

            found.Value = newValue;
            return true;
        }
        public bool SetBoolean(string variableName, bool newValue)
        {
            variableName = TrimVariableNameType(variableName);

            var found = Booleans.Where(v => v.Name == variableName).FirstOrDefault();
            if (found == null) return false;

            found.Value = newValue;
            return true;
        }

        public bool HasInt(string variableName) => Ints.Any(v => v.Name == TrimVariableNameType(variableName));
        public bool HasFloat(string variableName) => Floats.Any(v => v.Name == TrimVariableNameType(variableName));
        public bool HasString(string variableName) => Strings.Any(v => v.Name == TrimVariableNameType(variableName));
        public bool HasBoolean(string variableName) => Booleans.Any(v => v.Name == TrimVariableNameType(variableName));
        public bool HasAny(string variableName)
        {
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

    public class GameVariable<T>
    {
        protected event Action<GameVariableValueChangedCallbackContext<T>> m_onValueChanged;

        [SerializeField] protected string m_name;
        public string Name { get => m_name; protected set => m_name = value; }

        [SerializeField] protected T m_value;
        public T Value
        {
            get
            {
                return m_value;
            }

            set
            {
                var previous = m_value;
                var context = new GameVariableValueChangedCallbackContext<T>() { previousValue = previous, newValue = value };
                m_onValueChanged?.Invoke(context);
                m_value = value;
            }
        }

        public void RegisterOnValueChangedEvent(Action<GameVariableValueChangedCallbackContext<T>> evt)
        {
            m_onValueChanged += evt;
        }

        public void UnregisterOnValueChangedEvent(Action<GameVariableValueChangedCallbackContext<T>> evt)
        {
            m_onValueChanged -= evt;
        }
    }

    [System.Serializable] public class GameVariable_Integer : GameVariable<int> { }
    [System.Serializable] public class GameVariable_Float : GameVariable<float> { }
    [System.Serializable] public class GameVariable_String : GameVariable<string> { }
    [System.Serializable] public class GameVariable_Boolean : GameVariable<bool> { }

    public class GameVariableValueChangedCallbackContext<T>
    {
        public T previousValue { get; set; }
        public T newValue { get; set; }
    }
}