using UnityEditor;
using UnityEngine;

namespace ReactUI
{
    [ExecuteInEditMode]
    public abstract class UIVariableBind : MonoBehaviour
    {
        [Tooltip("The variable table for this bind.")] [SerializeField]
        private UIVariableTable variableTable;

        private bool isInited;


        private UIVariableTable bindTable;

        public UIVariableTable VariableTable { get; private set; }

        internal virtual void Init()
        {
            if (!isInited)
            {
                isInited = true;
                FindVarTable();
                BindVariables();
            }
        }

        static string _markCustomParentVariableTable = "@";

        public UIVariable FindVariable(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            string realName = name;
            UIVariableTable vt = VariableTable;

            if (name.StartsWith(_markCustomParentVariableTable))
            {
                int pos = name.IndexOf('/');
                if (pos >= 0)
                {
                    string tableName = name.Substring(1, pos - 1);
                    realName = name.Substring(pos + 1);
                    vt = FindCustomParentTable(tableName);
                }
            }

            if (vt != null)
            {
                return vt.FindVariable(realName);
            }

            return null;
        }

        UIVariableTable FindCustomParentTable(string name)
        {
            Transform t = this.transform;
            while (t != null)
            {
                if (t.name == name)
                {
                    UIVariableTable table = t.GetComponent<UIVariableTable>();
                    if (table != null)
                    {
                        return table;
                    }
                }

                t = t.parent;
            }

            return null;
        }

        protected virtual void OnDestroy()
        {
            UnbindVariables();
            isInited = false;
        }

        protected virtual void BindVariables()
        {
        }

        protected virtual void UnbindVariables()
        {
        }

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            PrefabType prefabType = PrefabUtility.GetPrefabType((Object) base.gameObject);
            if ((int) prefabType != 1)
            {
                UnbindVariables();
                isInited = true;
                FindVarTable();
                BindVariables();
            }
            else if (variableTable == null)
            {
                variableTable = GetComponentInParent<UIVariableTable>();
            }
#else
			if (variableTable == null)
			{
				variableTable = GetComponentInParent<UIVariableTable>();
			}
#endif
        }

        private void FindVarTable()
        {
            if (variableTable == null)
            {
                variableTable = GetComponentInParent<UIVariableTable>();
            }

            VariableTable = variableTable;
        }
    }
}