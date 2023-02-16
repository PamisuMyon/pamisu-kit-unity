inline float2 hash( float2 p ) {
	p=float2(dot(p,float2(127.1,311.7)),dot(p,float2(269.5,183.3)));
	return -1 + 2 * frac(sin(p)*43758.5453123); 
}

void Cellular_float(float2 x, float time, out float res) {
	float2 n = floor(x);
	float2 f = frac(x);
	float dist = 5;

	for (int j = -1; j <= 1; j++)
	for (int i = -1; i <= 1; i++) {
		float2 neighbor = float2(i, j);
		float2 o = hash(n + neighbor);
		float2 diff = neighbor - f + (0.5 + 0.5 * sin(time + 6.2831 * o));
		dist = min(dist, length(diff));
	}

	res = dist;
}

void VoronoiUV_float(float2 x, float time, float cellSize, out float2 res) {
	float2 n = floor(x);
	float2 f = frac(x);
	float dist = 5;

	for (int j = -1; j <= 1; j++)
	for (int i = -1; i <= 1; i++) {
		float2 neighbor = float2(i, j);
		float2 o = hash(n + neighbor);
		float2 p = 0.5 + 0.5 * sin(time + 6.2831 * o);
		float2 diff = neighbor + p - f;

		if (dist > length(diff)) {
			dist = length(diff);
			res = (x + diff) / cellSize;
		}
	}
}

void VoronoiLine_float(float2 x, float time, float cellSize, out float2 res, out float outLine) {
	float2 n = floor(x);
	float2 f = frac(x);
	float dist = 8;
	float2 dMin, nMin;

	for (int j = -1; j <= 1; j++)
	for (int i = -1; i <= 1; i++) {
		float2 neighbor = float2(i, j);
		float2 o = hash(n + neighbor);

		float2 p = 0.5 + 0.5 * sin(time + 6.2831 * o);
		//float2 p = 0.5 + 0.5 * float2(sin(time + 6.2831 * o.x), cos(time + 6.2831 * o.y));
		float2 diff = neighbor + p - f;

		if (dist > length(diff)) {
			dist = length(diff);
			dMin = diff;
			nMin = neighbor;
			res = (x + diff) / cellSize;
		}
	}

	outLine = 8;

	for (int j2 = -2; j2 <= 2; j2++)
	for (int i2 = -2; i2 <= 2; i2++) {
		float2 neighbor = nMin + float2(i2, j2);
		float2 o = hash(n + neighbor);
		float2 p = 0.5 + 0.5 * sin(time + 6.2831 * o);
		//float2 p = 0.5 + 0.5 * float2(sin(time + 6.2831 * o.x), cos(time + 6.2831 * o.y));
		float2 diff = neighbor + p - f;

		float2 cellDiff = abs(nMin - neighbor);

		if (cellDiff.x + cellDiff.y > 0.1) {
			float d = dot(0.5 * (dMin + diff), normalize(diff - dMin));
			outLine = min(outLine, d);
		}
	}
}
