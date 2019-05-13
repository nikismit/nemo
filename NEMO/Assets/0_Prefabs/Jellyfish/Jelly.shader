Shader "Custom/Jelly"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		//_NoiseScale("Noise Scale", float) = 1
	//	_NoiseFrequency("Noise Frequency", float) = 1
		_NormalDist("Normal Distance", float) = 1
		_NoiseOffset("Noise Offset", Vector) = (0,0,0,0)


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull Off
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 4.6
		#include "noiseSimplex.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 tangent : TANGENT;
			float2 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
			float2 texcoord2 : TEXCOORD2;
		};

        sampler2D _MainTex;
		uniform float _NoiseScale, _NoiseFrequency;
		float _NormalDist;
		float4 _NoiseOffset;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

		float N1(float x) { return frac(sin(x)*5346.1764); }
		float N2(float x, float y) { return N1(x + y * 23414.324); }

		const float halfpi = 1.570796326794896619;
		const float pi = 3.141592653589793238;
		const float twopi = 6.283185307179586;

		float N3(float3 p) {
			p = frac(p*0.3183099 + .1);
			p *= 17.0;
			return frac(p.x*p.y*p.z*(p.x + p.y + p.z));
		}

		uniform float _speed; //2.0
		uniform float3 _N3; //(0,0,2)
		uniform float _x; //1.0
		uniform float _pump1, _pump2, _pump3, _pump4; //2.0,0.1,1.0,1.2
		uniform float _swayY1, _swayY2, _swayY3; // 2.0,0.4,0.6
		uniform float _swayXZ1, _swayXZ2; // 1.0,0.2

		void vert(inout appdata v)
		{
				float3 v0 = v.vertex.xyz;
				float3 vPump = v.vertex.xyz;

				// Tangent & Bitangents
				float3 bitangent = cross(v.normal, v.tangent.xyz);
				float3 v1 = v0 + (v.tangent.xyz * _NormalDist);
				float3 v2 = v0 + (bitangent * _NormalDist);

				// 3D Noise & normal recalculation
				float ns0 = _NoiseScale * snoise(float3(v0.x + _NoiseOffset.x, v0.y + _NoiseOffset.y, v0.z + _NoiseOffset.z) * _NoiseFrequency);
				v0.xyz += ((ns0 + 1) / 2) * v.normal;

				float ns1 = _NoiseScale * snoise(float3(v1.x + _NoiseOffset.x, v1.y + _NoiseOffset.y, v1.z + _NoiseOffset.z) * _NoiseFrequency);
				v1.xyz += ((ns1 + 1) / 2) * v.normal;

				float ns2 = _NoiseScale * snoise(float3(v2.x + _NoiseOffset.x, v2.y + _NoiseOffset.y, v2.z + _NoiseOffset.z) * _NoiseFrequency);
				v2.xyz += ((ns2 + 1) / 2) * v.normal;

				float3 vn = cross(v2 - v0, v1 - v0);

				float t = _Time.y * _speed;
				float N = N3(_N3);
				float x = (v.vertex.z + N * twopi)*_x + t;
				//Jelly Pump
				float pump = cos(x + cos(x)) + sin(_pump1*x)*_pump2 + sin(_pump3*x)*_pump4;
				x = t + N * twopi;
				//Jelly Sway
				v0.z -= (cos(x + cos(x)) + sin(_swayY1*x)*_swayY2)*_swayY3;
				v0.xy *= _swayXZ1 + pump * _swayXZ2;

				vPump = v0;
				v.normal = normalize(vPump) * -1;
				v.vertex.xyz = v0;

		}

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
