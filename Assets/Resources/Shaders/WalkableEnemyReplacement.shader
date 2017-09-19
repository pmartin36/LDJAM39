Shader "Unlit/WalkableEnemyReplacement"
{
	Properties
	{
		_AggroIndicator("Aggro", Range(0,1)) = 0
		_AggroMultiplier("Aggro Color", Color) = (1,1,1,1)
		_Color("Color", Color) = (1,1,1,1)
	}
		SubShader
	{
		Tags {
			"RenderType" = "Opaque"
			"Enemy" = "True"
		}
		LOD 100

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4 _Color;
			float4 _AggroMultiplier;
			float _AggroIndicator;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;

			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target{
				return _AggroMultiplier;
				float4 mul = smoothstep(float4(1,1,1,1), _AggroMultiplier, saturate(i.uv.y - _AggroIndicator));
				return _Color * mul;
			}
			ENDCG
		}
	}
}
