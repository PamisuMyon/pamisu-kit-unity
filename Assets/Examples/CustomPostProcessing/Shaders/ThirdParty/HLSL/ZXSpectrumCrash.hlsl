float4 bmap(float3 c) {
	float gray = dot(c, float3(0.29, 0.59, 0.11));
	return lerp(float4(min(floor(c / _BrightnessThreshold + float3(0.5, 0.5, 0.5)), float3(1.0, 1.0, 1.0)), 0), float4(floor(c + float3(0.5, 0.5, 0.5)), 1.0), step(_BrightnessThreshold, gray));
}

void MinMaxColor_float(Texture2D tex, float2 bv, float2 sv, SamplerState ss, out float4 minc, out float4 maxc, out float bright) {
	minc = float4(1.0, 1.0, 1.0, 1.0);
	maxc = float4(0.0, 0.0, 0.0, 0.0);
	bright = 0.0;

	for (int i = 1; i < 8; i++) {
		for (int j = 0; j < 8; j++) {
			float4 c = bmap(SAMPLE_TEXTURE2D(tex, ss, ((bv + float2(i, j))/sv)).rgb);
			minc = min(c, minc);
			maxc = max(c, maxc);
			bright += c.a;
		}
	}
}

void FMapColor_float(float4 c, out float3 res) {
	res = lerp(c.rgb * _BrightnessThreshold, c.rgb, step(_BrightnessThreshold, c.a));
}