using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReactUI
{
    [AddComponentMenu("ReactUI/UI/Bind/Variable Bind Active")]
    public sealed class UIVariableBindActive : UIVariableBindBool
    {
        public enum TransitionModeEnum
        {
            Instant,
            Fade
        }

        [SerializeField] private TransitionModeEnum transitionMode;

        [SerializeField] private float transitionTime = 0.1f;
        
        protected override void OnValueChanged()
        {
            bool result = GetResult();
            if (transitionMode == TransitionModeEnum.Instant)
            {
                gameObject.SetActive(result);
                return;
            }

            //TODO 添加渐变模式  DOTween
        }
    }
}