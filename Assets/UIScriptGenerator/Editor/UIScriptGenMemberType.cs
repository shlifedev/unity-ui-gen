using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.Unity.Utility
{
    public class UIScriptGenMemberType
    { 
        public static List<System.Type> typeList = new List<System.Type>();  
        public static bool IsValid(UnityEngine.Component component)
        { 
            System.Type compoType =  component.GetType();
            for (int i = 0; i < typeList.Count; i++)
            {
                if (typeList[i] == compoType)
                { 
                    return true;
                }
            } 
            return false;
        }
        public static void Add (System.Type type)
        {
            typeList.Add(type);
        }
    }
}
