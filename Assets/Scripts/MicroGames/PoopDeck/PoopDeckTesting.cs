using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopDeckTesting : MonoBehaviour
{
    [Header("Base")]
    [SerializeField, Tooltip("The mesh renderer of the poop deck")]
    private MeshRenderer meshRenderer;
    [SerializeField, Tooltip("The size of the score texture")]
    private int textureSize = 32;
    [SerializeField, Tooltip("The size of the mop relative to the score texture size. The circle that removes from the albedo will be scaled based on this")]
    private int circleSize = 1;

    [Header("Textures")]
    [SerializeField, Tooltip("This is the texture used to determine score, we don't want to use the full texture as it might cause some preformance issues. Don't put anything in here")]
    private Texture2D scoreTexture;
    [SerializeField, Tooltip("Don't put anything in here, this is where the copy of the texture of the meshRenderer will be stored")]
    private Texture2D albedoTexture;
    [SerializeField, Tooltip("These are the textures of the poo, these will be added to the albedo when a bird poo lands on the surface")]
    private Texture2D[] birdPoo;

#if UNITY_EDITOR
    [SerializeField, Tooltip("Enable this to see the score texture")]
    private bool debugMode = false;
#endif

    public static PoopDeckTesting current
    {
        get
        {
            var curr = FindObjectOfType<PoopDeckTesting>();

            if (curr == null)
            {
                throw new System.Exception($"No {typeof(PoopDeckTesting).Name} found in scene, please add one and set it up");
            }

            return curr;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupTexture();

        InvokeRepeating("RandomBirdPoo", 1f, 0.25f);
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (debugMode && Input.GetMouseButton(0))
        {
            Mop(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
    }
#endif

    void SetupTexture()
    {
        // Create the texture
        scoreTexture = new Texture2D(textureSize, textureSize);
        scoreTexture.wrapMode = TextureWrapMode.Clamp; // We don't want the edges to repeat on the other side!

        // Set all pixels to black
        for (int i = 0; i < scoreTexture.width; i++)
        {
            for (int j = 0; j < scoreTexture.height; j++)
            {
                scoreTexture.SetPixel(i, j, Color.black);
            }
        }

        // Apply the changes
        scoreTexture.Apply();

        // Set the renderer's texture to the one we created
        //meshRenderer.material.SetTexture("_alphaTexture", scoreTexture);
        //meshRenderer.sharedMaterial.mainTexture = texture;

        // Duplicate the original material
        Texture2D original = meshRenderer.material.mainTexture as Texture2D;
        albedoTexture = new Texture2D(original.width, original.height);
        albedoTexture.SetPixels(original.GetPixels());
        albedoTexture.wrapMode = TextureWrapMode.Clamp; // We don't want the edges to repeat on the other side!
        albedoTexture.Apply();

        // Added UnityEditor stuff here so we don't accidently build with debug mode enabled
#if UNITY_EDITOR
        if (debugMode)
        {
            meshRenderer.material.mainTexture = scoreTexture;
        }
        else
        {
#endif
            meshRenderer.material.mainTexture = albedoTexture;
#if UNITY_EDITOR
        }
#endif

        // Old rendering stuff
        // Set the renderer's texture to the one we created
        //meshRenderer.material.SetTexture("_alphaTexture", scoreTexture);
        //meshRenderer.sharedMaterial.mainTexture = texture;

        // Duplicate the original material
        //Texture2D original = meshRenderer.material.GetTexture("_mainTexture") as Texture2D;
        //albedoTexture = new Texture2D(original.width, original.height);
        //albedoTexture.SetPixels(original.GetPixels());
        //albedoTexture.Apply();
        //meshRenderer.material.SetTexture("_mainTexture", albedoTexture);
    }

    Color[] SetCircle(Texture2D texture, int x, int y, int circleSize, Color color)
    {
        List<Color> coloursOverridden = new List<Color>();

        // Get the square area
        int xMin = Mathf.Clamp(x - circleSize, 0, texture.width);
        int xMax = Mathf.Clamp(x + circleSize, 0, texture.width);
        int yMin = Mathf.Clamp(y - circleSize, 0, texture.height);
        int yMax = Mathf.Clamp(y + circleSize, 0, texture.height);

        // Set all pixels in a circle in this square to white. We only loop this square as otherwise we loop through pixels we don't need and that's not gucci,
        // this also means we can have a bigger area with the circleSize having the same impact on performance
        for (int i = xMin; i < xMax; i++)
        {
            for (int j = yMin; j < yMax; j++)
            {
                if (Vector2Int.Distance(new Vector2Int(x, y), new Vector2Int(i, j)) < circleSize)
                {
                    if (texture.GetPixel(i, j) != color)
                    {
                        coloursOverridden.Add(texture.GetPixel(i, j));

                        texture.SetPixel(i, j, color);
                    }
                }
            }
        }

        texture.Apply();

        return coloursOverridden.ToArray();
    }

    void RandomBirdPoo()
    {
        float randomPositionX = Random.Range(0f, 1f);
        float randomPositionY = Random.Range(0f, 1f);

        SetCircle(scoreTexture, (int)(randomPositionX * scoreTexture.width), (int)(randomPositionY * scoreTexture.height), circleSize, Color.yellow);

        //SetCircle(albedoTexture, (int)(randomPositionX * albedoTexture.width), (int)(randomPositionY * albedoTexture.height), circleSize * 10, Color.yellow);
        //SetCircle(albedoTexture, (int)(randomPositionX * albedoTexture.width), (int)(randomPositionY * albedoTexture.height), circleSize, Color.yellow);

        SetDebris(birdPoo[0], (int)(randomPositionX * albedoTexture.width), (int)(randomPositionY * albedoTexture.height));


    }

    void SetDebris(Texture2D debrisTexture, int x, int y)
    {
        //// Get the square area
        //int xMin = Mathf.Clamp(x - debrisTexture.width / 2, 0, albedoTexture.width);
        //int xMax = Mathf.Clamp(x + debrisTexture.width / 2, 0, albedoTexture.width);
        //int yMin = Mathf.Clamp(y - debrisTexture.height / 2, 0, albedoTexture.height);
        //int yMax = Mathf.Clamp(y + debrisTexture.height / 2, 0, albedoTexture.height);

        //for (int i = xMin; i < xMax; i++)
        //{
        //    for (int j = yMin; j < yMax; j++)
        //    {
        //        albedoTexture.SetPixel(debrisTexture.GetPixel());
        //    }
        //}

        int w = debrisTexture.width;
        int h = debrisTexture.height;

        for (int i = -w / 2; i < w / 2; i++)
        {
            for (int j = -h / 2; j < h / 2; j++)
            {
                if (x + i < albedoTexture.width && y + j < albedoTexture.height && x + i > 0 && y + j > 0)
                {
                    Color a = albedoTexture.GetPixel(x + i, y + j);
                    Color b = debrisTexture.GetPixel(i + w / 2, j + h / 2);

                    albedoTexture.SetPixel(x + i, y + j, MergeColour(a, b));
                }
            }
        }

        albedoTexture.Apply();
    }

    Color MergeColour(Color a, Color b)
    {
        Color newColor = new Color();
        newColor.a = MergeColourComponent(a.a, b.a, a.a, b.a);//a.a + b.a * (1 - a.a);
        newColor.r = MergeColourComponent(a.r, b.r, a.a, b.a);
        newColor.g = MergeColourComponent(a.g, b.g, a.a, b.a);
        newColor.b = MergeColourComponent(a.b, b.b, a.a, b.a);

        return newColor;
    }

    float MergeColourComponent(float a, float b, float alphaa, float alphab)
    {
        return (a * (1 - alphab)) + (b * (alphab));
    }

    public int Mop(Ray ray)
    {
        int score = 0;

        // Cast that shit
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // If we don't hit the renderer associated with this, then we don't want to do anything
            if (hit.collider.GetComponent<MeshRenderer>() != meshRenderer) return 0;

            Vector2 textureCoord = hit.textureCoord;

            if (hit.collider.GetComponent<MeshCollider>() && hit.collider.GetComponent<MeshCollider>().convex)
            {
                Transform plane = hit.collider.transform;
                Vector3 size = hit.collider.bounds.size;

                textureCoord.x = 1f - ((plane.InverseTransformPoint(hit.point).x + size.x / 2f) / size.x);
                textureCoord.y = 1f - ((plane.InverseTransformPoint(hit.point).z + size.z / 2f) / size.z);
            }



            // Get the coordinates on the texture
            Vector2Int textureCoordsAlpha = new Vector2Int((int)(textureCoord.x * scoreTexture.width), (int)(textureCoord.y * scoreTexture.height));
            Vector2Int textureCoordsAlbedo = new Vector2Int((int)(textureCoord.x * albedoTexture.width), (int)(textureCoord.y * albedoTexture.height));

            Color[] changedColours = SetCircle(scoreTexture, textureCoordsAlpha.x, textureCoordsAlpha.y, circleSize, Color.clear);
            SetCircle(albedoTexture, textureCoordsAlbedo.x, textureCoordsAlbedo.y, (int)(((float)circleSize / (float)scoreTexture.width) * (float)albedoTexture.width), Color.clear);


            foreach (var color in changedColours)
            {
                if (color == Color.black)
                {
                    score += 1;
                }
                else if (color == Color.yellow)
                {
                    score += 2;
                }
            }
        }

        return score;
    }
}
