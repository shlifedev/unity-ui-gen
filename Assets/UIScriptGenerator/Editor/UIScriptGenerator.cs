using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace Hamster.Unity.Utility
{
    public class UIScriptGenerator
    { 
        public static List<string> reflectionFields = new List<string>();
        public static string CompleteCode = null;
        private static List<Transform> GetChilds(Transform target)
        {
            List < Transform >transformList = new List<Transform>();
            for (int i = 0; i < target.transform.childCount; i++)
            {
                var child = target.transform.GetChild(i);
                transformList.Add(child);
            }
            return transformList;
        }
        private static void DeepSearch(Transform target, List<Transform> makedList)
        {
            List<Transform> childs = GetChilds(target);
            if (childs.Count == 0)
            {
                return;
            }
            else
            {

                for (int i = 0; i < childs.Count; i++)
                {
                    makedList.Add(childs[i]);
                    DeepSearch(childs[i], makedList);
                }
            }

        }

        private static List<List<Transform>> GetObjectChilds(Transform transform)
        {
            return null;
        }

        public static List<Transform> Search(Transform target)
        {
            var transformList = new List<Transform>();
            DeepSearch(target, transformList);
            return transformList;
        }


        [MenuItem("GameObject/Run Generator", false, -1)]
        public static void GenScript(MenuCommand command)
        {
            Transform selectedObj = UnityEditor.Selection.activeTransform;
            if (selectedObj == null) return; 
            var list = Search(selectedObj);
            CodeGenerator(list);
        }


        private static void InitType()
        {
            CompleteCode = " Wait .. ";

            UIScriptGenMemberType.typeList.Clear();
            UIScriptGeneratorEditor.Window?.Repaint();
            if(reflectionFields.Count == 0)
            {
                reflectionFields.Add(typeof(Image).FullName);
                reflectionFields.Add(typeof(Text).FullName); 
            }
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            { 
                for (int n = 0; n < reflectionFields.Count; n++)
                {
                    var type  = assemblies[i].GetType(reflectionFields[n]);
                    if(type != null)
                    {
                        UIScriptGenMemberType.Add(type);
                    }
                } 
            }

           
        }

        private static void CodeGenerator(List<Transform> transforms)
        {
            InitType();
            StringBuilder builder = new StringBuilder();
            if (UIScriptGeneratorEditor.WriteRegion == true)
                builder.AppendLine("#region " + UIScriptGeneratorEditor.Region + "\n");


            for (int i = 0; i < transforms.Count; i++)
            { 
                var compArr = transforms[i].GetComponents<Component>();
                foreach (var component in compArr)
                {
                    string ret = null;

                    if(UIScriptGenMemberType.IsValid(component))
                    { 
                        string componentName = (UIScriptGeneratorEditor.WriteComponentName == false) ? component.name : component.GetType().Name + "_" + component.name;
                        ret = Write(UIScriptGeneratorEditor.AttrStr + UIScriptGeneratorEditor.AplStr, component.GetType().Name, componentName);
                    } 
                    if (ret != null)
                        builder.AppendLine(ret);
                }
            }

            if (UIScriptGeneratorEditor.WriteRegion)
                builder.AppendLine("\n#endregion");

            CompleteCode = builder.ToString();
            UIScriptGeneratorEditor.OpenWindow();

        }



        private static string Write(string accessMidifier, string variableType, string variableName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(accessMidifier + " ");
            builder.Append(variableType + " ");
            builder.Append(variableName + ";");
            return builder.ToString();
        }
    }
}