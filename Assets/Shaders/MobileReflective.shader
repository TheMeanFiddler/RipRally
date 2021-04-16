Shader "Mobile/Reflective"
{
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	_MainStrength("Main Strength", Range(0.01, 2)) = 1.0
	_Cube("ReflectionCube", CUBE) = "black" {}
	_Reflection("Reflection Power", Range(0.01, 1)) = 0.5
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
#pragma surface surf Lambert
		struct Input {
		float2 uv_MainTex;
		float3 worldRefl;
	};
	sampler2D _MainTex;
	samplerCUBE _Cube;
	fixed _MainStrength;
	fixed _Reflection;

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 mytex = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = mytex.rgb * _MainStrength;
		o.Emission = texCUBE(_Cube, IN.worldRefl).rgb * _Reflection;
	}
	ENDCG
	}
		Fallback "Diffuse"
}