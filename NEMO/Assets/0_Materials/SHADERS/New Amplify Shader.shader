// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "JellyWobble"
{
	Properties
	{
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 14.7
		_lumi("lumi", Range( 0 , 100)) = 0
		_animationspeed("animation speed", Range( 0 , 5)) = 1
		_speed("speed", Range( 0 , 5)) = 2.225604
		_animationscale("animation scale", Range( 0 , 1)) = 1
		_Color0("Color 0", Color) = (0,0,0,0)
		_Color1("Color 1", Color) = (0,0,0,0)
		_metallic("metallic", Range( 0 , 1)) = 0
		_smoothness("smoothness", Range( 0 , 1)) = 0
		_Float3("Float 3", Range( 0 , 10)) = 0
		_Float2("Float 2", Range( 0 , 10)) = 0
		_scale("scale", Range( -20 , 20)) = 0
		_texture("texture", 2D) = "white" {}
		_offset("offset", Range( -20 , 20)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Tessellation.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldNormal;
			float2 uv_texcoord;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Transmission;
			half3 Translucency;
		};

		uniform float _animationspeed;
		uniform float _animationscale;
		uniform float4 _Color1;
		uniform float4 _Color0;
		uniform sampler2D _texture;
		uniform float _speed;
		uniform float _scale;
		uniform float _offset;
		uniform float _lumi;
		uniform float _metallic;
		uniform float _smoothness;
		uniform float _Float2;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform float _Float3;
		uniform float _EdgeLength;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float simplePerlin2D6 = snoise( ( v.texcoord.xy + ( _Time.x * _animationspeed ) ) );
			float3 temp_cast_0 = ((( _animationscale * -1.0 ) + (simplePerlin2D6 - 0.0) * (_animationscale - ( _animationscale * -1.0 )) / (1.0 - 0.0))).xxx;
			v.vertex.xyz += temp_cast_0;
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c + d;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 temp_cast_0 = (_speed).xx;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float2 panner39 = ( _Time.x * temp_cast_0 + ase_vertexNormal.xy);
			float2 temp_cast_2 = ((0.0*_scale + _offset)).xx;
			float2 uv_TexCoord23 = i.uv_texcoord * temp_cast_2;
			float4 lerpResult56 = lerp( _Color1 , _Color0 , ( tex2D( _texture, ( panner39 + uv_TexCoord23 ) ) * _lumi ));
			o.Emission = lerpResult56.rgb;
			o.Metallic = _metallic;
			o.Smoothness = _smoothness;
			float3 temp_cast_4 = (_Float2).xxx;
			o.Transmission = temp_cast_4;
			float3 temp_cast_5 = (_Float3).xxx;
			o.Translucency = temp_cast_5;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustom keepalpha fullforwardshadows noshadow exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandardCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
159;357;1196;1004;-386.0555;1689.535;1.841888;True;False
Node;AmplifyShaderEditor.RangedFloatNode;32;-271.9748,-892.5246;Float;False;Property;_offset;offset;27;0;Create;True;0;0;False;0;0;3;-20;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-267.5276,-980.843;Float;False;Property;_scale;scale;25;0;Create;True;0;0;False;0;0;9.5;-20;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-725.1653,-1136.21;Float;False;Property;_speed;speed;15;0;Create;True;0;0;False;0;2.225604;2.69;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;40;-633.6693,-1451.4;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;41;-657.4434,-1294.473;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;34;-195.8645,-1120.395;Float;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;39;-359.7901,-1371.187;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;49.32438,-1122.24;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-73.07575,-394.1108;Float;False;Property;_animationspeed;animation speed;13;0;Create;True;0;0;False;0;1;3.99;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;2;68.13125,-536.0441;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;320.1313,-497.0442;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;284.7043,-1241.291;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;199.4704,-858.404;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;473.9091,-1263.256;Float;True;Property;_texture;texture;26;0;Create;True;0;0;False;0;001f03db475ce144fbf5565ada18d1c6;001f03db475ce144fbf5565ada18d1c6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;556.6738,-1013.387;Float;False;Property;_lumi;lumi;12;0;Create;True;0;0;False;0;0;3.2;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;102.505,-255.5745;Float;False;Property;_animationscale;animation scale;16;0;Create;True;0;0;False;0;1;0.002;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;5;519.9252,-619.3789;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;45;732.8842,-1408.625;Float;False;Property;_Color0;Color 0;19;0;Create;True;0;0;False;0;0,0,0,0;0.9339623,0.004405493,0.8610561,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;921.8593,-1263.951;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;388.0666,-188.7851;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;57;715.5181,-1587.494;Float;False;Property;_Color1;Color 1;20;0;Create;True;0;0;False;0;0,0,0,0;0.7075917,0.2432805,0.7264151,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;6;578.2875,-804.4096;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;1131.268,-924.9735;Float;False;Property;_metallic;metallic;21;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2438.053,-514.1849;Float;False;Property;_Float0;Float 0;14;0;Create;True;0;0;False;0;2.225604;1.97;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;1132.268,-852.9735;Float;False;Property;_smoothness;smoothness;22;0;Create;True;0;0;False;0;0;0.532;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;900.5085,-1032.588;Float;False;Constant;_Float4;Float 4;26;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;1157.558,-597.2887;Float;False;Property;_Float3;Float 3;23;0;Create;True;0;0;False;0;0;3.42;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;1145.962,-680.7019;Float;False;Property;_Float2;Float 2;24;0;Create;True;0;0;False;0;0;4.01;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectScaleNode;33;-1526.053,-898.1849;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1766.053,-450.185;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-20;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;11;-2358.053,-658.1849;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;56;1655.663,-1374.078;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;25;-1702.053,-866.185;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;8;869.2823,-802.9254;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-1334.053,-578.1849;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2054.053,-354.1846;Float;False;Property;_Float1;Float 1;17;0;Create;True;0;0;False;0;1;-4.8;-20;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;1085.234,-1019.797;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-1798.053,-738.1849;Float;True;Property;_TextureSample0;Texture Sample 0;18;0;Create;True;0;0;False;0;001f03db475ce144fbf5565ada18d1c6;001f03db475ce144fbf5565ada18d1c6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;10;-2070.053,-738.1849;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalVertexDataNode;14;-2342.053,-818.1848;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2118.201,-1143.528;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;JellyWobble;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;14.7;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;0;-1;7;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;34;1;31;0
WireConnection;34;2;32;0
WireConnection;39;0;40;0
WireConnection;39;2;38;0
WireConnection;39;1;41;1
WireConnection;23;0;34;0
WireConnection;3;0;2;1
WireConnection;3;1;4;0
WireConnection;42;0;39;0
WireConnection;42;1;23;0
WireConnection;22;1;42;0
WireConnection;5;0;44;0
WireConnection;5;1;3;0
WireConnection;51;0;22;0
WireConnection;51;1;47;0
WireConnection;9;0;7;0
WireConnection;6;0;5;0
WireConnection;18;0;16;0
WireConnection;56;0;57;0
WireConnection;56;1;45;0
WireConnection;56;2;51;0
WireConnection;8;0;6;0
WireConnection;8;3;9;0
WireConnection;8;4;7;0
WireConnection;19;0;21;0
WireConnection;19;3;18;0
WireConnection;19;4;16;0
WireConnection;21;1;10;0
WireConnection;10;0;14;2
WireConnection;10;2;12;0
WireConnection;10;1;11;1
WireConnection;0;2;56;0
WireConnection;0;3;49;0
WireConnection;0;4;50;0
WireConnection;0;6;53;0
WireConnection;0;7;66;0
WireConnection;0;11;8;0
ASEEND*/
//CHKSM=A49CE587E2E7445D84A2F69C82AFED0A8EB1D3E8