// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

#ifndef UNITY_SPRITES_INCLUDED
#define UNITY_SPRITES_INCLUDED

#include "UnityCG.cginc"

#ifdef UNITY_INSTANCING_ENABLED

    UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
        // SpriteRenderer.Color while Non-Batched/Instanced.
        UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
        // this could be smaller but that's how bit each entry is regardless of type
        UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
    UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

    #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
    #define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

#endif // instancing

CBUFFER_START(UnityPerDrawSprite)
#ifndef UNITY_INSTANCING_ENABLED
    fixed4 _RendererColor;
    fixed2 _Flip;
#endif
    float _EnableExternalAlpha;
CBUFFER_END

// Material Color.
fixed4 _Color;

// Roundness coefficient
int _RoundnessCoefficient;

//
float _IsRect;
float _Radius;
float _Center;
float _Strength;
float _Offset;

struct appdata_t
{
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 vertex   : SV_POSITION;
    fixed4 color    : COLOR;
    float4 position : TEXCOORD1;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
{
    return float4(pos.xy * flip, pos.z, 1.0);
}

v2f SpriteVert(appdata_t IN)
{
    v2f OUT;

    UNITY_SETUP_INSTANCE_ID (IN);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

    OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
    OUT.position = OUT.vertex;
    OUT.vertex = UnityObjectToClipPos(OUT.vertex);
    OUT.texcoord = IN.texcoord;
    OUT.color = IN.color * _Color * _RendererColor;

    #ifdef PIXELSNAP_ON
    OUT.vertex = UnityPixelSnap (OUT.vertex);
    #endif

    return OUT;
}

sampler2D _MainTex;
sampler2D _AlphaTex;

fixed4 SampleSpriteTexture (float2 uv)
{
    fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
    fixed4 alpha = tex2D (_AlphaTex, uv);
    color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif

    return color;
}

float int_pow(float value, const int power)
{
	for (int i = 1; i < power; ++i)
	{
		value *= value;
	}

	return value;
}

float rounded_rect_alpha(const float2 uv)
{
	float2 relativeUV = uv - float2(0.5, 0.5);

	const float x = int_pow(relativeUV.x, _RoundnessCoefficient);
	const float y = int_pow(relativeUV.y, _RoundnessCoefficient);
	const float r = int_pow(0.5, _RoundnessCoefficient);

	float equation_for_roundness = abs(x) + abs(y);
	return step(equation_for_roundness, r);
}

float custom_rect_alpha(const float2 uv)
{
	float2 relativeUV = uv - float2(0.5, 0.5);

	const float y = int_pow(relativeUV.y, _RoundnessCoefficient);
	const float x = int_pow(relativeUV.x, _RoundnessCoefficient);
	const float r = int_pow(0.5, _RoundnessCoefficient);

	float equation_for_roundness = abs(x) + abs(y);
	return step(equation_for_roundness, r);
}

float variation(float2 v1, float2 v2, float strength, float offset) {
	return sin(
		dot(normalize(v1), normalize(v2)) * strength + offset
	) / 100.0;
}

float paint_circle(float2 uv, float2 center, float rad) {

	float2 diff = center - uv;
	float len = length(diff);

	len += variation(diff, float2(0.0, 1.0), _Strength, _Offset);
	len -= variation(diff, float2(1.0, 0.0), _Strength, _Offset);

	float circle = step(len, rad);
	return circle;
}

fixed4 SpriteFrag(v2f IN) : SV_Target
{
	float2 uv = (IN.texcoord - 0.5) * _Radius + 0.5;
    fixed4 c = SampleSpriteTexture (uv) * IN.color;

	//paint color circle
	c.a *= paint_circle(IN.texcoord, _Center, _Radius);

	//if (_IsRect > 0)
	//	c.a *= rounded_rect_alpha(IN.texcoord);
	//else
	//	c.a *= custom_rect_alpha(IN.texcoord);

    c.rgb *= c.a;
    return c;
}

#endif // UNITY_SPRITES_INCLUDED
