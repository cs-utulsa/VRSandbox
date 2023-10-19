
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeatmapSettings = HeatmapVisualization.HeatmapTextureGenerator.Settings;


namespace HeatmapVisualization
{
	public class Heatmap : MonoBehaviour
	{
		#region Settings
		[SerializeField]
		private ComputeShader gaussianComputeShader;
		[SerializeField]
		public Vector3Int resolution = new Vector3Int(64, 64, 64);
		[SerializeField]
		[Range(0.0f, 1.0f)]
		public float cutoffPercentage = 1.0f;
		[SerializeField]
		public float gaussStandardDeviation = 1.0f;
        [SerializeField]
        private Gradient colormap;
		[SerializeField]
		public Gradient noDensityGradient;
		[SerializeField]
        public Gradient lowDensityGradient;
		[SerializeField]
		public Gradient midLowDensityGradient;

		[SerializeField]
        public Gradient mediumDensityGradient;

        [SerializeField]
        public Gradient highDensityGradient;
        /*		[SerializeField]
                public Gradient lowDensityGradient = CreateGradient(Color.blue, Color.green);
                [SerializeField]
                public Gradient mediumDensityGradient = CreateGradient(Color.green, Color.yellow);
                [SerializeField]
                public Gradient highDensityGradient = CreateGradient(Color.yellow, Color.red);*/

        [SerializeField]
		private bool renderOnTop = false;
		[SerializeField]
		private FilterMode textureFilterMode = FilterMode.Bilinear;

		private const int colormapTextureResolution = 32;
		#endregion


		#region Globals
		private MeshRenderer ownRenderer;
		private MeshRenderer OwnRenderer { get { if (ownRenderer == null) { ownRenderer = GetComponent<MeshRenderer>(); } return ownRenderer; } }
		public Bounds BoundsFromTransform { get => new Bounds { center = transform.position, size = transform.localScale }; }
		private float maxHeatFromTexture;
		#endregion

		private Gradient continuousGradient;
		private float previousDensity = 0f;
		private const float smoothingFactor = 0.1f;


		private void Awake()
		{
			noDensityGradient = CreateGradient(new Color(0, 0, 0, 0));
			lowDensityGradient = CreateGradient(
				new Color(0, 0, 0.5f, 0.5f),  // Deep blue
				new Color(0, 0.5f, 1, 0.5f),  // Lighter blue
				new Color(0, 0.8f, 0.5f, 0.5f),  // Teal
				new Color(0, 1, 0, 0.5f)  // Green
			);
			midLowDensityGradient = CreateGradient(
				new Color(0, 1, 0, 0.5f),  // Green
				new Color(0.5f, 1, 0.5f, 0.5f),  // Yellowish-green
				new Color(1, 1, 0.5f, 0.5f),  // Light Yellow
				new Color(1, 0.8f, 0, 0.5f)  // Light Orange
);

			mediumDensityGradient = CreateGradient(
				new Color(0, 1, 0, 0.5f),  // Green
				new Color(0.5f, 1, 0, 0.5f),  // Yellowish-green
				new Color(1, 1, 0, 0.5f),  // Yellow
				new Color(1, 0.5f, 0, 0.5f)  // Orange
			);

			highDensityGradient = CreateGradient(
				new Color(1, 0.5f, 0, 0.5f),  // Orange
				new Color(1, 0.25f, 0, 0.5f),  // Darker orange
				new Color(1, 0, 0, 0.5f),  // Red
				new Color(0.5f, 0, 0, 0.5f)  // Dark red
			);

			/*continuousGradient = CreateContinuousGradient();*/
		}

		private void Start()
		{
			OwnRenderer.enabled = false;
		}



/*		private Gradient CreateContinuousGradient()
		{
			Gradient gradient = new Gradient();

			List<GradientColorKey> colorKeys = new List<GradientColorKey>();
			List<GradientAlphaKey> alphaKeys = new List<GradientAlphaKey>();

			// Add keys from lowDensityGradient at the start (0 to 0.33)
			AddGradientKeys(lowDensityGradient, 0f, 0.33f, colorKeys, alphaKeys);

			// Add keys from mediumDensityGradient in the middle (0.33 to 0.66)
			AddGradientKeys(mediumDensityGradient, 0.33f, 0.33f, colorKeys, alphaKeys);

			// Add keys from highDensityGradient at the end (0.66 to 1)
			AddGradientKeys(highDensityGradient, 0.66f, 0.34f, colorKeys, alphaKeys);

			gradient.SetKeys(colorKeys.ToArray(), alphaKeys.ToArray());
			return gradient;
		}*/
/*
		private void AddGradientKeys(Gradient sourceGradient, float start, float range, List<GradientColorKey> colorKeys, List<GradientAlphaKey> alphaKeys)
		{
			foreach (var colorKey in sourceGradient.colorKeys)
			{
				colorKeys.Add(new GradientColorKey(colorKey.color, start + colorKey.time * range));
			}

			foreach (var alphaKey in sourceGradient.alphaKeys)
			{
				alphaKeys.Add(new GradientAlphaKey(alphaKey.alpha, start + alphaKey.time * range));
			}
		}*/

