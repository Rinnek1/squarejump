using System.Collections;
using UnityEngine;
using TMPro;

public class RainbowText : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float speed = 2f; //speed of the rainbow effect
    public float frequency = 5f; //frequency of the rainbow effect

    void Update()
    {
        if (textMeshPro == null) return;

        string text = textMeshPro.text;
        textMeshPro.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            //calculate a rainbow colour for each character
            float colorShift = Time.time * speed + i * frequency;
            Color32 color = Color.HSVToRGB((colorShift % 1f), 1f, 1f);

            //apply the colour 
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            for (int j = 0; j < 4; j++) //each character has 4 vertices
            {
                textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32[vertexIndex + j] = color;
            }
        }

        //update the mesh with the new colours
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
            textMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
