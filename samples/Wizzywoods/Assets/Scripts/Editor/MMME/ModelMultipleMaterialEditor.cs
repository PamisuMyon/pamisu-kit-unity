using UnityEngine;
using UnityEditor;

/// <summary>
/// Copyright(c) 2021 Shawn Fox

/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// 
/// If this tool has helped you with your project, please consider donating to help me with my next Stealth Game (It's gonna be cool)
/// https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=QNBCJGGNLJ2LN&item_name=Thanks+for+supporting+me%21&currency_code=AUD
/// Thanks and godspeed to your own project
/// Designed Shawn Fox
/// </summary>

public class ModelMultipleMaterialEditor : EditorWindow
{
    [MenuItem("Tools/Model Multiple Material Editor", false, 2)]
    public static void ShowWindow()
    {
        ggllglg = GetWindow(typeof(ModelMultipleMaterialEditor));
        ggllglg.titleContent = new GUIContent("MMME");
    }
    static EditorWindow ggllglg;
    Vector2 glgllggg;

    ModelImporterMaterialImportMode slsslsls = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
    ModelImporterMaterialName slssllsls = ModelImporterMaterialName.BasedOnMaterialName;
    ModelImporterMaterialSearch slslslls = ModelImporterMaterialSearch.Everywhere;
    ModelImporterMaterialLocation slslllls = ModelImporterMaterialLocation.InPrefab;
    bool bbbbbabb;
    string aaabbbab;

    public void OnSelectionChange()
    {
        Repaint();
    }

    public void OnGUI()
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
        ggllglg.minSize = new Vector2(400, 1);
        ggllglg.maxSize = new Vector2(1500, 1000);

        GUILayout.Label("Model Multiple Material Editor - Designed by Shawn Fox");

        LLOOL();

        GUILayout.Space(16);

