// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Rock_2"
{
	Properties
	{
		_Basecolor("Base color ", Color) = (0,0,0,0)
		_AmbientGlow("Ambient Glow", Color) = (1,0.5514706,0.950507,0)
		_Colorrim("Color rim", Color) = (0.8161765,0.9923934,1,0)
		_Bias("Bias", Range( 0 , 1)) = 0.5
		_Power("_Power", Range( 0 , 10)) = 1
		_Intensity("Intensity", Range( 0 , 20)) = 0.5
		_GlossIntensity("Gloss Intensity", Range( 0 , 2)) = 0
		_Spec("Spec", Range( 0 , 2)) = 0
		_SpecColor("Specular Color",Color)=(1,1,1,1)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Basecolor;
		uniform float4 _AmbientGlow;
		uniform float4 _Colorrim;
		uniform float _Bias;
		uniform float _Intensity;
		uniform float _Power;
		uniform float _Spec;
		uniform float _GlossIntensity;

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Albedo = _Basecolor.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV22 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode22 = ( _Bias + _Intensity * pow( 1.0 - fresnelNdotV22, (12.0 + (_Power - 0.0) * (0.0 - 12.0) / (12.0 - 0.0)) ) );
			float4 lerpResult28 = lerp( _AmbientGlow , _Colorrim , fresnelNode22);
			o.Emission = lerpResult28.rgb;
			o.Specular = _Spec;
			o.Gloss = _GlossIntensity;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf BlinnPhong keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
Version=16301
7;280;765;731;1884.567;155.6856;1.333573;True;False
Node;AmplifyShaderEditor.RangedFloatNode;18;-1462.554,314.223;Float;False;Property;_Power;_Power;7;0;Create;True;0;0;False;0;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1461.554,233.2227;Float;False;Property;_Intensity;Intensity;8;0;Create;True;0;0;False;0;0.5;1.5;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;19;-1162.599,359.6136;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;12;False;3;FLOAT;12;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1462.554,152.2228;Float;False;Property;_Bias;Bias;6;0;Create;True;0;0;False;0;0.5;0.342;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;-942.0616,-227.2222;Float;False;Property;_AmbientGlow;Ambient Glow;1;0;Create;True;0;0;False;0;1,0.5514706,0.950507,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;22;-970.552,111.2228;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;27;-941.4996,-58.97491;Float;False;Property;_Colorrim;Color rim;2;0;Create;True;0;0;False;0;0.8161765,0.9923934,1,0;0.759434,0.9893081,1,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-2178.912,968.1551;Float;False;Property;_Power1;Power 1;5;0;Create;True;0;0;False;0;2;2;-3;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;2;-1390.197,650.4631;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;-1858.912,680.1552;Float;False;Property;_EdgeGlow;Edge Glow;4;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;6;-1842.912,872.1553;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;17;-1858.912,504.1552;Float;False;Property;_EdgeAmbient;Edge Ambient;3;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;28;-305.6441,52.97907;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;3;-437.8285,-257.9572;Float;False;Property;_Basecolor;Base color ;0;0;Create;True;0;0;False;0;0,0,0,0;1,0,0.7204204,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;8;-2242.912,776.1552;Float;False;PerturbNormal;-1;;18;c8b64dd82fb09f542943a895dffb6c06;1,26,0;1;6;FLOAT3;0,0,0;False;4;FLOAT3;9;FLOAT;28;FLOAT;29;FLOAT;30
Node;AmplifyShaderEditor.RangedFloatNode;16;-495.4895,606.6583;Float;False;Property;_GlossIntensity;Gloss Intensity;9;0;Create;True;0;0;False;0;0;0.45;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-496.5132,519.1282;Float;False;Property;_Spec;Spec;10;0;Create;True;0;0;False;0;0;0.244;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1586.912,728.1552;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;BlinnPhong;Rock_2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;11;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;18;0
WireConnection;22;1;21;0
WireConnection;22;2;20;0
WireConnection;22;3;19;0
WireConnection;2;0;17;0
WireConnection;2;1;4;0
WireConnection;6;2;7;0
WireConnection;28;0;26;0
WireConnection;28;1;27;0
WireConnection;28;2;22;0
WireConnection;4;0;5;0
WireConnection;4;1;6;0
WireConnection;0;0;3;0
WireConnection;0;2;28;0
WireConnection;0;3;1;0
WireConnection;0;4;16;0
ASEEND*/
//CHKSM=91190FE149702E77D8DAC334A50C7938E6153F7C