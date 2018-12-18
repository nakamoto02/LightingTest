Shader "Custom/LakeShader" {
	Properties{
		_MainTex("Water Texture", 2D) = "white"{}
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float amp = 0.2*sin(_Time * 25 + v.vertex.x * 100);
			v.vertex.xyz = float3(v.vertex.x, v.vertex.y + amp, v.vertex.z);
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed2 uv = IN.uv_MainTex;
			uv.x += 0.5 * _Time;
			uv.y += 0.5 * _Time;
			o.Albedo = tex2D(_MainTex, uv);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