		public void GenerateHeatmap(List<Vector3> points, float density)
		{
			if (points == null)
			{
				return;
			}
		

			/*density = (1 - smoothingFactor) * previousDensity + smoothingFactor * density;*/
			previousDensity = density;
			Debug.Log(density);
            colormap = ModifyColormapBasedOnDensity(density);
            /*colormap = continuousGradient;*/


			//calculate heatmap texture
			HeatmapSettings settings = new HeatmapSettings(BoundsFromTransform, resolution, gaussStandardDeviation);
			HeatmapTextureGenerator heatmapTextureGenerator = new HeatmapTextureGenerator(gaussianComputeShader);
			float[] heats = heatmapTextureGenerator.CalculateHeatTexture(points, settings);

			//create texture object
			Texture3D heatTexture = new Texture3D(settings.resolution.x, settings.resolution.y, settings.resolution.z, TextureFormat.RFloat, false);
			heatTexture.SetPixelData(heats, 0);
			heatTexture.wrapMode = TextureWrapMode.Clamp;
			heatTexture.filterMode = textureFilterMode;
			heatTexture.Apply();

			maxHeatFromTexture = GetMaxValue(heats);

			//apply To material
			SetHeatTexture(heatTexture);
			/*SetColormap();*/
			SetMaxHeat();
			SetRenderOnTop();
			SetTextureFilterMode();

			if (density > 0.05f && !OwnRenderer.enabled) // Check if density is non-zero and Renderer is not enabled
			{
				OwnRenderer.enabled = true;
			}
		}


		private float previousDensityValue = -1f; // Initialize with an invalid value

		private Gradient ModifyColormapBasedOnDensity(float density)
		{
			Gradient currentGradient = DetermineGradientFromDensity(previousDensityValue);
			Gradient targetGradient = DetermineGradientFromDensity(density);

			// If the previous density value is invalid (first run), set the current gradient to the target
			if (previousDensityValue < 0)
			{
				currentGradient = targetGradient;
			}

			// Update the previous density value for the next run
			previousDensityValue = density;

			// Start the gradient transition
			StartCoroutine(TransitionGradientOverTime(currentGradient, targetGradient, 1.0f)); // 1.4 seconds transition

			return targetGradient;
		}

		/*		private Gradient DetermineGradientFromDensity(float density)
				{
					if (density <= 0.05f)
					{
						return noDensityGradient;
					}
					else if (density <= 0.33f)
					{
						return lowDensityGradient;
					}
					else if (density <= 0.66f)
					{
						return mediumDensityGradient;
					}
					else
					{
						return highDensityGradient;
					}
				}*/

		private Gradient DetermineGradientFromDensity(float density)
		{
			if (density <= 0.05f)
			{
				return noDensityGradient;
			}
			else if (density <= 0.33f)
			{
				return lowDensityGradient;
			}
			else if (density <= 0.66f)
			{
				return midLowDensityGradient;
			}
			else
			{
				return mediumDensityGradient;
			}
		}

		public IEnumerator TransitionGradientOverTime(Gradient current, Gradient target, float duration)
		{
			float elapsedTime = 0;

			while (elapsedTime < duration)
			{
				elapsedTime += Time.deltaTime;
				float t = elapsedTime / duration;
				colormap = LerpGradient(current, target, t);
				SetColormap(); // This will update the heatmap's gradient
				yield return null;
			}
			colormap = target; // Ensure the final gradient is set correctly
			SetColormap();
		}





