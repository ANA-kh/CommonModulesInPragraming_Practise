using System;
using System.Collections.Generic;
using System.IO;
using ReactUI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(UIVariableTable))]
public class UIVariableTableEditor : Editor
{
   private SerializedProperty m_variables;

	private ReorderableList m_list;

	//private Dictionary<int, SerializedProperty> m_propMap = new Dictionary<int, SerializedProperty>();
	
	private HashSet<int> m_nameset = new HashSet<int>();
	private Dictionary<string, int> m_name2idx = new Dictionary<string, int>(StringComparer.Ordinal);
	

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		m_list.DoLayoutList();
		if (serializedObject.ApplyModifiedProperties())
		{
			Init();
		}
	}

	private void OnEnable()
	{
		if (!(target == null))
		{
			m_variables = serializedObject.FindProperty("variables");
			m_list = (ReorderableList)(object)new ReorderableList(serializedObject, m_variables);
			m_list.drawHeaderCallback = delegate(Rect P_0)
			{
				GUI.Label(P_0, "Variables:");
			};
			m_list.elementHeightCallback = (ReorderableList.ElementHeightCallbackDelegate)(object)(ReorderableList.ElementHeightCallbackDelegate)((int P_0) => GetHeight(m_variables, P_0));
			m_list.drawElementCallback = delegate(Rect P_0, int P_1, bool P_2, bool P_3)
			{
				DrawOneVariable(m_variables, P_0, P_1, P_2, P_3);
			};
			m_list.onAddCallback = delegate
			{
				UIVariableTable val = (UIVariableTable)(object)(UIVariableTable)target;
				val.AddDefaultVariable();
				EditorUtility.SetDirty(target);
			};
			Init();
		}
	}

	private float GetHeight(SerializedProperty P_0, int P_1)
	{
		var value = P_0.GetArrayElementAtIndex(P_1);
		if (!value.isExpanded)
		{
			return 2f * EditorGUIUtility.singleLineHeight;
		}
		UIVariableTable val = (UIVariableTable)target;
		UIVariable variable = val.GetVariable(P_1);
		ICollection<UIVariableBind> binds = variable.Binds;
		return (float)(2 + binds.Count) * EditorGUIUtility.singleLineHeight;
	}

	private void DrawOneVariable(SerializedProperty P_0, Rect P_1, int P_2, bool P_3, bool P_4)
	{
		var value = P_0.GetArrayElementAtIndex(P_2);
		bool flag = m_nameset.Contains(P_2);
		Color color = GUI.color;
		if (flag)
		{
			GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
		}
		SerializedProperty val = value.FindPropertyRelative("name");
		SerializedProperty val2 = value.FindPropertyRelative("type");
		Rect rect = new Rect(P_1.x + 8f, P_1.y, 16f, EditorGUIUtility.singleLineHeight);
		Rect rect2 = new Rect(P_1.x + 12f, P_1.y, (P_1.width - 12f) / 2f - 5f, EditorGUIUtility.singleLineHeight);
		Rect rect3 = new Rect(P_1.x + P_1.width / 2f + 5f, P_1.y, (P_1.width - 12f) / 2f - 5f, EditorGUIUtility.singleLineHeight);
		value.isExpanded=(EditorGUI.Foldout(rect, value.isExpanded, GUIContent.none));
		EditorGUI.PropertyField(rect2, val, GUIContent.none);
		EditorGUI.PropertyField(rect3, val2, GUIContent.none);
		Rect rect4 = new Rect(P_1.x + 12f, P_1.y + EditorGUIUtility.singleLineHeight, P_1.width - 12f, EditorGUIUtility.singleLineHeight);
		SerializedProperty val3 = null;
		UIVariableType val4 = (UIVariableType)val2.enumValueIndex;
		switch ((int)val4)
		{
		case 0:
			val3 = value.FindPropertyRelative("booleanValue");
			val3.boolValue=(EditorGUI.ToggleLeft(rect4, GUIContent.none, val3.boolValue));
			break;
		case 1:
			val3 = value.FindPropertyRelative("integerValue");
			val3.intValue=(EditorGUI.IntField(rect4, GUIContent.none, val3.intValue));
			break;
		case 2:
			val3 = value.FindPropertyRelative("floatValue");
			val3.floatValue=(EditorGUI.FloatField(rect4, GUIContent.none, val3.floatValue));
			break;
		case 3:
			val3 = value.FindPropertyRelative("stringValue");
			val3.stringValue=(EditorGUI.TextField(rect4, GUIContent.none, val3.stringValue));
			break;
		}
		if (value.isExpanded)
		{
			UIVariableTable val5 = (UIVariableTable)target;
			UIVariable variable = val5.GetVariable(P_2);
			ICollection<UIVariableBind> binds = variable.Binds;
			if (binds.Count > 0)
			{
				GUI.enabled = false;
				Rect rect5 = new Rect(P_1.x + 12f, P_1.y + EditorGUIUtility.singleLineHeight, P_1.width - 12f, EditorGUIUtility.singleLineHeight);
				foreach (UIVariableBind item in binds)
				{
					rect5.y += EditorGUIUtility.singleLineHeight;
					if ((UnityEngine.Object)(object)item != null)
					{
						EditorGUI.ObjectField(rect5, (UnityEngine.Object)(object)item, ((object)item).GetType(), true);
					}
					else
					{
						EditorGUI.ObjectField(rect5, (UnityEngine.Object)null, typeof(UnityEngine.Object), true);
					}
				}
				GUI.enabled = true;
			}
		}
		GUI.color = color;
	}

	private void Init()
	{
		m_nameset.Clear();
		m_name2idx.Clear();
		for (int i = 0; i < m_variables.arraySize; i++)
		{
			SerializedProperty arrayElementAtIndex = m_variables.GetArrayElementAtIndex(i);
			SerializedProperty val = arrayElementAtIndex.FindPropertyRelative("name");
			if (m_name2idx.ContainsKey(val.stringValue))
			{
				m_nameset.Add(m_name2idx[val.stringValue]);
				m_nameset.Add(i);
			}
			else
			{
				m_name2idx.Add(val.stringValue, i);
			}
		}
	}
}


