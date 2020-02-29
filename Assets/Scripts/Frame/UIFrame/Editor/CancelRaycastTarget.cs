﻿using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Reflection;

[InitializeOnLoad]
static internal class UIMenuOptionsExtend
{
    // The reflected dafault methods.
    private static MethodInfo m_miGetDefaultResource = null;
    private static MethodInfo m_miPlaceUIElementRoot = null;

    static UIMenuOptionsExtend()
    {
        Initialize();
    }

    private static void Initialize()
    {
        // Get all loaded assemblies.
        Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        Assembly uiEditorAssembly = null;
        foreach (Assembly assembly in allAssemblies)
        {
            AssemblyName assemblyName = assembly.GetName();
            if ("UnityEditor.UI" == assemblyName.Name)
            {
                uiEditorAssembly = assembly;
                break;
            }
        }

        // Check if we find ui assembly.
        if (null == uiEditorAssembly)
        {
            Debug.LogError("Can not find assembly: UnityEditor.UI.dll");
            return;
        }

        // Get things we need.
        Type menuOptionType = uiEditorAssembly.GetType("UnityEditor.UI.MenuOptions");
        m_miGetDefaultResource = menuOptionType.GetMethod("GetStandardResources", BindingFlags.NonPublic | BindingFlags.Static);
        m_miPlaceUIElementRoot = menuOptionType.GetMethod("PlaceUIElementRoot", BindingFlags.NonPublic | BindingFlags.Static);
    }

    [MenuItem("GameObject/UI/Text", false, 2000)]
    static public void AddText(MenuCommand menuCommand)
    {
        GameObject go = DefaultControls.CreateText((DefaultControls.Resources)m_miGetDefaultResource.Invoke(null, null));
        m_miPlaceUIElementRoot.Invoke(null, new object[] { go, menuCommand });

        // Remove raycast target.
        Text text = go.GetComponent<Text>();
        text.raycastTarget = false;
    }
}