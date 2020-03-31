Shader "Custom/LavaShader"
{
	Properties
	{
		[Header(Main)]
		_TintStart("Tint Start", Color) = (0, 0, 0, 1)
		_TintEnd("Tint End", Color) = (1, 1, 1, 1)

		_TintOffset("Tint Offset", float) = 0
		_TintStrength("Tint Strength", float) = 1

		_DirectionX("Stream Direction X", Range(-1, 1)) = 1
		_DirectionY("Stream Direction Y", Range(-1, 1)) = 1

		_TopLayerColor("Top Color", Color) = (1, 1, 1, 1)

		_TopLayerCutoff("Top Cutoff", Range(0, 1)) = 0.9
		_TopLayerBlur("Top Blur", Range(0, 1)) = 1
		_TopLayerStrength("Top Strength", float) = 1

		[Header(Noise)]
		_NoiseMap("Noise Map", 2D) = "white" {}

		_NoisePrimaryScale("Primary Scale", float) = 1
		_NoiseSecondaryScale("Secondary Scale", float) = 1
		_NoiseSpeed("Speed", float) = 1

		[Header(Caustic)]
		_CausticMap("Caustic Map", 2D) = "white" {}

		_CausticScale("Scale", Range(0, 1)) = 1
		_CausticSpeed("Speed", Range(-10, 10)) = 1
		_CausticDistortion("Distortion", Range(0, 1)) = 1
		_CausticVertexDistortion("Vertex Distortion", Range(0, 2)) = 1

		[Header(Edge)]
		_EdgeColor("Color", Color) = (1, 1, 1, 1)

		_EdgeBlur("Blur", Range(0, 1)) = 1
		_EdgeWidth("Width", float) = 1
		_EdgeStrength("Strength", float) = 1
	}

		SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"RenderPipeline" = "UniversalPipeline"
			"IgnoreProjector" = "true"
			"Queue" = "Transparent"
		}

		Pass
		{
			Tags
			{
				"LightMode" = "UniversalForward"
			}

			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

			CBUFFER_START(UnityPerMaterial)
			
			float4 _TintStart;
			float4 _TintEnd;

			float _TintOffset;
			float _TintStrength;

			float _DirectionX;
			float _DirectionY;

			float4 _TopLayerColor;

			float _TopLayerCutoff;
			float _TopLayerBlur;
			float _TopLayerStrength;

			float4 _NoiseMap_ST;

			float _NoisePrimaryScale;
			float _NoiseSecondaryScale;
			float _NoiseSpeed;

			float4 _CausticMap_ST;

			float _CausticScale;
			float _CausticSpeed;
			float _CausticDistortion;
			float _CausticVertexDistortion;

			float4 _EdgeColor;

			float _EdgeBlur;
			float _EdgeWidth;
			float _EdgeStrength;

			CBUFFER_END

			TEXTURE2D(_NoiseMap);
			SAMPLER(sampler_NoiseMap);

			TEXTURE2D(_CausticMap);
			SAMPLER(sampler_CausticMap);

			struct Attributes
			{
				float4 positionOS : POSITION;
				float4 color : COLOR;
				float4 texCoord : TEXCOORD;
			};

			struct Varyings
			{
				float4 positionCS : SV_POSITION; //ClipSpace
				float3 positionWS : POSITION1; //WorldSpace
				float4 positionSS : POSITION2; //ScreenSpace
				float4 color : COLOR;
				float2 uvNoise : TEXCOORD;
				float2 uvCaustic : TEXCOORD1;
			};
			
			Varyings vert(Attributes attr)
			{
				Varyings var;

				var.positionCS = TransformObjectToHClip(attr.positionOS.xyz);
				var.positionWS = TransformObjectToWorld(attr.positionOS.xyz);
				var.positionSS = ComputeScreenPos(var.positionCS);
				var.color = attr.color;
				var.uvNoise = TRANSFORM_TEX(var.positionWS.xz, _NoiseMap);
				var.uvCaustic = TRANSFORM_TEX(var.positionWS.xz, _CausticMap);

				return var;
			}

			float4 frag(Varyings var) : SV_TARGET
			{
				float2 streamDirection = normalize(float2(_DirectionX, _DirectionY));
				
				float2 noiseDistort = _Time.x * streamDirection * _NoiseSpeed;

				float2 noisePrimaryUV = var.uvNoise + noiseDistort * _NoisePrimaryScale;
				float noisePrimaryVal = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, noisePrimaryUV).r;

				float2 noiseSecondaryUV = var.uvNoise + noiseDistort * _NoiseSecondaryScale;
				float noiseSecondaryVal = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, noiseSecondaryUV).r;

				float noiseCombinedVal = (noisePrimaryVal + noiseSecondaryVal) * 0.5;

				float2 causticDistort = _Time.x * streamDirection * _CausticSpeed;

				float2 causticUV = var.uvCaustic * _CausticScale + causticDistort + noiseCombinedVal * _CausticDistortion + var.color.r * _CausticVertexDistortion;
				float causticVal = SAMPLE_TEXTURE2D(_CausticMap, sampler_CausticMap, causticUV).r;

				float totalVal = noiseCombinedVal + causticVal;

				float4 color = lerp(_TintStart, _TintEnd, totalVal * _TintOffset) * _TintStrength;

				float2 uv = var.positionSS.xy / var.positionSS.w;
				float rawDepth = SampleSceneDepth(uv);
				float depth = LinearEyeDepth(rawDepth, _ZBufferParams);

				float4 edgeLine = 1 - saturate(_EdgeWidth * (depth - var.positionSS.w));
				float edge = smoothstep(1 - totalVal, 1 - totalVal + _EdgeBlur, edgeLine);

				color *= (1 - edge);
				color += (edge * _EdgeColor) * _EdgeStrength;

				float top = smoothstep(_TopLayerCutoff, _TopLayerCutoff + _TopLayerBlur, totalVal) * var.color.r;

				color *= (1 - top);
				color += top * _TopLayerColor * _TopLayerStrength;

				color *= var.color.r;

				return color;
			}

			ENDHLSL
		}
	}

	Fallback "Hidden/Universal Render Pipeline/FallbackError"
}