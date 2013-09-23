Shader "Image Effects/SquidTint" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AccumOrig("AccumOrig", Float) = 0.5
		_clearColor("Clear Color", COLOR) = (1,0,0)
		_discardColor("Discard Color", COLOR) = (0,0,1)
	}

    SubShader { 
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
		    BindChannels { 
				Bind "vertex", vertex 
				Bind "texcoord", texcoord
			} 
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
	
			#include "UnityCG.cginc"
	
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD;
			};
	
			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD;
			};
			
			float4 _MainTex_ST; 
			float _AccumOrig;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				float2 coord = v.texcoord;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(coord, _MainTex);
				return o;
			}
	
			sampler2D _MainTex;
			float4 _clearColor;
			float4 _discardColor;
			
			half4 frag (v2f i) : COLOR
			{
				float2 coord = i.texcoord;
				half4 tex = tex2D(_MainTex, coord);
				 
				if (tex[0] == _clearColor[0] && tex[1] == _clearColor[1] && tex[2] == _clearColor[2]) {
					return half4(float3(1, 1, 1), 0.8f);
				} else if (tex[0] == _discardColor[0] && tex[1] == _discardColor[1] && tex[2] == _discardColor[2]) {
					return half4(float3(1, 1, 1), _AccumOrig); 
				} else if (tex[0] != 0) {
					return half4(float3(1, 1, 1), _AccumOrig); 
				} else {
					return half4(tex.rgb, tex.a); 
				}
			
			}
			ENDCG 
		} 
		
	}

Fallback off

}