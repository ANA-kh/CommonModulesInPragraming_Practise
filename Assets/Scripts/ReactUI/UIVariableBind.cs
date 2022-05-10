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

        public UIVariable FindVariable(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            string realName = name;

            if (VariableTable != null)
            {
                return VariableTable.FindVariable(realName);
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
            PrefabType prefabType = PrefabUtility.GetPrefabType(gameObject);
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