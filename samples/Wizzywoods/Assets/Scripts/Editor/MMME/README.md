# Model Multiple Material Editor for Unity

For best results, use Unity 2021.1 or higher

This tool allows you to search and remap multiple models in the project view at one time.
There is also an option to extract all of the textures if there are any embedded in the model file. (Only tested with .fbx and material extraction is unavailable)

Apply Material Setting to Multiple Models:
Select 'Tools/Model Multiple Material Editor'.
Select the settings that you wish to apply to your models
(By default, these are set to automatically search your entire project for each embedded material)
Select other models and press 'Apply Settings'.
Editor Window, place it in your 'Scripts/Editor' folder.

Extract embedded textures from models:
Use the top group Extract Textures
Enter the path for the extracted textures, or select a folder in the project view and press the '...' button
Select all models in the project view that have embedded textures
Press 'Extract Textures', each model will have its own folder (name of the model file) for any textures that were extracted

If this tool was helpful, I also make other games and software. Contributions will go to my next in-the-works Stealth title.

Note: If your model exporter package includes an option for "Safe File Names", this will remove any spaces from your material names and the tool 
won't be able to find them since they don't match exactly. 
Such an example is seen with the Unity FBX Exporter and its "Use Compatible Naming" option.