        LOOLL();

    }

    void LLOOL()
    {
        GUILayout.BeginVertical("Extract Textures", "window", GUILayout.MaxHeight(64));//T

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        if (GUILayout.Button("Extract all textures"))
        {
            LLOLL();
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Destination", GUILayout.Width(88));
        aaabbbab = GUILayout.TextField(aaabbbab);
        if (GUILayout.Button("...", GUILayout.Width(32)))
        {
            if (LLOLOLO(out string selFolder))
            {
                aaabbbab = selFolder;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    void LLOLL()
    {
        ModelImporter lloll = null;

        try
        {
            if (Selection.objects != null)
            {
                foreach (Object bbebd in Selection.objects)
                {
                    if (bbebd is GameObject)
                    {
                        string pathName = AssetDatabase.GetAssetPath(bbebd);
                        try
                        {
                            lloll = ModelImporter.GetAtPath(pathName) as ModelImporter;
                        }
                        catch (System.Exception ea)
                        {
                            Debug.LogError(AssetDatabase.GetAssetPath(bbebd) + " could not be converted to a model.");
                            continue;
                        }
                        if (lloll == null)
                        {
                            Debug.LogError("Invalid model selected");
                            return;
                        }
                        if (!AssetDatabase.IsValidFolder(aaabbbab + "/" + bbebd.name))
                        {
                            AssetDatabase.CreateFolder(aaabbbab, bbebd.name);
                        }
                        if (!lloll.ExtractTextures(aaabbbab + "/" + bbebd.name))
                        {
                            Debug.LogError("There were no embedded textures to export or the operation failed");
                        }
                        else
                        {
                            Debug.Log("Textures extracted to " + aaabbbab + "/" + bbebd.name);
                        }
                    }
                    else
                    {
                        if (lloll == null)
                        {
                            Debug.LogError("Selected isn't a model");
                            return;
                        }
                    }
                }
            }
            else
            {
                if (lloll == null)
                {
                    Debug.LogError("Nothing selected");
                    return;
                }
            }
        }
        catch (System.Exception ea)
        {
            Debug.LogError("There was a problem processing the operation.\n" + ea);
        }
    }

    private void HandleButtonClickApply()
    {
        ModelImporter lloll = null;
        try
        {
            if (Selection.objects != null)
            {
                foreach (Object beebe in Selection.objects)
                {
                    if (beebe is GameObject)
                    {
                        string bbbee = AssetDatabase.GetAssetPath(beebe);
                        try
                        {
                            lloll = ModelImporter.GetAtPath(bbbee) as ModelImporter;
                        }
                        catch (System.Exception ea)
                        {
                            Debug.LogError(AssetDatabase.GetAssetPath(beebe) + " could not be converted to a model.");
                            continue;
                        }
                        lloll.materialImportMode = slsslsls;
                        lloll.materialLocation = slslllls;
                        if (slsslsls != ModelImporterMaterialImportMode.None)
                        {
                            lloll?.SearchAndRemapMaterials(slssllsls, slslslls);
                        }
                        lloll?.SaveAndReimport();
                    }
                    else
                    {
                        Debug.LogError(AssetDatabase.GetAssetPath(beebe) + " is not a model.");
                    }
                }
                Debug.Log("Applied settings to " + Selection.objects.Length + " models.");
            }
        }
        catch (System.Exception ea)
        {
            Debug.LogError("There was a problem processing the operation.\n" + ea);
        }
    }



    private void LOOLL()
    {
        GUILayout.BeginVertical("Automatically search and remap materials on a model", "window", GUILayout.MaxHeight(1000));//T


        if (bbbbbabb)
        {
            if (GUILayout.Button("Close"))
            {
                bbbbbabb = !bbbbbabb;
            }
            EditorGUILayout.HelpBox("If you are using Unity FBX Exporter or a similar tool, ensure you uncheck the option 'Compatible Naming'. " +
                "It will replace all of the spaces in your material names with underscores and thus this tool will not find " +
                "the correct materials. If you are using Autodesk Maya in your toolset, you should keep this option on and use safe characters in your file names.", MessageType.Info);
        }
        else
        {
            if (GUILayout.Button("Can't find my model's materials?"))
            {
                bbbbbabb = !bbbbbabb;
            }
        }

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Material Creation Mode", GUILayout.Width(150));
        slsslsls = (ModelImporterMaterialImportMode)EditorGUILayout.EnumPopup(slsslsls);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Material Location", GUILayout.Width(150));
        slslllls = (ModelImporterMaterialLocation)EditorGUILayout.EnumPopup(slslllls);
        GUILayout.EndHorizontal();
        GUILayout.Space(8);


        if (slsslsls == ModelImporterMaterialImportMode.None)
        {
            EditorGUILayout.HelpBox("Materials will not be imported. Use Unity's default material instead.", MessageType.Info);
        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Naming", GUILayout.Width(150));
            slssllsls = (ModelImporterMaterialName)EditorGUILayout.EnumPopup(slssllsls);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Search", GUILayout.Width(150));
            slslslls = (ModelImporterMaterialSearch)EditorGUILayout.EnumPopup(slslslls);
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(8);
        if (GUILayout.Button("Apply to selection"))
        {
            HandleButtonClickApply();
        }

        glgllggg = DrawCurrentSelection(glgllggg);

        GUILayout.EndVertical();
    }

    private static string LOLLOLL()
    {
        string ajajjaja = Selection.assetGUIDs[0];
        string ajjajaja = AssetDatabase.GUIDToAssetPath(ajajjaja);
        string jajjajaj = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), ajjajaja);

        System.IO.FileAttributes jajaja = System.IO.File.GetAttributes(jajjajaj);
        return jajaja.HasFlag(System.IO.FileAttributes.Directory) ? jajjajaj : System.IO.Path.GetDirectoryName(jajjajaj);
    }

    public static bool LLOLOLO(out string ooiio)
    {
        ooiio = "";
        try
        {
            var ioioi = LOLLOLL();
            if (ioioi != "")
            {
                ooiio = ioioi.Substring(ioioi.LastIndexOf("Assets")).Replace("/", "\\");
                return true;
            }
            else
            {
                Debug.LogError("Select a folder in the project view.");
                return false;
            }
        }
        catch (System.Exception ea)
        {
            if (ea is System.IndexOutOfRangeException)
            {
                Debug.LogError("Select a folder from the project view.\n" + ea);
            }
            else
            {
                Debug.LogError("The folder you have selected is not accessible.\n" + ea);
            }

            return false;
        }
    }

    public static Vector2 DrawCurrentSelection(Vector2 scsccss)
    {
        if (Selection.objects != null)
        {
            GUILayout.Label("Current selection: (" + Selection.objects.Length + ")");
            scsccss = GUILayout.BeginScrollView(scsccss);
            foreach (Object sbsbsbs in Selection.objects)
            {
                GUILayout.Label(string.Format("{0}: {1}", sbsbsbs.GetType().Name, sbsbsbs.name));
            }
            GUILayout.EndScrollView();
        }
        return scsccss;
    }
}