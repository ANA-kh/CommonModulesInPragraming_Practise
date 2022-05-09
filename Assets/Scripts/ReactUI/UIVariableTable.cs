using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReactUI
{
    [AddComponentMenu("ReactUI/UI/Bind/UI Variable Table")]
    public sealed class UIVariableTable : MonoBehaviour
    {
        [SerializeField]
        private UIVariable[] variables;

        public UIVariable[] Variables { get; set; }

        public UIVariable GetVariable(int index)
        {
            return variables[index];
        }

        public void AddDefaultVariable()
        {
            UIVariable uIVariable = new UIVariable();
            if (variables == null)
            {
                variables = new UIVariable[1];
                variables[0] = uIVariable;
            }
            else
            {
                UIVariable[] var_new = new UIVariable[variables.Length+1];
                for(int i=0;i< variables.Length;i++)
                {
                    var_new[i] = variables[i];
                }
                var_new[var_new.Length - 1] = uIVariable;
                variables = var_new;
            }
        }

        public void Sort()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public sealed class UIVariable
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private UIVariableType type;

        [SerializeField]
        private bool booleanValue;

        [SerializeField]
        private long integerValue;

        [SerializeField]
        private float floatValue;

        [SerializeField]
        private string stringValue;

        public ICollection<UIVariableBind> Binds => binderList;
        private List<UIVariableBind> binderList = new List<UIVariableBind>();
        public string Name => name;
    }

    public class UIVariableBind
    {
    }

    public enum UIVariableType
    {
        Boolean = 0,
        Integer = 1,
        Float = 2 ,
        String = 3,
        Array = 5,
        Object = 6,
    }
}