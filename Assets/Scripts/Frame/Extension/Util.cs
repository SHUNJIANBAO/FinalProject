using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Util
{
    public static Type GetType(string typeName)
    {
        return Type.GetType(typeName);
    }

    /// <summary>
    /// 查找子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static T GetChild<T>(GameObject go, string childName) where T : UnityEngine.Object
    {
        Transform trans = go.transform.Find(childName);
        if (trans == null) return null;
        if (typeof(T) == typeof(GameObject))
        {
            return trans.gameObject as T;
        }
        if (typeof(T) == typeof(Transform)) return trans as T;
        else
        {
            return trans.GetComponent<T>();
        }
    }

    /// <summary>
    /// 将组件按Y轴高低排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    public static void SortForPositionY<T>(ref T[] array) where T : UnityEngine.Component
    {
        T tempValue = null;
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (array[i].transform.position.y < array[j].transform.position.y)
                {
                    tempValue = array[i];
                    array[i] = array[j];
                    array[j] = tempValue;
                }
            }
        }
    }

    /// <summary>
    /// 将值转化成指定类型
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetValue(object obj, System.Type type,string propertyName)
    {
        string value = obj.ToString().Trim();
        if (string.IsNullOrEmpty(value))
        {
            if (type == typeof(int) || type == typeof(float) || type == typeof(double) ||
                type == typeof(uint) || type == typeof(byte) || type == typeof(sbyte) ||
                type == typeof(short) || type == typeof(long) || type == typeof(Int32) ||
                type == typeof(Int64))
            {
                return 0;
            }
            if (type == typeof(bool))
            {
                return false;
            }
        }
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            string[] tempArry = value.Split('|', ';', ':', ',');
            var memberType = type.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(new Type[] { memberType });
            var list = Activator.CreateInstance(listType, new object[] { });

            //List<string> tempList = new List<string>();
            for (int i = 0; i < tempArry.Length; i++)
            {
                object addItem = GetValue(tempArry[i], memberType, "");
                list.GetType().InvokeMember("Add", BindingFlags.Default | BindingFlags.InvokeMethod, null, list, new object[] { addItem });
                //tempList.Add(tempArry[i]);
            }
            //return tempList;
            return list;
        }
        if (type == typeof(bool))
        {
            float temp = float.Parse(value);
            if (temp != 0) return true;
            return false;
        }
        else if (type.BaseType == typeof(Enum))
            return GetValue(value, Enum.GetUnderlyingType(type),"");
        if (type == typeof(Vector3))
        {
            string[] tempArry = value.Split('|', ';', ':', ',');
            Vector3 temp;
            if (tempArry.Length < 3)
            {
                temp = Vector3.zero;
            }
            else
            {
                temp = new Vector3(float.Parse(tempArry[0]), float.Parse(tempArry[1]), float.Parse(tempArry[2]));
            }
            return temp;
        }
        if (type == typeof(Vector2))
        {
            string[] tempArry = value.Split('|', ';', ':', ',');
            Vector2 temp;
            if (tempArry.Length < 3)
            {
                temp = Vector2.zero;
            }
            else
            {
                temp = new Vector2(float.Parse(tempArry[0]), float.Parse(tempArry[1]));
            }
            return temp;
        }
        return System.Convert.ChangeType(value, type);
    }

    public static Type[] GetTypes<T>()
    {
        var ass = typeof(T).Assembly;
        var types = ass.GetTypes();
        List<Type> tempList = new List<Type>();
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i].IsSubclassOf(typeof(T)))
            {
                tempList.Add(types[i]);
            }
        }
        return tempList.ToArray();
    }

    /// <summary>
    /// 判断字符能不能转成int类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public static bool IsNumber(string value, out int num)
    {
        bool result = int.TryParse(value, out num);
        return result;
    }
}
