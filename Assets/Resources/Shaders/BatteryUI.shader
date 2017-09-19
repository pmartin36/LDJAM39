Shader "Unlit/BatteryUI"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		_Color("Tint", Color) = (0,1,0,1)
		_Pct("Percent", Range(0,1)) = 1

		[ToggleOff] _Blinked("Blinked", Range(0,1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color    : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _Color;
			float _Pct;

			float _Blinked;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color * _Color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				if (_Blinked > 0.5) {
					discard;
				}

				fixed4 col = tex2D(_MainTex, i.uv) * i.color;
				if (distance(col.a, 0.5) < 0.1) {
					if (i.uv.y > _Pct) {
						discard;
					}
					else {
						col = i.color;
					}
				}
				
				return col;
			}
			ENDCG
		}
	}
}
