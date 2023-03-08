//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
//using UnityEditor.Experimental;
//using UnityEditor.Experimental.AssetImporters;
using System.IO;

[ScriptedImporter(1, "word")]
public class WordBankImporter : ScriptedImporter
{
    //public float m_Scale = 1;
   
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var word = new TextAsset(File.ReadAllText(ctx.assetPath));
        //word.text = (File.ReadAllText(ctx.assetPath)).ToString();

        ctx.AddObjectToAsset("main", word);
        ctx.SetMainObject(word);

    }
}
