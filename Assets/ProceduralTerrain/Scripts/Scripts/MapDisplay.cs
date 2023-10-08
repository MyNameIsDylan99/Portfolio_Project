using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {

    #region Fields
    [SerializeField]
	 Renderer textureRender;
    [SerializeField]
     MeshFilter meshFilter;
    [SerializeField]
     MeshRenderer meshRenderer;
    [SerializeField]
    Spawner spawner;
    #endregion

    #region Methods
    public void DrawTexture(Texture2D texture) {
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
	}

	public void DrawMesh(MeshData meshData, Texture2D texture) {
		meshFilter.sharedMesh = meshData.CreateMesh ();
		meshRenderer.sharedMaterial.mainTexture = texture;
        spawner.SetVertices(meshFilter.sharedMesh.vertices);
	}
    #endregion
}
