using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Helper
{
    public static class ExtensionHelper
    {
        public static string ArrayToString(string[] array)
        {
            string result = "";
            foreach (string s in array)
            {
                result += s + " ";
            }
            return result.Trim();


        }

        public static string ArrayString(this string[] array)
        {
            string result = "";
            foreach (string s in array)
            {
                result += s + " ";
            }
            return result.Trim();
        }
    }

    public static class ContextHelper
    {
        static Dictionary<object, Dictionary<string, object>> VariableContext = new Dictionary<object, Dictionary<string, object>>();
        static Dictionary<object, Dictionary<string, object>> ObjStates = new Dictionary<object, Dictionary<string, object>>();

        public static void SaveToContext(string name, object instance)
        {
            FieldInfo field = instance.GetType().GetField(name);
            if (VariableContext.ContainsKey(instance.GetType()))
            {
                // if both class and field exist in variable context
                if (VariableContext[instance.GetType()].ContainsKey(name))
                {
                    VariableContext[instance.GetType()][name] = field.GetValue(instance);
                }
                else
                {
                    VariableContext[instance.GetType()].Add(name, field.GetValue(instance));
                }
            }
            else
            {
                VariableContext.Add(instance.GetType(), new Dictionary<string, object>());
                VariableContext[instance.GetType()].Add(field.Name, field.GetValue(instance));
            }
        }

        public static object GetSavedVal(string name, object instance)
        {
            
            if (VariableContext.ContainsKey(instance.GetType()))
            {
                // if both class and field exist in variable context
                if (VariableContext[instance.GetType()].ContainsKey(name))
                {
                    return VariableContext[instance.GetType()][name];
                }
                
            }

            return null;
        }

        public static void SaveObjState(this object obj)
        {
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (ObjStates.ContainsKey(obj.GetType()))
            {
                foreach (FieldInfo field in fields)
                {
                    // if both class and field exist in variable context
                    if (ObjStates[obj.GetType()].ContainsKey(field.Name))
                    {
                        ObjStates[obj.GetType()][field.Name] = field.GetValue(obj);
                    }
                    else
                    {
                        ObjStates[obj.GetType()].Add(field.Name, field.GetValue(obj));
                    }
                }
            }
            else
            {
                ObjStates.Add(obj.GetType(), new Dictionary<string, object>());
                foreach (FieldInfo field in fields)
                {
                    ObjStates[obj.GetType()].Add(field.Name, field.GetValue(obj));
                }
            }
        }
    }

    public static class JsonHelper
    {
        public static object ReadFile(object dummyObj, string fileName)
        {
            return JsonConvert.DeserializeObject<object>(File.ReadAllText(fileName));
        }

        public static void SaveFile(object dummyObj, string fileName )
        {
            string json = JsonConvert.SerializeObject(dummyObj);
            File.WriteAllText(fileName, json);
        }
    }
}
