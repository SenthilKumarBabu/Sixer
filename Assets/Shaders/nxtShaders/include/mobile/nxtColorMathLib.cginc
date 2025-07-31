#ifndef NXTCOLORMATHLIB_INCLUDE
#define NXTCOLORMATHLIB_INCLUDE

#include "UnityCG.cginc"

half mobHueDriver(half3 OriginalRGB, half3 DriverRGB)
{
	half3 HSV;

	half minChannel, maxChannel;
	if (OriginalRGB.x > OriginalRGB.y)
	{
		maxChannel = OriginalRGB.x;
		minChannel = OriginalRGB.y;
	}
	else
	{
		maxChannel = OriginalRGB.y;
		minChannel = OriginalRGB.x;
	}

	if (OriginalRGB.z > maxChannel) maxChannel = OriginalRGB.z;
	if (OriginalRGB.z < minChannel) minChannel = OriginalRGB.z;

	HSV.xy = 0;
	HSV.z = maxChannel;
	half delta = maxChannel - minChannel;              //Delta RGB value
	if (delta != 0)                                    // If gray, leave H  S at zero 
	{
		HSV.y = delta / HSV.z;
		half3 delRGB;
		delRGB = (HSV.zzz - OriginalRGB + 3 * delta) / (6.0*delta);
		if (OriginalRGB.x == HSV.z) HSV.x = delRGB.z - delRGB.y;
		else if (OriginalRGB.y == HSV.z) HSV.x = (1.0 / 3.0) + delRGB.x - delRGB.z;
		else if (OriginalRGB.z == HSV.z) HSV.x = (2.0 / 3.0) + delRGB.y - delRGB.x;
	}
	return (HSV);
}

half3 mobRGB2HSV(half3 RGB)
{
	half3 HSV;

	half minChannel, maxChannel;
	if (RGB.x > RGB.y)
	{
		maxChannel = RGB.x;
		minChannel = RGB.y;
	}
	else
	{
		maxChannel = RGB.y;
		minChannel = RGB.x;
	}

	if (RGB.z > maxChannel) maxChannel = RGB.z;
	if (RGB.z < minChannel) minChannel = RGB.z;

	HSV.xy = 0;
	HSV.z = maxChannel;
	half delta = maxChannel - minChannel;              //Delta RGB value
	if (delta != 0)                                    // If gray, leave H  S at zero 
	{
		HSV.y = delta / HSV.z;
		half3 delRGB;
		delRGB = (HSV.zzz - RGB + 3 * delta) / (6.0*delta);
		if (RGB.x == HSV.z) HSV.x = delRGB.z - delRGB.y;
		else if (RGB.y == HSV.z) HSV.x = (1.0 / 3.0) + delRGB.x - delRGB.z;
		else if (RGB.z == HSV.z) HSV.x = (2.0 / 3.0) + delRGB.y - delRGB.x;
	}
	return (HSV);
}


half3 mobHSV2RGB(half3 HSV)
{
	half3 RGB = HSV.z;

	half var_h = HSV.x * 6;
	half var_i = floor(var_h);
	half var_1 = HSV.z * (1.0 - HSV.y);
	half var_2 = HSV.z * (1.0 - HSV.y * (var_h - var_i));
	half var_3 = HSV.z * (1.0 - HSV.y * (1 - (var_h - var_i)));
	if (var_i == 0) { RGB = half3(HSV.z, var_3, var_1); }
	else if (var_i == 1) { RGB = half3(var_2, HSV.z, var_1); }
	else if (var_i == 2) { RGB = half3(var_1, HSV.z, var_3); }
	else if (var_i == 3) { RGB = half3(var_1, var_2, HSV.z); }
	else if (var_i == 4) { RGB = half3(var_3, var_1, HSV.z); }
	else { RGB = half3(HSV.z, var_1, var_2); }

	return (RGB);
}

#endif
