using com.absence.gamevariables;
using com.absence.variablebanks;
using com.absence.variablesystem;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private List<VariableComparer> m_comparers = new();
    [SerializeField] private List<VariableSetter> m_setters = new();
}
