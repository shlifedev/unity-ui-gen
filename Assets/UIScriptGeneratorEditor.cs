using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class UIScriptGeneratorEditor : UnityEditor.EditorWindow
{

    public enum AccessPermissionLevel { None, PUBLIC, PRIVATE};
    public enum Attribute { None, SerializedField };
    private static bool writeRegion = false;
    private static string region = "SetRegionName";

    private static AccessPermissionLevel apl;
    private static Attribute attr;
    private static UIScriptGeneratorEditor m_window = null;

    public static AccessPermissionLevel Apl { get => apl; }
    public static string AplStr
    {
        get
        { 
            if (apl == AccessPermissionLevel.None) return null;
            if (apl == AccessPermissionLevel.PRIVATE) return "private";
            if (apl == AccessPermissionLevel.PUBLIC) return "public";

            return null;
        }
    }
    public static Attribute Attr { get => attr;}
    public static string AttrStr
    {
        get {
            if (attr == Attribute.None) return null;
            var ret = "[" + attr.ToString() + "] ";
            return ret;
        }
    }

    public static bool WriteRegion { get => writeRegion; set => writeRegion = value; }
    public static string Region { get => region; set => region = value; }

    [MenuItem("UIScriptGeneratorEditor/Code")]
    public static void OpenWindow()
    {
        if (m_window == null)
            m_window = EditorWindow.GetWindow(typeof(UIScriptGeneratorEditor)) as UIScriptGeneratorEditor;
        m_window.titleContent = new GUIContent("QuickCode", "Make by.CheeseAllergyHamster ");
        m_window.Show();

    }

    private void DrawSourceCodeWindow()
    { 
        GUILayout.Label("Generated Code :)");
        UIScriptGenerator.CompleteCode = EditorGUILayout.TextArea(UIScriptGenerator.CompleteCode);

        GUILayout.Label("Option");

        Region = EditorGUILayout.TextField(Region);
        WriteRegion = GUILayout.Toggle(WriteRegion, "Use Region");
        apl  = (AccessPermissionLevel)EditorGUILayout.EnumPopup("AccessPermissionLevel", Apl);
        attr = (Attribute)EditorGUILayout.EnumPopup("Attribute", Attr);

    }
    private void OnGUI()
    {
        DrawSourceCodeWindow();
    }
}
     


