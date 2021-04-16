Shader "Mobile/Decal Reflective Brightness Saturation"
{
	Properties{
		_MainTex("Base Texture", 2D) = "white" {}
		_BaseBri("Base Brightness", Range(-1, 1)) = 0
		_BaseSat("Base Saturation", Range(0.01, 3)) = 1.0
		_DecalTex("Decal (RGB) Trans (A)", 2D) = "white" {}
		_Cube("ReflectionCube", CUBE) = "black" {}
		_Reflection("Reflection Power", Range(0.01, 1)) = 0.5
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;
		fixed _BaseBri;
		fixed _BaseSat;
		samplerCUBE _Cube;
		fixed _Reflection;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldRefl;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 mytex = tex2D(_MainTex, IN.uv_MainTex) + _BaseBri;
			float3 intensity = dot(mytex.rgb, float3(0.299, 0.587, 0.114));
			o.Albedo = lerp(intensity, mytex.rgb, _BaseSat);
			o.Emission = texCUBE(_Cube, IN.worldRefl).rgb * _Reflection;
			o.Alpha = mytex.a;
		}
		ENDCG

		CGPROGRAM
		#pragma surface surf Lambert alpha:fade

		sampler2D _DecalTex;

		struct Input
		{
			float2 uv_DecalTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_DecalTex, IN.uv_DecalTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}