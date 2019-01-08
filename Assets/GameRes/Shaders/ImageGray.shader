// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ImageGray"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint" , Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags 
		{ 
			"RenderType"="Transparent" 
			"Queue" = "Transparent"
			"IgnorProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
			Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			
			v2f vert (appdata_t IN)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos( IN.vertex);
				o.texcoord = IN.texcoord;
#ifdef UNITY_HALF_TEXEL_OFFSET
				o.vertex.xy -= (_ScreenParams.zw - 1.0);
#endif
				o.color = IN.color * _Color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half4 color = tex2D(_MainTex , i.texcoord) * i.color;
				float grey = dot(color.rgb, fixed3(0.22, 0.707, 0.071));
				return half4(grey, grey, grey, color.a);
			}
			ENDCG
		}
	}
}
