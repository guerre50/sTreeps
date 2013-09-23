Shader "Custom/FadeAlpha" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		ZTest Always Cull Back ZWrite Off
		Fog { Mode off }
	 	Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
	
	
			struct vertexInput {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
	
			struct vertexOutput {
				float4 vertex : SV_POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			float4 _MainTex_ST;
			float _AccumOrig;
			
			vertexOutput vert (vertexInput input)
			{
				vertexOutput output;
				output.vertex = mul(UNITY_MATRIX_MVP, input.vertex);
				output.texcoord = input.texcoord;//TRANSFORM_TEX(v.texcoord, _MainTex);
				
				return output;
			}
	
			sampler2D _MainTex;
			
			half4 frag (vertexOutput input) : COLOR
			{
				half4 tex = tex2D(_MainTex, float2(input.texcoord));
				
				return half4(tex);
			}
			ENDCG 
        }
	} 

	Fallback off
}
