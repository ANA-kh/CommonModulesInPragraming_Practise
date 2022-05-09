using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ReactUI
{
    [AddComponentMenu("ReactUI/UI/Bind/UI Variable Table")]
    public sealed class UIVariableTable : MonoBehaviour
    {
		[SerializeField]
		private UIVariable[] variables;

		private Dictionary<string, UIVariable> m_varMap;


		public UIVariable[] Variables => variables;

		private Dictionary<string, UIVariable> GetVariableMap()
		{
			if (m_varMap == null)
			{
				m_varMap = new Dictionary<string, UIVariable>(StringComparer.Ordinal);
				if (variables != null)
				{
					UIVariable[] array = variables;
					foreach (UIVariable uIVariable in array)
					{
						m_varMap.Add(uIVariable.Name, uIVariable);
					}
				}
			}
			return m_varMap;
		}

		public UIVariable FindVariable(string name)
		{
            if (string.IsNullOrEmpty(name))
            {
				return null;
            }
			if (GetVariableMap().TryGetValue(name, out UIVariable value))
			{
				return value;
			}
			return null;
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

		public string[] GetVariableNames()
		{
			var keys = GetVariableMap().Keys;
			var array = new string[keys.Count];
			var idx = 0;
			foreach (var key in keys)
			{
				array[idx] = key;
				idx++;
			}
			return array;
		}

		public string[] GetOriginalVariableNames(List<UIVariableType> excludeTypes = null)
		{
			var checkExclude = excludeTypes != null && excludeTypes.Count > 0;
			var names = new List<string>();
			foreach (var var in variables)
			{
				if (checkExclude)
				{
					if (!excludeTypes.Contains(var.Type))
					{
						names.Add(var.Name);
					}
					else
					{
						names.Add($"{var.Name} (X)");
					}
				}
				else
				{
					names.Add(var.Name);
				}
			}

			return names.ToArray();
		}

		public void Sort()
		{
			Array.Sort(variables, (UIVariable P_0, UIVariable P_1) => P_0.Name.CompareTo(P_1.Name));
		}

		public void InitializeBinds()
		{
			InitVariableMap(base.transform);
		}

		public UIVariable GetVariable(int index)
		{
			return variables[index];
		}

		private static void InitVariableMap(Transform trans)
		{
			UIVariableBind[] components = trans.GetComponents<UIVariableBind>();
			UIVariableBind[] array = components;
			foreach (UIVariableBind uIVariableBind in array)
			{
				uIVariableBind.Init();
			}
			foreach (Transform item in trans)
			{
				DeepInitVariableBind(item);
			}
		}

		private static void DeepInitVariableBind(Transform trans)
		{
			if (trans.GetComponent<UIVariableTable>() == null)
			{
				UIVariableBind[] components = trans.GetComponents<UIVariableBind>();
				UIVariableBind[] array = components;
				foreach (UIVariableBind uIVariableBind in array)
				{
					uIVariableBind.Init();
				}
				foreach (Transform item in trans)
				{
					DeepInitVariableBind(item);
				}
			}
		}

		private void OnValidate()
		{
			m_varMap = null;
			if (variables != null)
			{
				UIVariable[] array = variables;
				foreach (UIVariable uIVariable in array)
				{
					//uIVariable.ResetValue();
					//uIVariable.ClearBinderList();
					uIVariable.InvokeValueChange();
				}
			}
		}

		public void ResetVarMap()
		{
			m_varMap = null;
		}

		private void Awake()
		{
            InitVariableMap(base.transform);
			m_varMap = null;
		}

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
    
    public class UIItemVariable : MonoBehaviour
    {
        public EUIItemExportType ExportType;
        [HideInInspector] [SerializeField] public string CustomExportTypeName;
        public string ExportName;

        [HideInInspector]
        public Object ExportObject;
        public void TryToAttachObject()
        {
            switch (ExportType)
            {
                case EUIItemExportType.GameObject:
                    ExportObject = this.gameObject;
                    break;
                case EUIItemExportType.Transform:
                    ExportObject = this.gameObject.transform;
                    break;
                case EUIItemExportType.Animator:
                    ExportObject = this.gameObject.GetComponent<Animator>();
                    break;
                case EUIItemExportType.CustomType:
                    ExportObject = this.gameObject.GetComponent(CustomExportTypeName);
                    break;
                case EUIItemExportType.Label:
                    ExportObject = this.gameObject.GetComponent<Text>();
                    break;
                case EUIItemExportType.Texture:
                    ExportObject = this.gameObject.GetComponent<Image>();
                    break;
                case EUIItemExportType.CanvasGroup:
                    ExportObject = this.gameObject.GetComponent<CanvasGroup>();
                    break;
                case EUIItemExportType.Button:
                    ExportObject = this.gameObject.GetComponent<Button>();
                    break;
                case EUIItemExportType.RectTransform:
                    ExportObject = this.gameObject.GetComponent<RectTransform>();
                    break;
                case EUIItemExportType.Grid:
                    ExportObject = this.gameObject.GetComponent<GridLayoutGroup>();
                    break;
                case EUIItemExportType.Toggle:
                    ExportObject = this.gameObject.GetComponent<Toggle>();
                    break;
                case EUIItemExportType.ToggleGroup:
                    ExportObject = this.gameObject.GetComponent<ToggleGroup>();
                    break;
                case EUIItemExportType.HorizontalLayoutGroup:
                    ExportObject = this.gameObject.GetComponent<HorizontalLayoutGroup>();
                    break;
                case EUIItemExportType.VerticalLayoutGroup:
                    ExportObject = this.gameObject.GetComponent<VerticalLayoutGroup>();
                    break;
                case EUIItemExportType.InputField:
                    ExportObject = this.gameObject.GetComponent<InputField>();
                    break;
                case EUIItemExportType.Scrollbar:
                    ExportObject = this.gameObject.GetComponent<Scrollbar>();
                    break;
                case EUIItemExportType.ScrollRect:
                    ExportObject = this.gameObject.GetComponent<ScrollRect>();
                    break;
                case EUIItemExportType.Slider:
                    ExportObject = this.gameObject.GetComponent<Slider>();
                    break;
                case EUIItemExportType.RawImage:
                    ExportObject = this.gameObject.GetComponent<RawImage>();
                    break;
                case EUIItemExportType.Dropdown:
                    ExportObject = this.gameObject.GetComponent<Dropdown>();
                    break;
                case EUIItemExportType.TMP_Text:
                    ExportObject = this.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                    break;
                case EUIItemExportType.TMP_InputField:
                    ExportObject = this.gameObject.GetComponent<TMPro.TMP_InputField>();
                    break;
                case EUIItemExportType.TMP_Dropdown:
                    ExportObject = this.gameObject.GetComponent<TMPro.TMP_Dropdown>();
                    break;
                // case EUIItemExportType.UISafeArea:
                //     ExportObject = this.gameObject.GetComponent<UISafeArea>();
                //    break;
                default:
                    Debug.LogError("UIItemVariable dont implement uitype > " + ExportType);
                    break;
            }

            if (ExportObject == null)
                Debug.LogWarning("UIItemVariable dont get exportobject > " + ExportType);
        }

        public string GetExportedName()
        {
            if (string.IsNullOrEmpty(ExportName))
            {
                return this.gameObject.name;
            }
            return ExportName;
        }

        //将EUIItemExportType类型映射为对应类型
        public string GetEUIItemExportTypeCorrespondOriginTypeName(EUIItemExportType euiItemExportType, UIItemVariable InItemVar)
        {
            var ret = "";
            switch (euiItemExportType)
            {
                case EUIItemExportType.GameObject:
                    ret = "GameObject";
                    break;
                case EUIItemExportType.Label:
                    ret = "Text";
                    break;
                case EUIItemExportType.Transform:
                    ret = "Transform";
                    break;
                case EUIItemExportType.Animator:
                    ret = "Animator";
                    break;
                case EUIItemExportType.CustomType: // Customized export type for variable which will ease the accessing for coding.
                    ret = InItemVar.CustomExportTypeName;
                    break;
                case EUIItemExportType.Texture:
                    ret = "Image";
                    break;
                case EUIItemExportType.CanvasGroup:
                    ret = "CanvasGroup";
                    break;
                case EUIItemExportType.Button:
                    ret = "Button";
                    break;
                case EUIItemExportType.TweenAlpha:
                    ret = "TweenAlpha";
                    break;
                case EUIItemExportType.RectTransform:
                    ret = "RectTransform";
                    break;
                case EUIItemExportType.Grid:
                    ret = "GridLayoutGroup";
                    break;
                case EUIItemExportType.Toggle:
                    ret = "Toggle";
                    break;
                case EUIItemExportType.ToggleGroup:
                    ret = "ToggleGroup";
                    break;
                case EUIItemExportType.VerticalLayoutGroup:
                    ret = "VerticalLayoutGroup";
                    break;
                case EUIItemExportType.HorizontalLayoutGroup:
                    ret = "HorizontalLayoutGroup";
                    break;
                case EUIItemExportType.InputField:
                    ret = "InputField";
                    break;
                case EUIItemExportType.Scrollbar:
                    ret = "Scrollbar";
                    break;
                case EUIItemExportType.ScrollRect:
                    ret = "ScrollRect";
                    break;
                case EUIItemExportType.UIEasyList:
                    ret = "UIEasyList";
                    break;
                case EUIItemExportType.Slider:
                    ret = "Slider";
                    break;
                case EUIItemExportType.TweenPosition:
                    ret = "TweenPosition";
                    break;
                case EUIItemExportType.UIPanel:
                    ret = "UIPanel";
                    break;
                case EUIItemExportType.UIImage:
                    ret = "UIImage";
                    break;
                case EUIItemExportType.RawImage:
                    ret = "RawImage";
                    break;
                case EUIItemExportType.Dropdown:
                    ret = "Dropdown";
                    break;
                case EUIItemExportType.ProgressBar:
                    ret = "UIProgressBar";
                    break;
                case EUIItemExportType.TMP_Text:
                    ret = "TMPro.TextMeshProUGUI";
                    break;
                case EUIItemExportType.TMP_InputField:
                    ret = "TMPro.TMP_InputField";
                    break;
                case EUIItemExportType.TMP_Dropdown:
                    ret = "TMPro.TMP_Dropdown";
                    break;
                case EUIItemExportType.UILineConnector:
                    ret = "UILineConnector";
                    break;
                case EUIItemExportType.UILineRenderer:
                    ret = "UILineRenderer";
                    break;
                case EUIItemExportType.UIDragAndScale:
                    ret = "UIDragAndScale";
                    break;
                case EUIItemExportType.UITextCountdown:
                    ret = "UITextCountdown";
                    break;
                case EUIItemExportType.UIEllipse:
                    ret = "UIEllipse";
                    break;
                case EUIItemExportType.UIToggleSwitch:
                    ret = "UIToggleSwitch";
                    break;
                case EUIItemExportType.UIToggleGroup:
                    ret = "UIToggleGroup";
                    break;
                case EUIItemExportType.DotweenAnim:
                    ret = "DG.Tweening.DOTweenAnimation";
                    break;
                case EUIItemExportType.UIEventTrigger:
                    ret = "UIEventTrigger";
                    break;
                case EUIItemExportType.RadialProgress:
                    ret = "RadialProgress";
                    break;
                case EUIItemExportType.UIAnimationCurve:
                    ret = "UIAnimationCurve";
                    break;
                case EUIItemExportType.UISafeArea:
                    ret = "UISafeArea";
                    break;
                default:
                    Debug.LogError("UIItemVariable dont implement uitype > " + ExportType.ToString());
                    break;
            }
            return ret;
        }
    }
    public enum EUIItemExportType
    {
        GameObject,
        Button,
        Label,
        InputField,
        Texture,
        Transform,
        CustomType,
        Animator,
        CanvasGroup,
        TweenAlpha,
        RectTransform,
        Grid,
        Toggle,
        ToggleGroup,
        VerticalLayoutGroup,
        HorizontalLayoutGroup,
        Scrollbar,
        ScrollRect,
        UIEasyList,
        Slider,
        TweenPosition,
        UIPanel,
        UIImage,
        RawImage,
        Dropdown,
        ProgressBar,
        TMP_Text,
        TMP_InputField,
        TMP_Dropdown,
        UILineConnector,
        UILineRenderer,
        UIDragAndScale,
        UITextCountdown,
        UIEllipse,
        UIToggleSwitch,
        UIToggleGroup,
        DotweenAnim,
        UIEventTrigger,
        RadialProgress,
        UIAnimationCurve,
        UISafeArea,
    }
}