using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEditor;
using UnityEngine;

namespace ReactUI
{
    /// <summary>
    /// VariableName外观
    /// </summary>
    [CustomPropertyDrawer(typeof(VariableNameAttribute))]
    internal sealed class VariableNameAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rc, SerializedProperty prop, GUIContent ctx)
        {
            EditorGUI.BeginProperty(rc, ctx, prop);
            UIVariableBind val = (UIVariableBind) prop.serializedObject.targetObject;
            if (val != null)
            {
                UIVariableTable variableTable = val.VariableTable;
                if (variableTable != null)
                {
                    string[] variableNames = variableTable.GetVariableNames();
                    if (variableNames != null)
                    {
                        DrawTable(rc, prop, ctx, variableTable);
                    }
                    else
                    {
                        GUI.enabled = false;
                        EditorGUI.PropertyField(rc, prop);
                        GUI.enabled = true;
                    }
                }
                else
                {
                    GUI.enabled = false;
                    EditorGUI.PropertyField(rc, prop);
                    GUI.enabled = true;
                }
            }
            else
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(rc, prop);
                GUI.enabled = true;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent ctx)
        {
            return 1f * EditorGUIUtility.singleLineHeight;
        }

        private void DrawTable(Rect P_0, SerializedProperty propVar, GUIContent ctx, UIVariableTable table)
        {
            string[] varNameList = table.GetVariableNames();
            VariableNameAttribute val = (VariableNameAttribute) this.attribute;
            Assert.IsNotNull(val);
            List<string> list = new List<string>();
            foreach (string text in varNameList)
            {
                UIVariable val2 = table.FindVariable(text);
                if (val2 != null && val.IsValid(val2.Type))
                {
                    list.Add(text);
                }
            }

            UIVariableBind obj = (UIVariableBind) propVar.serializedObject.targetObject;
            //search parent variable table
            Transform start = obj.transform.parent;
            string baseNamePath = "";
            while (start != null)
            {
                UIVariableTable parentTable = start.gameObject.GetComponentInParent<UIVariableTable>();
                if (parentTable == null)
                {
                    break;
                }

                if (parentTable == table)
                {
                    baseNamePath = parentTable.name + "/";
                    start = parentTable.transform.parent;
                    continue;
                }

                varNameList = parentTable.GetVariableNames();
                foreach (string text in varNameList)
                {
                    UIVariable val2 = parentTable.FindVariable(text);
                    if (val2 != null && val.IsValid(val2.Type))
                    {
                        //string namePath = baseNamePath + parentTable.name + "/" + text;
                        string namePath = "@" + parentTable.name + "/" + text;
                        list.Add(namePath);
                    }
                }

                baseNamePath = parentTable.name + "/";
                start = parentTable.transform.parent;
            }

            ////////////////////////////////
            Rect rect = new Rect(P_0.x, P_0.y, P_0.width * 0.39f, P_0.height);
            Rect rect2 = new Rect(P_0.x + P_0.width * 0.39f, P_0.y, P_0.width * 0.61f, P_0.height);
            EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), ctx);
            int num = list.IndexOf(propVar.stringValue);
            list.Add("None");
            int num2 = EditorGUI.Popup(rect2, num, list.ToArray());
            if (num2 != num && num2 >= 0)
            {
                if (num2 < list.Count - 1)
                {
                    propVar.stringValue = (list[num2]);
                }
                else
                {
                    propVar.stringValue = (string.Empty);
                }
            }
        }
    }
}