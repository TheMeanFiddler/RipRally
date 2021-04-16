Shader "Mobile/Reflective Saturation"
{
	Properties{
	_MainTex("Base Texture", 2D) = "white" {}
	_BaseBri("Base Brightness", Range(-1, 1)) = 0
	_BaseSat("Base Saturation", Range(0.01, 3)) = 1.0
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
	fixed _BaseBri;
	fixed _BaseSat;
	fixed _Reflection;

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 mytex = tex2D(_MainTex, IN.uv_MainTex);
		float3 intensity = dot(mytex.rgb, float3(0.299, 0.587, 0.114));
		o.Albedo = lerp(intensity, mytex.rgb, _BaseSat) + _BaseBri;
		o.Emission = texCUBE(_Cube, IN.worldRefl).rgb * _Reflection;
	}
	ENDCG
	}
		Fallback "Diffuse"
}