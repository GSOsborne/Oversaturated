using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ReflectiveMPS : ModularParticleSystem, ISerializationCallbackReceiver
{
    public ParameterModifier[] modifiers;
    
    private List<ParameterAdjustor> adjustors = new List<ParameterAdjustor>();

    [System.Serializable]
    public struct ParameterModifier
    {
        public string ParamPath;
        public ParamDataType ParamType;
        public ParamDeltaType ParamDelta;
        public string[] ParamList;
        public MusicState[] StateList;

        public ParameterModifier(string paramPath, ParamDataType paramType, ParamDeltaType paramDelta, string[] paramList, MusicState[] stateList)
        {
            ParamPath = paramPath;
            ParamType = paramType;
            ParamDelta = paramDelta;
            ParamList = paramList;
            StateList = stateList;
        }
    }

    public void OnBeforeSerialize()
    {
        int adjustorCount = adjustors.Count;

        modifiers = new ParameterModifier[adjustorCount];

        for (int i = 0; i < adjustorCount; i++)
        {
            ParameterAdjustor adjustor = adjustors[i];
            string[] pList = adjustor.ParamList.ToArray();
            MusicState[] aStates = adjustor.StateList.ToArray();
            ParameterModifier modifier = new ParameterModifier(adjustor.ParamPath, adjustor.ParamType, adjustor.ParamDelta, pList, aStates);
            modifiers[i] = modifier;
        }
    }

    public void OnAfterDeserialize()
    {
        adjustors = new List<ParameterAdjustor>();
        foreach (ParameterModifier modifier in modifiers)
        {
            List<string> pList = new List<string>();
            foreach (string s in modifier.ParamList)
            {
                pList.Add(s);
            }
            List<MusicState> aStates = new List<MusicState>();
            foreach (MusicState state in modifier.StateList)
            {
                aStates.Add(state);
            }
            ParameterAdjustor adjustor = new ParameterAdjustor(modifier.ParamPath, modifier.ParamType, modifier.ParamDelta, pList, aStates);
            adjustors.Add(adjustor);
        }
    }

    private class ParameterAdjustor
    {
        public string ParamPath { get; set; }
        public ParamDataType ParamType { get; set; }
        public ParamDeltaType ParamDelta { get; set; }
        public List<string> ParamList { get; set; }
        public List<MusicState> StateList { get; set; }

        private HashSet<MusicState> activeStates;
        private bool hasConflict;
        private GetSettable paramFieldInfo;

        public ParameterAdjustor(string paramPath, ParamDataType type, ParamDeltaType delta, List<string> paramList, List<MusicState> stateList)
        {
            ParamPath = paramPath;
            ParamType = type;
            ParamDelta = delta;
            ParamList = paramList;
            StateList = stateList;

            activeStates = new HashSet<MusicState>();
            foreach (MusicState state in StateList)
            {
                activeStates.Add(state);
            }

            paramFieldInfo = null;

            Validate();
        }

        private void Validate()
        {
            if (ParamList.Count != DeltaParameterLength(ParamDelta))
            {
                Debug.Log("Invalid parameter list length " + ParamList.Count + " for delta function " + ParamDelta.ToString("G"));
                hasConflict = true;
            }
            else if (!DeltaSupportsType(ParamDelta, ParamType))
            {
                Debug.Log("Data type " + ParamType.ToString("G") + " not supported by delta function " + ParamDelta.ToString("G"));
                hasConflict = true;
            }
            else
            {
                hasConflict = false;
            }
        }

        private interface GetSettable
        {
            System.Type GetSettableType { get; }

            void SetValue<T>(T obj, object value);

            object GetValue<T>(T obj);

            object GetParentObject();

            object GetValueFromParent();

            void SetValueForParent(object value);
        }

        private class FieldWrapper : GetSettable
        {
            private FieldInfo fieldInfo;
            private object parentObject;

            public FieldWrapper(FieldInfo _fieldInfo, object _parentObject)
            {
                fieldInfo = _fieldInfo;
                parentObject = _parentObject;
            }

            public System.Type GetSettableType
            {
                get
                {
                    return fieldInfo.GetType();
                }
            }

            public void SetValue<T>(T obj, object value)
            {
                fieldInfo.SetValue(obj, value);
            }

            public object GetValue<T>(T obj)
            {
                return fieldInfo.GetValue(obj);
            }

            public object GetParentObject()
            {
                return parentObject;
            }

            public object GetValueFromParent()
            {
                return GetValue(parentObject);
            }

            public void SetValueForParent(object value)
            {
                SetValue(parentObject, value);
            }
        }

        private class PropertyWrapper : GetSettable
        {
            private PropertyInfo propertyInfo;
            private object parentObject;

            public PropertyWrapper(PropertyInfo _propertyInfo, object _parentObject)
            {
                propertyInfo = _propertyInfo;
                parentObject = _parentObject;
            }

            public System.Type GetSettableType
            {
                get
                {
                    return propertyInfo.PropertyType;
                }
            }

            public void SetValue<T>(T obj, object value)
            {
                propertyInfo.SetValue(obj, value);
            }

            public object GetValue<T>(T obj)
            {
                return propertyInfo.GetValue(obj);
            }

            public object GetParentObject()
            {
                return parentObject;
            }

            public object GetValueFromParent()
            {
                return GetValue(parentObject);
            }

            public void SetValueForParent(object value)
            {
                SetValue(parentObject, value);
            }
        }

        /// <summary>
        /// Retrieves the field info matching the specified property path on the object given
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="obj">The object to retrieve the property from</param>
        /// <param name="propertyPath">The path of the property</param>
        /// <param name="type">The Type object corresponding to the object's type</param>
        /// <returns>The property's info</returns>
        private GetSettable GetTargetProperty<T>(T obj, string propertyPath, System.Type type = null)
        {
            System.Type targetType = type ?? typeof(T);
            int pointIndex = propertyPath.IndexOf(".");
            if (pointIndex != -1)
            {
                string propertyName = propertyPath.Substring(0, pointIndex);
                GetSettable property;
                FieldInfo fieldInfo = targetType.GetField(propertyName);
                PropertyInfo propertyInfo = targetType.GetProperty(propertyName);
                if (fieldInfo != null)
                {
                    property = new FieldWrapper(fieldInfo, obj);
                }
                else if (propertyInfo != null)
                {
                    property = new PropertyWrapper(propertyInfo, obj);
                }
                else
                {
                    Debug.Log("Property " + propertyName + " not found on object type " + type.ToString());
                    return null;
                }
                var propertyObj = property.GetValue(obj);
                return GetTargetProperty(propertyObj, propertyPath.Substring(pointIndex + 1), property.GetSettableType);
            }
            else
            {
                GetSettable property;
                FieldInfo fieldInfo = targetType.GetField(propertyPath);
                PropertyInfo propertyInfo = targetType.GetProperty(propertyPath);
                if (fieldInfo != null)
                {
                    property = new FieldWrapper(fieldInfo, obj);
                }
                else if (propertyInfo != null)
                {
                    property = new PropertyWrapper(propertyInfo, obj);
                }
                else
                {
                    property = null;
                    Debug.Log("Property " + propertyPath + " not found on object type " + type.ToString());
                }
                return property;
            }
        }

        public void Apply(ParticleSystem ps, MusicState state, int measureCount)
        {
            if (!hasConflict)
            {
                if (activeStates.Contains(state))
                {
                    if (paramFieldInfo == null)
                    {
                        paramFieldInfo = GetTargetProperty(ps, ParamPath, typeof(ParticleSystem));
                    }

                    switch (ParamDelta)
                    {
                        case ParamDeltaType.Constant:
                            {
                                switch (ParamType)
                                {
                                    case ParamDataType.Bool:
                                        bool bValue = ParamList[0].ToLower() == "true";
                                        paramFieldInfo.SetValueForParent(bValue);
                                        break;
                                    case ParamDataType.Float:
                                        float fValue = float.Parse(ParamList[0]);
                                        paramFieldInfo.SetValueForParent(fValue);
                                        break;
                                    case ParamDataType.Integer:
                                        float iValue = int.Parse(ParamList[0]);
                                        paramFieldInfo.SetValueForParent(iValue);
                                        break;
                                }
                                break;
                            }

                        case ParamDeltaType.Linear:
                            {
                                switch (ParamType)
                                {
                                    case ParamDataType.Float:
                                        float fStartValue = float.Parse(ParamList[0]);
                                        float fRate = float.Parse(ParamList[1]);
                                        float fValue = fStartValue + measureCount * fRate;
                                        paramFieldInfo.SetValueForParent(fValue);
                                        break;
                                    case ParamDataType.Integer:
                                        int iStartValue = int.Parse(ParamList[0]);
                                        float iRate = float.Parse(ParamList[1]);
                                        int iValue = (int)(iStartValue + measureCount * iRate);
                                        paramFieldInfo.SetValueForParent(iValue);
                                        break;
                                }
                                break;
                            }

                        case ParamDeltaType.LinearCapped:
                            {
                                switch (ParamType)
                                {
                                    case ParamDataType.Float:
                                        float fStartValue = float.Parse(ParamList[0]);
                                        float fRate = float.Parse(ParamList[1]);
                                        float fEndValue = float.Parse(ParamList[2]);
                                        float fPotentialValue = fStartValue + measureCount * fRate;

                                        //Debug.Log(fPotentialValue);

                                        float fValue = fRate >= 0 ? Mathf.Min(fEndValue, fPotentialValue)
                                            : Mathf.Max(fEndValue, fPotentialValue);
                                        paramFieldInfo.SetValueForParent(fValue);
                                        break;
                                    case ParamDataType.Integer:
                                        int iStartValue = int.Parse(ParamList[0]);
                                        float iRate = float.Parse(ParamList[1]);
                                        int iEndValue = int.Parse(ParamList[2]);
                                        int iPotentialValue = iStartValue + (int)(measureCount * iRate);
                                        int iValue = iRate >= 0 ? System.Math.Min(iEndValue, iPotentialValue)
                                            : System.Math.Max(iEndValue, iPotentialValue);
                                        paramFieldInfo.SetValueForParent(iValue);
                                        break;
                                }
                                break;
                            }

                        case ParamDeltaType.Threshold:
                            {
                                int threshold = int.Parse(ParamList[0]);
                                switch (ParamType)
                                {
                                    case ParamDataType.Bool:
                                        bool bValue = ParamList[1].ToLower() == "true";
                                        if (measureCount >= threshold)
                                        {
                                            paramFieldInfo.SetValueForParent(bValue);
                                        }
                                        break;
                                    case ParamDataType.Float:
                                        float fValue = float.Parse(ParamList[1]);
                                        if (measureCount >= threshold)
                                        {
                                            paramFieldInfo.SetValueForParent(fValue);
                                        }
                                        break;
                                    case ParamDataType.Integer:
                                        int iValue = int.Parse(ParamList[1]);
                                        if (measureCount >= threshold)
                                        {
                                            paramFieldInfo.SetValueForParent(iValue);
                                        }
                                        break;
                                }
                                break;
                            }

                        case ParamDeltaType.InheritLinear:
                            {
                                switch (ParamType)
                                {
                                    case ParamDataType.Float:
                                        float fRate = float.Parse(ParamList[0]);
                                        float fOldValue = (float)paramFieldInfo.GetValueFromParent();
                                        float fValue = fOldValue + fRate;
                                        paramFieldInfo.SetValueForParent(fValue);
                                        break;
                                    case ParamDataType.Integer:
                                        float iRate = float.Parse(ParamList[0]);
                                        int iOldValue = (int)paramFieldInfo.GetValueFromParent();
                                        int iValue = (int)(iOldValue + iRate);
                                        paramFieldInfo.SetValueForParent(iValue);
                                        break;
                                }
                                break;
                            }

                        case ParamDeltaType.InheritLinearCapped:
                            {
                                switch (ParamType)
                                {
                                    case ParamDataType.Float:
                                        float fRate = float.Parse(ParamList[0]);
                                        float fEndValue = float.Parse(ParamList[1]);
                                        float fOldValue = (float)paramFieldInfo.GetValueFromParent();
                                        float fPotentialValue = fOldValue + fRate;

                                        //Debug.Log(fPotentialValue);

                                        float fValue = fRate >= 0 ? Mathf.Min(fEndValue, fPotentialValue)
                                            : Mathf.Max(fEndValue, fPotentialValue);
                                        paramFieldInfo.SetValueForParent(fValue);
                                        break;
                                    case ParamDataType.Integer:
                                        float iRate = float.Parse(ParamList[0]);
                                        int iEndValue = int.Parse(ParamList[1]);
                                        int iOldValue = (int)paramFieldInfo.GetValueFromParent();
                                        int iPotentialValue = (int)(iOldValue + iRate);
                                        int iValue = iRate >= 0 ? System.Math.Min(iEndValue, iPotentialValue)
                                            : System.Math.Max(iEndValue, iPotentialValue);
                                        paramFieldInfo.SetValueForParent(iValue);
                                        break;
                                }
                                break;
                            }

                    }
                }
            }
        }
    }

    public enum ParamDataType
    {
        Integer, Float, Bool
    }

    // When adding new delta type, update DeltaSupportsType with all supported data types 
    // and DeltaParameterLength with parameter count
    public enum ParamDeltaType
    {
        Constant, Linear, LinearCapped, Threshold, InheritLinear, InheritLinearCapped
    }

    private static bool DeltaSupportsType(ParamDeltaType delta, ParamDataType type)
    {
        switch (delta)
        {
            case ParamDeltaType.Constant:
                return type == ParamDataType.Bool || type == ParamDataType.Float || type == ParamDataType.Integer;
            case ParamDeltaType.Linear:
                return type == ParamDataType.Integer || type == ParamDataType.Float;
            case ParamDeltaType.LinearCapped:
                return type == ParamDataType.Integer || type == ParamDataType.Float;
            case ParamDeltaType.Threshold:
                return type == ParamDataType.Bool || type == ParamDataType.Integer || type == ParamDataType.Float;
            case ParamDeltaType.InheritLinear:
                return type == ParamDataType.Integer || type == ParamDataType.Float;
            case ParamDeltaType.InheritLinearCapped:
                return type == ParamDataType.Integer || type == ParamDataType.Float;
            default:
                return false;
        }
    }

    private static int DeltaParameterLength(ParamDeltaType delta)
    {
        switch (delta)
        {
            case ParamDeltaType.Constant:
                return 1;
            case ParamDeltaType.Linear:
                // Two delta params: starting value and rate of change per measure
                return 2;
            case ParamDeltaType.LinearCapped:
                // Three delta params: starting value, rate of change per measure, and ending value
                return 3;
            case ParamDeltaType.Threshold:
                // Two delta params: measure count at which change activates and new value after threshold
                return 2;
            case ParamDeltaType.InheritLinear:
                // One delta param: rate of change per measure
                return 1;
            case ParamDeltaType.InheritLinearCapped:
                // Two delta params: rate of change per measure and ending value
                return 2;
            default:
                return 0;
        }
    }

    protected override void Initialize()
    {
        
    }

    protected override void OnMeasureTransition(MusicState oldState, MusicState newState)
    {
        return;
    }

    protected override void UpdateForNewMeasure()
    {
        if (targetParticleSystem == null)
        {
            targetParticleSystem = GetComponent<ParticleSystem>();
        }

        foreach (ParameterAdjustor adjustor in adjustors)
        {
            adjustor.Apply(targetParticleSystem, latestState, measuresInState);
        }
    }
}
