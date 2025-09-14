#pragma once
#include <cmath>

namespace wb
{
#define PI 3.141592f
	
	static float ConvertDegree(float radian) { return (radian * (180 / PI)); }

	struct Vector2
	{
		float x;
		float y;

		Vector2() :x(0.0f), y(0.0f) {}

		Vector2(float _x, float _y) :x(_x), y(_y) {}

		Vector2 operator+(Vector2 other)
		{
			return Vector2(x + other.x, y + other.y);
		}

		Vector2 operator-(Vector2 other)
		{
			return Vector2(x - other.x, y - other.y);
		}

		Vector2 operator*(float scalar)
		{
			return Vector2(x * scalar, y * scalar);
		}

		Vector2 operator*(Vector2 other)
		{
			return Vector2(x * other.x, y * other.y);
		}


		Vector2 operator/(float scalar)
		{
			return Vector2(x / scalar, y / scalar);
		}

		void operator+=(Vector2 other)
		{
			x += other.x;
			y += other.y;
		}
		void Clear()
		{
			x = 0.0f;
			y = 0.0f;
		}


		static float Distance(const Vector2& a, const Vector2& b)
		{
			float dx = b.x - a.x;
			float dy = b.y - a.y;
			return sqrt(dx * dx + dy * dy);  // 피타고라스 정리
		}
		
		static Vector2 lerp( Vector2& start, Vector2& end, float t)
		{
			return start + (end - start) * t;
		}
		float length() 
		{
			return sqrt(x * x + y * y);
		}

		Vector2 Normalized()
		{
			float len = length();
			if (len == 0) return Vector2(0, 0);  // 길이가 0이면 (0,0) 반환
			
			x /= len;
			y /= len;
			return *this;
		}

		static Vector2 Rotate(Vector2 v, float degree)
		{
			float radian = (degree / 180.0f) * PI;
			v.Normalized();
			float x = cosf(radian) * v.x - sinf(radian) * v.y;
			float y = sinf(radian) * v.x + cosf(radian) * v.y;

			return Vector2(x, y);
		}

		static float Dot(Vector2& v1, Vector2 v2)
		{
			return v1.x * v2.x + v1.y * v2.y;
		}

		static Vector2 Cross(Vector2 v1, Vector2 v2)
		{
			return Vector2(0.0f ,v1.x * v2.y - v1.y * v2.x);
		}
		static Vector2 One;
		static Vector2 Zero;
		static Vector2 Right;
		static Vector2 Left;
		static Vector2 Up;
		static Vector2 Down;
	};
}