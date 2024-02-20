using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;
using com.absence.gamevariables;

[CustomPropertyDrawer(typeof(GameVariableSetter), true)]
public class GameVariableSetterDrawer : PropertyDrawer
{
    GameVariableBank m_mainBank;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new VisualElement();
        root.style.flexShrink = 1;
        root.style.alignSelf = Align.Stretch;

        var container = DrawGUI(property);

        root.Add(container);
        return root;
    }

    private VisualElement DrawGUI(SerializedProperty property)
    {
        // get serialized object.
        var serializedObject = property.serializedObject;

        // get properties.
        //var bankSelectorProp = property.FindPropertyRelative("BankSelector");
        var bankProp = property.FindPropertyRelative("TargetBank");
        var targetVarNameProp = property.FindPropertyRelative("TargetVariableName");

        var intValueProp = property.FindPropertyRelative("IntValue");
        var floatValueProp = property.FindPropertyRelative("FloatValue");
        var stringValueProp = property.FindPropertyRelative("StringValue");
        var boolValueProp = property.FindPropertyRelative("BoolValue");

        // declare needed variables.
        bool hasBankSelectorProp = false;
        GameVariableBank targetBank = null;

        // refresh banks.
        RefreshMainBank();
        RefreshTargetBank();

        // instantiate the main container.
        VisualElement container = new VisualElement();
        container.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/GameVariableEditor/GameVariableSetter.uss"));
        container.AddToClassList("container");

        //// instantiate selector for bank.
        //EnumField bankSelector = new EnumField();
        //bankSelector.BindProperty(bankSelectorProp);
        //bankSelector.AddToClassList("bankSelector");

        // instantiate selector for variable.
        DropdownField variableSelector = new DropdownField(new List<string>() { GameVariableBank.Null }, 0);
        variableSelector.AddToClassList("varSelector");
        variableSelector.BindProperty(targetVarNameProp);

        // instantiate fields for actual values to check.
        #region Value Fields
        var intField = new IntegerField();
        intField.BindProperty(intValueProp);
        intField.AddToClassList("valueField");

        var floatField = new FloatField();
        floatField.BindProperty(floatValueProp);
        floatField.AddToClassList("valueField");

        var stringField = new TextField();
        stringField.BindProperty(stringValueProp);
        stringField.AddToClassList("valueField");

        var boolToggle = new Toggle();
        boolToggle.BindProperty(boolValueProp);
        boolToggle.AddToClassList("valueField");
        #endregion

        //// register bank selector on change.
        //bankSelector.RegisterValueChangedCallback(evt =>
        //{
        //    RefreshMainBank();
        //    RefreshTargetBank();
        //    RefreshVarSelector();
        //});

        // register var selector on change.
        variableSelector.RegisterValueChangedCallback(evt =>
        {
            RefreshValueFields(evt.newValue);
            RefreshCompSelector();
        });

        // refresh all of them.
        RefreshVarSelector();
        RefreshCompSelector();
        RefreshValueFields(variableSelector.value);

        // add all needed to the main container.
        //if (hasBankSelectorProp) container.Add(bankSelector);
        container.Add(variableSelector);

        return container;

        /* NEEDED METHODS */
        void RefreshTargetBank()
        {
            if (!hasBankSelectorProp)
            {
                targetBank = m_mainBank;
                bankProp.objectReferenceValue = m_mainBank;
                return;
            }

            //var selectorValue = (VariableBankOption)bankSelectorProp.enumValueIndex;
            //switch (selectorValue)
            //{
            //    case VariableBankOption.GameVariables:
            //        if (m_mainBank != null) targetBank = m_mainBank;
            //        else targetBank = null;
            //        break;

            //    case VariableBankOption.Blackboard:
            //        try
            //        {
            //            var blackboardBank = bankProp.FindPropertyRelative("BlackboardBank").objectReferenceValue as GameVariableBank;
            //            if (blackboardBank != null) targetBank = blackboardBank;
            //            else targetBank = null;
            //        }
            //        catch
            //        {
            //            targetBank = null;
            //        }
            //        break;

            //    default:
            //        targetBank = null;
            //        break;
            //}
            if (targetBank == null) bankProp.objectReferenceValue = null;
            else bankProp.objectReferenceValue = targetBank;
        }
        void RefreshVarSelector()
        {
            if (targetBank == null)
            {
                variableSelector.choices = new List<string>() { GameVariableBank.Null };

                variableSelector.value = GameVariableBank.Null;
                return;
            }

            var variableNamesWithTypes = targetBank.GetAllVariableNamesWithTypes();
            variableNamesWithTypes.Insert(0, GameVariableBank.Null);

            variableSelector.choices = variableNamesWithTypes;

            var currentVarName = targetVarNameProp.stringValue;
            if (targetBank.HasAny(currentVarName)) variableSelector.value = currentVarName;
            else variableSelector.value = GameVariableBank.Null;
        }
        void RefreshCompSelector()
        {
            if (targetBank == null) return;

            var currVarName = targetVarNameProp.stringValue;
            if (currVarName != GameVariableBank.Null && !(targetBank.HasString(currVarName)) &&
                !(targetBank.HasBoolean(currVarName))) return;
        }
        void RefreshValueFields(string selectedVarName)
        {
            if (container.Contains(intField)) container.Remove(intField);
            if (container.Contains(floatField)) container.Remove(floatField);
            if (container.Contains(stringField)) container.Remove(stringField);
            if (container.Contains(boolToggle)) container.Remove(boolToggle);

            if (selectedVarName == GameVariableBank.Null)
            {
                targetVarNameProp.stringValue = GameVariableBank.Null;
                return;
            }

            targetVarNameProp.stringValue = selectedVarName;

            if (targetBank == null) return;

            var targetVariableName = targetVarNameProp.stringValue;
            if (targetBank.HasInt(targetVariableName))
                container.Add(intField);
            else if (targetBank.HasFloat(targetVariableName))
                container.Add(floatField);
            else if (targetBank.HasString(targetVariableName))
                container.Add(stringField);
            else if (targetBank.HasBoolean(targetVariableName))
                container.Add(boolToggle);
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // needed stuff.
        label.text = "";
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // getting properties.
        var bankSelector = property.FindPropertyRelative("BankSelector");
        var bankProp = property.FindPropertyRelative("TargetBank");
        var varNameProp = property.FindPropertyRelative("TargetVariableName");

        var intProp = property.FindPropertyRelative("IntValue");
        var floatProp = property.FindPropertyRelative("FloatValue");
        var stringProp = property.FindPropertyRelative("StringValue");
        var boolProp = property.FindPropertyRelative("BoolValue");

        bool hasBankSelector = bankSelector != null;
        GameVariableBank targetBank = null;

        #region editor shit
        // holding default indent level and setting our own.
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // rect variables.
        var horizontalPointer = position.x;
        var horizontalSpace = 5f;
        var bankSelectorWidth = 80f;
        var variableSelectorWidth = 100f;
        var actualValueWidth = 50f;

        // rect calcs.
        var bankSelectorRect = new Rect(horizontalPointer, position.y, bankSelectorWidth, position.height);
        if(hasBankSelector)
        {
            horizontalPointer += bankSelectorWidth;
            horizontalPointer += horizontalSpace;
        }

        var variableSelectorRect = new Rect(horizontalPointer, position.y, variableSelectorWidth, position.height);

        horizontalPointer += variableSelectorWidth;
        horizontalPointer += horizontalSpace;

        // some calcs to expand the area of actual value field.
        var rest = position.width - horizontalPointer;
        actualValueWidth = rest > 0f ? rest : actualValueWidth;

        var actualValueRect = new Rect(horizontalPointer, position.y, actualValueWidth, position.height);
        #endregion

        // undo redo stuff.
        EditorGUI.BeginChangeCheck();
        Undo.RecordObject(property.serializedObject.targetObject, "Game Variable Comparer (Edited)");

        //if (hasBankSelector) bankSelector.enumValueIndex((VariableBankOption)EditorGUI.
        //    EnumPopup(bankSelectorRect, (VariableBankOption)bankSelector.enumValueIndex));

        if (m_mainBank == null) RefreshMainBank();
        if (!hasBankSelector) targetBank = m_mainBank;
        //else
        //{
        //    switch ((VariableBankOption)bankSelector.enumValueIndex)
        //    {
        //        case VariableBankOption.GameVariables:
        //            if (m_mainBank != null) targetBank = m_mainBank;
        //            else targetBank = null;
        //            break;

        //        case VariableBankOption.Blackboard:
        //            try
        //            {
        //                var blackboardBank = bankProp.FindPropertyRelative("BlackboardBank").objectReferenceValue as GameVariableBank;
        //                if (blackboardBank != null) targetBank = blackboardBank;
        //                else targetBank = null;
        //            }
        //            catch
        //            {
        //                targetBank = null;
        //            }
        //            break;

        //        default:
        //            targetBank = null;
        //            break;
        //    }
        //}

        List<string> allNamesWithTypes = new List<string> { GameVariableBank.Null };

        // getting all variables from bank.
        if (targetBank != null) allNamesWithTypes.AddRange(targetBank.GetAllVariableNamesWithTypes());

        // drawing variable selection.
        if (allNamesWithTypes.Count != 0)
        {
            varNameProp.stringValue = allNamesWithTypes[EditorGUI.Popup(variableSelectorRect,
                allNamesWithTypes.Contains(varNameProp.stringValue) ? allNamesWithTypes.IndexOf(varNameProp.stringValue) : 0, allNamesWithTypes.ToArray())];
        }

        // borrowing needed info from property to draw the rest.
        var targetVariableName = varNameProp.stringValue;

        // re-enabling the editor.
        GUI.enabled = true;

        if (targetBank != null)
        {
            // drawing the actual value depending on it's type.
            if (targetBank.HasInt(targetVariableName))
                intProp.intValue = EditorGUI.IntField(actualValueRect, intProp.intValue);
            else if (targetBank.HasFloat(targetVariableName))
                floatProp.floatValue = EditorGUI.FloatField(actualValueRect, floatProp.floatValue);
            else if (targetBank.HasString(targetVariableName))
                stringProp.stringValue = EditorGUI.TextField(actualValueRect, stringProp.stringValue);
            else if (targetBank.HasBoolean(targetVariableName))
                boolProp.boolValue = EditorGUI.Toggle(actualValueRect, boolProp.boolValue);
        }

        // setting indent back default.
        EditorGUI.indentLevel = indent;

        // finishing undo and marking target object as dirty.
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }

        // simply ending the property.
        EditorGUI.EndProperty();
    }

    private void RefreshMainBank()
    {
        if (m_mainBank == null)
        {
            var foundGuid = AssetDatabase.FindAssets("t:GameVariableBank l:GameVariables").ToList().FirstOrDefault();
            if (foundGuid != null) m_mainBank =
                    AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(foundGuid), typeof(GameVariableBank)) as GameVariableBank;
            else m_mainBank = null;
        }
    }
}
