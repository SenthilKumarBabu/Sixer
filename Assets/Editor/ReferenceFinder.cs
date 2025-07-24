using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ReferenceFinder : EditorWindow
{

    public GameObject target;
    public MonoScript script;
    //bool isGameObject = true;
    SerializedProperty property/*, scriptProperty*/;
    SerializedObject so;

    private void OnEnable()
    {
        so = new SerializedObject(this);
        property = so.FindProperty("target");
        //scriptProperty = so.FindProperty("script");
    }

    [MenuItem("Custom Tools/Misc/Find GameObject Reference")]
    public static void Test()
    {
        GetWindow<ReferenceFinder>().ShowUtility();
    }

    private void OnGUI()
    {

        //isGameObject = EditorGUILayout.Toggle("Is GameObject", isGameObject);
        EditorGUI.BeginChangeCheck();
        //if (isGameObject)
            EditorGUILayout.PropertyField(property);
        //else
        //    EditorGUILayout.PropertyField(scriptProperty);
        if(EditorGUI.EndChangeCheck())
        {
            so.ApplyModifiedProperties();
        }
        if (GUILayout.Button("Find References"))
            Find();
    }

    void Find()
    {
        var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
        List<Component> refs = new List<Component>();
        foreach (var obj in rootObjs)
        {
            refs.AddRange(RecursiveFind(obj));
        }
        if (refs.Count == 1)
            Selection.activeGameObject = refs[0].gameObject;
        Debug.LogError(refs != null ? $"Total Reference Count: {refs.Count.ToString()}" : "");
    }
    List<Component> RecursiveFind(GameObject root)
    {
        Component[] curRefs;
        List<Component> refs = new List<Component>();
        //if (isGameObject)
            FindGORef(root, out curRefs);
        //else
        //    FindRef(root, out curRefs);
        if (curRefs != null && curRefs.Length > 0)
            refs.AddRange(curRefs);

        foreach (Transform trans in root.transform)
        {
            refs.AddRange(RecursiveFind(trans.gameObject));
        }
        return refs;
    }

    static string TrimEnd(string a, string b)
    {
        return a.Substring(0, a.IndexOf(b));
    }

    Type GetType(string type)
    {
        return Type.GetType(Assembly.CreateQualifiedName(TrimEnd(Assembly.GetExecutingAssembly().ToString(), "-Editor"), type));
    }

    //void FindRef(GameObject go, out MonoBehaviour[] refTargets)
    //{
    //    refTargets = null;
    //    List<MonoBehaviour> result = new List<MonoBehaviour>();
    //    foreach (Component comp in go.GetComponents<MonoBehaviour>())
    //    {
    //        if (comp is Button)
    //        {
    //            var button = comp as Button;
    //            var onclick = button.onClick;
    //            for (int i = 0; i < onclick.GetPersistentEventCount(); i++)
    //            {
    //                if (onclick.GetPersistentTarget(i).GetType().Equals(script.GetClass()))
    //                {
    //                    Debug.LogError("Found " + comp);
    //                    result.Add(comp as MonoBehaviour);
    //                }
    //            }
    //            continue;
    //        }
    //        else if (comp is Toggle)
    //        {
    //            var toggle = comp as Toggle;
    //            var onChange = toggle.onValueChanged;
    //            for (int i = 0; i < onChange.GetPersistentEventCount(); i++)
    //            {
    //                if (onChange.GetPersistentTarget(i).GetType().Equals(script.GetClass()))
    //                {
    //                    Debug.LogError("Found " + comp);
    //                    result.Add(comp as MonoBehaviour);
    //                }
    //            }
    //            continue;
    //        }
    //        else if (comp is Slider)
    //        {
    //            var slider = comp as Slider;
    //            var onChange = slider.onValueChanged;
    //            for (int i = 0; i < onChange.GetPersistentEventCount(); i++)
    //            {
    //                if (onChange.GetPersistentTarget(i).GetType().Equals(script.GetClass()))
    //                {
    //                    Debug.LogError("Found " + comp);
    //                    result.Add(comp as MonoBehaviour);
    //                }
    //            }
    //            continue;
    //        }
    //        else if (comp is InputField)
    //        {
    //            var inputField = comp as InputField;
    //            var onChange = inputField.onValueChanged;
    //            var onEnd = inputField.onEndEdit;
    //            for (int i = 0; i < onChange.GetPersistentEventCount(); i++)
    //            {
    //                if (onChange.GetPersistentTarget(i).GetType().Equals(script.GetClass()))
    //                {
    //                    Debug.LogError("Found " + comp);
    //                    result.Add(comp as MonoBehaviour);
    //                }
    //            }
    //            for (int i = 0; i < onEnd.GetPersistentEventCount(); i++)
    //            {
    //                if (onEnd.GetPersistentTarget(i).GetType().Equals(script.GetClass()))
    //                {
    //                    Debug.LogError("Found " + comp);
    //                    result.Add(comp as MonoBehaviour);
    //                }
    //            }
    //            continue;
    //        }

    //        Type type = comp.GetType();
    //        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
    //        foreach (var field in fields)
    //        {
    //            var attrs = field.GetCustomAttributes(true);
    //            var flag = false;
    //            foreach (var attr in attrs)
    //            {
    //                if (attr.GetType().Equals(typeof(SerializeField)))
    //                {
    //                    flag = true;
    //                    break;
    //                }
    //            }
    //            if (flag || field.IsPublic)
    //            {
    //                if (field.FieldType.Equals(script.GetClass()))
    //                {
    //                    Debug.LogError("Found " + comp);
    //                    result.Add(comp as MonoBehaviour);
    //                }

    //            }
    //        }
    //    }
    //    refTargets = result.ToArray();
    //}

    void FindGORef(GameObject go, out Component[] refTargets)
    {
        refTargets = null;
        List<Component> result = new List<Component>();
        foreach (Component comp in go.GetComponents<Component>())
        {
            if (comp == null)
                continue;
            if (comp is Button)
            {
                var button = comp as Button;
                var onclick = button.onClick;
                for (int i = 0; i < onclick.GetPersistentEventCount(); i++)
                {
                    var persistentTarget = onclick.GetPersistentTarget(i);
                    CheckForExists(persistentTarget, result, comp);
                }
                continue;
            }
            else if (comp is Toggle)
            {
                var toggle = comp as Toggle;
                var onChange = toggle.onValueChanged;
                for (int i = 0; i < onChange.GetPersistentEventCount(); i++)
                {
                    var persistentTarget = onChange.GetPersistentTarget(i);
                    CheckForExists(persistentTarget, result, comp);
                }
                continue;
            }
            else if (comp is Slider)
            {
                var slider = comp as Slider;
                var onChange = slider.onValueChanged;
                for (int i = 0; i < onChange.GetPersistentEventCount(); i++)
                {
                    var persistentTarget = onChange.GetPersistentTarget(i);
                    CheckForExists(persistentTarget, result, comp);
                }
                continue;
            }
            else if (comp is InputField)
            {
                var inputField = comp as InputField;
                var onChange = inputField.onValueChanged;
                var onEnd = inputField.onEndEdit;
                for (int i = 0; i < onChange.GetPersistentEventCount(); i++)
                {
                    var persistentTarget = onChange.GetPersistentTarget(i);
                    CheckForExists(persistentTarget, result, comp);
                }
                for (int i = 0; i < onEnd.GetPersistentEventCount(); i++)
                {
                    var persistentTarget = onEnd.GetPersistentTarget(i);
                    CheckForExists(persistentTarget, result, comp);
                }
                continue;
            }

            Type type = comp.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i=0; i < fields.Length;i++)
            {
                if (fields[i].GetValue(comp) is null)
                    continue;
                var attrs = fields[i].GetCustomAttributes(true);
                var flag = false;
                foreach (var attr in attrs)
                {
                    if (attr.GetType().Equals(typeof(SerializeField)))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag || fields[i].IsPublic)
                {
                    if (fields[i].FieldType.IsArray)
                    {
                        if (ProcessArray(fields[i], comp))
                        {
                            Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                            result.Add(comp);
                        }
                    }
                    else if (fields[i].GetValue(comp) is IList)
                    {
                        if (ProcessList(fields[i], comp))
                        {
                            Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                            result.Add(comp);
                        }
                    }
                    else
                    {

                        var val = fields[i].GetValue(comp);
                        if (fields[i].FieldType.Equals(typeof(GameObject)) && (val as GameObject).Equals(target))
                        {
                            Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                            result.Add(comp);
                        }
                        else if (val is Component)
                        {
                            var compval = val as Component;
                            if (compval == null)
                                continue;
                            if ((val as Component).gameObject.Equals(target))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                                result.Add(comp);
                            }
                        }
                    }
                }
            }
        }
        refTargets = result.ToArray();
    }

    void CheckForExists(UnityEngine.Object obj, List<Component> result, Component comp)
    {
        if (obj is GameObject)
        {
            if (obj == target)
            {
                Debug.LogError($"Component: {comp.gameObject}", comp.gameObject);
                result.Add(comp);
                return;
            }
        }
        var persistentComponent = obj as Component;
        if (persistentComponent == null)
            return;
        if (persistentComponent.gameObject == target)
        {
            Debug.LogError($"Component: {comp.gameObject}", comp.gameObject);
            result.Add(comp);
            return;
        }
        return;
    }

    bool ProcessList(FieldInfo fieldInfo, Component comp)
    {
        if (fieldInfo.FieldType.GetGenericArguments().Single().IsValueType)
            return false;
        IList objs = fieldInfo.GetValue(comp) as IList;
        if (objs == null || objs.Count == 0)
            return false;
        foreach(var obj in objs)
        {
            if (obj == null)
                continue;
            if(obj is Component)
            {
                var component = obj as Component;
                if (component == null)
                    continue;
                if (component.gameObject.Equals(target))
                    return true;
            }
            else if(obj is GameObject)
            {
                var gameObject = obj as GameObject;
                if (gameObject == null)
                    continue;
                if (gameObject.Equals(target))
                    return true;  
            }
            else
            {
                Type type = obj.GetType();
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].GetValue(obj) is null)
                        continue;
                    var attrs = fields[i].GetCustomAttributes(true);
                    var flag = false;
                    foreach (var attr in attrs)
                    {
                        if (attr.GetType().Equals(typeof(SerializeField)))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag || fields[i].IsPublic)
                    {
                        if (fields[i].FieldType.IsArray)
                        {
                            if (ProcessArray(fields[i], obj))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                                return true;
                            }
                        }
                        else if (fields[i].GetValue(obj) is IList)
                        {
                            if (ProcessList(fields[i], obj))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                                return true;
                            }
                        }
                        else
                        {

                            var val = fields[i].GetValue(obj);
                            if (fields[i].FieldType.Equals(typeof(GameObject)) && (val as GameObject).Equals(target))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                                return true;
                            }
                            else if (val is Component)
                            {
                                var compval = val as Component;
                                if (compval == null)
                                    continue;
                                if ((val as Component).gameObject.Equals(target))
                                {
                                    Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    bool ProcessArray(FieldInfo fieldInfo, Component comp)
    {
        if (fieldInfo.FieldType.GetElementType().IsValueType)
            return false;
        object[] objs = fieldInfo.GetValue(comp) as object[];
        if (objs == null || objs.Length == 0)
            return false;
        foreach (var obj in objs)
        {
            if (obj == null)
                continue;
            if (obj is Component)
            {
                var component = obj as Component;
                if (component == null)
                    continue;
                if (component.gameObject.Equals(target))
                    return true;
            }
            else if (obj is GameObject)
            {
                var gameObject = obj as GameObject;
                if (gameObject == null)
                    continue;
                if (gameObject.Equals(target))
                    return true;
            }
            else
            {
                Type type = obj.GetType();
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].GetValue(obj) is null)
                        continue;
                    var attrs = fields[i].GetCustomAttributes(true);
                    var flag = false;
                    foreach (var attr in attrs)
                    {
                        if (attr.GetType().Equals(typeof(SerializeField)))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag || fields[i].IsPublic)
                    {
                        if (fields[i].FieldType.IsArray)
                        {
                            if (ProcessArray(fields[i], obj))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                                return true;
                            }
                        }
                        else if (fields[i].GetValue(obj) is IList)
                        {
                            if (ProcessList(fields[i], obj))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                                return true;
                            }
                        }
                        else
                        {

                            var val = fields[i].GetValue(obj);
                            if (fields[i].FieldType.Equals(typeof(GameObject)) && (val as GameObject).Equals(target))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                                return true;
                            }
                            else if (val is Component)
                            {
                                var compval = val as Component;
                                if (compval == null)
                                    continue;
                                if ((val as Component).gameObject.Equals(target))
                                {
                                    Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", comp.gameObject);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    bool ProcessList(FieldInfo fieldInfo, object comp)
    {
        if (fieldInfo.FieldType.GetGenericArguments().Single().IsValueType)
            return false;
        IList objs = fieldInfo.GetValue(comp) as IList;
        if (objs == null || objs.Count == 0)
            return false;
        foreach (var obj in objs)
        {
            if (obj == null)
                continue;
            if (obj is Component)
            {
                var component = obj as Component;
                if (component == null)
                    continue;
                if (component.gameObject.Equals(target))
                    return true;
            }
            else if (obj is GameObject)
            {
                var gameObject = obj as GameObject;
                if (gameObject == null)
                    continue;
                if (gameObject.Equals(target))
                    return true;
            }
            else
            {
                Type type = obj.GetType();
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].GetValue(obj) is null)
                        continue;
                    var attrs = fields[i].GetCustomAttributes(true);
                    var flag = false;
                    foreach (var attr in attrs)
                    {
                        if (attr.GetType().Equals(typeof(SerializeField)))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag || fields[i].IsPublic)
                    {
                        if (fields[i].FieldType.IsArray)
                        {
                            if (ProcessArray(fields[i], obj))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}");
                                return true;
                            }
                        }
                        else if (fields[i].GetValue(obj) is IList)
                        {
                            if (ProcessList(fields[i], obj))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}");
                                return true;
                            }
                        }
                        else
                        {

                            var val = fields[i].GetValue(obj);
                            if (fields[i].FieldType.Equals(typeof(GameObject)) && (val as GameObject).Equals(target))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", val as GameObject);
                                return true;
                            }
                            else if (val is Component)
                            {
                                var compval = val as Component;
                                if (compval == null)
                                    continue;
                                if ((val as Component).gameObject.Equals(target))
                                {
                                    Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", val as Component);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    bool ProcessArray(FieldInfo fieldInfo, object comp)
    {
        if (fieldInfo.FieldType.GetElementType().IsValueType)
            return false;
        object[] objs = fieldInfo.GetValue(comp) as object[];
        if (objs == null || objs.Length == 0)
            return false;
        foreach (var obj in objs)
        {
            if (obj == null)
                continue;
            if (obj is Component)
            {
                var component = obj as Component;
                if (component == null)
                    continue;
                if (component.gameObject.Equals(target))
                    return true;
            }
            else if (obj is GameObject)
            {
                var gameObject = obj as GameObject;
                if (gameObject == null)
                    continue;
                if (gameObject.Equals(target))
                    return true;
            }
            else
            {
                Type type = obj.GetType();
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].GetValue(obj) is null)
                        continue;
                    var attrs = fields[i].GetCustomAttributes(true);
                    var flag = false;
                    foreach (var attr in attrs)
                    {
                        if (attr.GetType().Equals(typeof(SerializeField)))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag || fields[i].IsPublic)
                    {
                        if (fields[i].FieldType.IsArray)
                        {
                            if (ProcessArray(fields[i], obj))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}");
                                return true;
                            }
                        }
                        else if (fields[i].GetValue(obj) is IList)
                        {
                            if (ProcessList(fields[i], obj))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}");
                                return true;
                            }
                        }
                        else
                        {

                            var val = fields[i].GetValue(obj);
                            if (fields[i].FieldType.Equals(typeof(GameObject)) && (val as GameObject).Equals(target))
                            {
                                Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", val as GameObject);
                                return true;
                            }
                            else if (val is Component)
                            {
                                var compval = val as Component;
                                if (compval == null)
                                    continue;
                                if ((val as Component).gameObject.Equals(target))
                                {
                                    Debug.LogError($"Component: {comp}\nField: {fields[i].Name}", val as Component);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

}
