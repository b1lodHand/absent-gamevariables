using com.absence.gamevariables;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private List<GVComparer> m_comparers = new();
    [SerializeField] private List<GVSetter> m_setters = new();

    private void Awake()
    {
        m_comparers.ForEach(comparer => comparer.GetResult());
        m_setters.ForEach(setter => setter.Perform());
    }

    private void OnValidate()
    {   
        m_comparers.ForEach(comparer => comparer.Refresh());
        m_setters.ForEach(setter => setter.Refresh());
    }
}