		private Gradient LerpGradient(Gradient a, Gradient b, float t)
		{
			int maxKeys = Mathf.Max(a.colorKeys.Length, b.colorKeys.Length);
			GradientColorKey[] cKeys = new GradientColorKey[maxKeys];
			GradientAlphaKey[] aKeys = new GradientAlphaKey[maxKeys];

			for (int i = 0; i < maxKeys; i++)
			{
				if (i < a.colorKeys.Length && i < b.colorKeys.Length)
				{
					cKeys[i].color = Color.Lerp(a.colorKeys[i].color, b.colorKeys[i].color, t);
					cKeys[i].time = a.colorKeys[i].time; // Assuming both gradients have keys at the same time positions

					aKeys[i].alpha = Mathf.Lerp(a.alphaKeys[i].alpha, b.alphaKeys[i].alpha, t);
					aKeys[i].time = a.alphaKeys[i].time;
				}
				else if (i < a.colorKeys.Length)
				{
					cKeys[i] = a.colorKeys[i];
					aKeys[i] = a.alphaKeys[i];
				}
				else
				{
					cKeys[i] = b.colorKeys[i];
					aKeys[i] = b.alphaKeys[i];
				}
			}

			Gradient result = new Gradient();
			result.SetKeys(cKeys, aKeys);
			return result;
		}

		private Gradient CreateGradient(params Color[] colors)
		{
			Gradient gradient = new Gradient();
			GradientColorKey[] colorKeys = new GradientColorKey[colors.Length];
			GradientAlphaKey[] alphaKeys = new GradientAlphaKey[colors.Length];

			for (int i = 0; i < colors.Length; i++)
			{
				colorKeys[i].color = colors[i];
				colorKeys[i].time = i / (float)(colors.Length - 1);
				alphaKeys[i].alpha = colors[i].a;
				alphaKeys[i].time = i / (float)(colors.Length - 1);
			}

			// Ensure the first and last colors have 0 alpha
			alphaKeys[0].alpha = 0f;
			alphaKeys[colors.Length - 1].alpha = 0f;

			gradient.SetKeys(colorKeys, alphaKeys);
			return gradient;
		}


		private void SetHeatTexture(Texture3D heatTexture)
		{
			Material material = new Material(OwnRenderer.sharedMaterial); //not edit the material asset
			material.SetTexture("_DataTex", heatTexture);
			OwnRenderer.sharedMaterial = material;
		}


        /*		public void SetColormap(Gradient colormap)
                {
                    this.colormap = colormap;
                    SetColormap();
                }*/


        public void SetColormap()
        {
            OwnRenderer.sharedMaterial.SetTexture("_GradientTex", GradientToTexture(colormap, colormapTextureResolution));
        }

        /*		public void SetColormap(Gradient newColormap)
                {
                    this.colormap = newColormap;
                    OwnRenderer.sharedMaterial.SetTexture("_GradientTex", GradientToTexture(colormap, colormapTextureResolution));
                }*/



        public void SetMaxHeat()
		{
			OwnRenderer.sharedMaterial.SetFloat("_MaxHeat", maxHeatFromTexture * cutoffPercentage);
		}


		public void SetRenderOnTop(bool renderOnTop)
		{
			this.renderOnTop = renderOnTop;
			SetRenderOnTop();
		}


		public void SetRenderOnTop()
		{
			if (renderOnTop)
			{
				OwnRenderer.sharedMaterial.DisableKeyword("USE_SCENE_DEPTH");
			}
			else
			{
				OwnRenderer.sharedMaterial.EnableKeyword("USE_SCENE_DEPTH");
			}
		}


		public void SetTextureFilterMode(FilterMode textureFilterMode)
		{
			this.textureFilterMode = textureFilterMode;
			SetTextureFilterMode();
		}


		public void SetTextureFilterMode()
		{
			OwnRenderer.sharedMaterial.GetTexture("_DataTex").filterMode = textureFilterMode;
		}


		/// <summary>
		/// Get the maximum value from an array.
		/// </summary>
		private static float GetMaxValue(float[] heats)
		{
			float maxHeat = 0.0f;

			for (int i = 0; i < heats.Length; i++)
			{
				if (heats[i] > maxHeat)
				{
					maxHeat = heats[i];
				}
			}

			return maxHeat;
		}


		private static Texture2D GradientToTexture(Gradient gradient, int resolution)
		{
			Texture2D texture = new Texture2D(resolution, 1);

			for (int i = 0; i < resolution; i++)
			{
				texture.SetPixel(i, 1, gradient.Evaluate(((float)i) / (resolution - 1)));
			}

			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Bilinear;
			texture.Apply();
			return texture;
		}

	}
}