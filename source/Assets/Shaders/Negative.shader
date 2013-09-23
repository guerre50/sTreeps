Shader "Image Effects/Negative" {
	Properties
	{
		_MainTex( "Texture", 2D ) = "white" {}
	}
 
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry" }
		Lighting Off Fog { Mode Off } Cull Back
		LOD 100
 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma exclude_renderers xbox360 ps3 flash
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float4    _MainTex_ST;
 
			// Convert Color.
			inline fixed4 ConvertNegative( fixed4 rgba )
			{
				fixed3 nega;
				nega.r = 1.0 - rgba.r;
				nega.g = 1.0 - rgba.g;
				nega.b = 1.0 - rgba.b;
				return fixed4( nega, rgba.a );
			}
 
			// Vertex Input.
			struct appdata
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};
 
			// Structure.
			struct v2f
			{
				float4 pos   : SV_POSITION;
				float2 uv    : TEXCOORD0;
			};
 
			// VertexShader.
			v2f vert( appdata v )
			{
				v2f o;
				o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
				o.uv  = TRANSFORM_TEX( v.texcoord, _MainTex );
				return o;
			}
 
			// FragmentShader.
			fixed4 frag( v2f i ) : COLOR
			{
				return ConvertNegative( tex2D( _MainTex, i.uv ) );
			}
			ENDCG
		}
	}
	FallBack off
}